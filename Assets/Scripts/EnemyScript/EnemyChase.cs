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
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (distance < distanceBetween && hasLineOfSight)
        {
            StartFollow();
            transform.position = UnityEngine.Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            StopFollow();
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
    private void StartFollow()
    {
        float currentX = transform.position.x;
        if (currentX > previousX) spriteRenderer.flipX = false;
        else if (currentX < previousX) spriteRenderer.flipX = true;
        previousX = currentX;
        animator.SetBool("isMoving", true);
    }
    private void StopFollow()
    {
        animator.SetBool("isMoving", false);
    }
}
