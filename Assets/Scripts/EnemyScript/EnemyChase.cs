using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private GameObject player;
    private Rigidbody2D rb;
    private readonly float speed = 1f;
    public float distanceBetween = 2f;
    public float distance;
    private float previousX;
    private bool hasLineOfSight = false;
    public Transform moveSpot;
    private float waitTime;
    [SerializeField] private float startWaitTime = 3;
    public float minX, maxX, minY, maxY;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        waitTime = startWaitTime;
        moveSpot.position = new UnityEngine.Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }
    private void Update()
    {
        if (distance < distanceBetween && hasLineOfSight)
        {
            StartMoving();
            transform.position = UnityEngine.Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            waitTime = startWaitTime;
        }
        else
        {
            StopMoving();
            if (waitTime <= 0) StartPatrol();
            else waitTime -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        distance = UnityEngine.Vector2.Distance(transform.position, player.transform.position);
        UnityEngine.Vector2 direction = player.transform.position - transform.position;

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
        hasLineOfSight = !hitBlock;

        if (distance < distanceBetween && hasLineOfSight)
        {
            Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
        }
        else
        {
            Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
        }
    }
    private void StartMoving()
    {
        float currentX = transform.position.x;
        if (currentX > previousX) spriteRenderer.flipX = false;
        else if (currentX < previousX) spriteRenderer.flipX = true;
        previousX = currentX;
        animator.SetBool("isMoving", true);
    }
    private void StopMoving()
    {
        animator.SetBool("isMoving", false);
    }
    private void StartPatrol()
    {
        StartMoving();
        transform.position = UnityEngine.Vector2.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);
        if (UnityEngine.Vector2.Distance(transform.position, moveSpot.position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                moveSpot.position = new UnityEngine.Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
