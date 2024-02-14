using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    //*OBJECTS*//
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private GameObject _player;
    [SerializeField]private BoxCollider2D _playerCollider;
    private Rigidbody2D _rigidBody;
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
        if (_player != null)
        {
            _playerCollider = _player.GetComponent<BoxCollider2D>();
            if (_playerCollider != null)
            {
                _player = _playerCollider.gameObject;
            }
        }
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _waitTime = startWaitTime;
        moveSpot.position = new UnityEngine.Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
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
        if (distance < distanceBetween && _hasLineOfSight && shouldMove)
        {
            StartMovingAnimation();
            transform.position = UnityEngine.Vector2.MoveTowards(transform.position, _player.transform.position, _moveSpeed * Time.deltaTime);
            _waitTime = startWaitTime;
        }
        else
        {
            StopMovingAnimation();
            if (_waitTime <= 0) StartPatrol();
            else _waitTime -= Time.deltaTime;
        }
    }
    private void SightAndDistanceCheck()
    {
        distance = UnityEngine.Vector2.Distance(transform.position, _player.transform.position);
        UnityEngine.Vector2 direction = _player.transform.position - transform.position;
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
