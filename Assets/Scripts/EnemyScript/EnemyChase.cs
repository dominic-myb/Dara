using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// Todo List
//! 1. Enemy must target the player collider instead of player transform position, to get away with stuck issue
//! 2. I apply _playerCollider currently, still not working  
//? 3. apply navmesh to the game 

public class EnemyChase : MonoBehaviour
{
    //*OBJECTS*//
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private GameObject _player;
    private Collider2D _playerCollider;
    private Rigidbody2D _rigidBody;
    private Character _character;
    public Transform moveSpot;

    //*PRIVATE*//
    private readonly float _moveSpeed = 1f; //movement speed of enemy
    private float _previousX; //horizontal orientation of sprite
    private float _waitTime;
    private bool _hasLineOfSight = false; //enemy to player raycast
    [SerializeField] private float startWaitTime = 3; //time for changing position
    [SerializeField] private float minX, maxX, minY, maxY; //area of patrol

    //*PUBLIC*//
    public float distanceBetween = 2f; //enemy to player
    public float distance;
    public bool shouldMove;
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerCollider = _player.GetComponent<Collider2D>();
        _character = _player.GetComponent<Character>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _waitTime = startWaitTime;
        moveSpot.position = new UnityEngine.Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        if (_player == null) Debug.LogError("Player not assigned in the Inspector.");
        if (_playerCollider == null) Debug.LogError("Player not assigned in the Inspector.");
    }
    private void Update()
    {
        EnemyMove();
    }
    private void FixedUpdate()
    {
        SightAndDistanceCheck();
    }
    private void EnemyMove()
    {
        if (distance < distanceBetween && _hasLineOfSight && shouldMove && _character.isAlive)
        {
            StartMovingAnimation();
            transform.position = UnityEngine.Vector2.MoveTowards(transform.position, _playerCollider.transform.position, _moveSpeed * Time.deltaTime);
            _waitTime = startWaitTime;
        }
        else
        {
            if (_character.isAlive) StopMovingAnimation();
            if (_waitTime <= 0 || !_character.isAlive) StartPatrol();
            else _waitTime -= Time.deltaTime;
        }
    }
    private void SightAndDistanceCheck()
    {
        if (_player != null)
        {
            distance = UnityEngine.Vector2.Distance(transform.position, _playerCollider.transform.position);
            UnityEngine.Vector2 direction = _playerCollider.transform.position - transform.position;
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction.normalized, distance);
            bool hitBlock = false;
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Block"))
                    {
                        hitBlock = true;
                        break;
                    }
                }
            }
            _hasLineOfSight = !hitBlock;
            shouldMove = !hitBlock;
            //!Remove this when final
            if (distance < distanceBetween && _hasLineOfSight && shouldMove)
            {
                Debug.DrawRay(transform.position, direction, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, direction, Color.red);
            }
        }
    }
    private void StartMovingAnimation()
    {
        float currentX = transform.position.x;
        if (currentX > _previousX) _spriteRenderer.flipX = false;
        else if (currentX < _previousX) _spriteRenderer.flipX = true;
        _previousX = currentX;
        _animator.SetBool("isMoving", true);
    }
    private void StopMovingAnimation()
    {
        _animator.SetBool("isMoving", false);
    }
    private void StartPatrol()
    {
        UnityEngine.Vector2 direction = moveSpot.position - transform.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction.normalized, distance);
        bool hitBlock = false;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Block"))
                {
                    hitBlock = true;
                    break;
                }
            }
        }
        shouldMove = !hitBlock;
        if (shouldMove)
        {
            StartMovingAnimation();
            transform.position = UnityEngine.Vector2.MoveTowards(transform.position, moveSpot.position, _moveSpeed * Time.deltaTime);
            if (UnityEngine.Vector2.Distance(transform.position, moveSpot.position) < 0.2f)
            {
                if (_waitTime <= 0)
                {
                    moveSpot.position = new UnityEngine.Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                    _waitTime = startWaitTime;
                }

                else _waitTime -= Time.deltaTime;
            }
        }
        else
        {
            if (_waitTime <= 0)
            {
                moveSpot.position = new UnityEngine.Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                _waitTime = startWaitTime;
            }

            else _waitTime -= Time.deltaTime;
        }
    }
}
