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
    private float speed = .5f; 
    public float distanceBetween = 2f; 
    public float distance;
    private float previousX; 
    private bool hasLineOfSight = false;
    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player"); 
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update() {
        if(distance < distanceBetween){
            EnemyDirection();
            StartMove();
            // if(hasLineOfSight){
                transform.position = UnityEngine.Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime); 
            // }
        }else{
            StopMove();
        }
        
    }
    private void FixedUpdate() {
        distance = UnityEngine.Vector2.Distance(transform.position, player.transform.position); 
        UnityEngine.Vector2 direction = player.transform.position - transform.position; 
    
        // RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction);
        // foreach (RaycastHit2D hit in hits){
        //     if(hit.collider!=null){
        //         if(hit.collider.CompareTag("Player")){
        //             hasLineOfSight = true;
        //         }else{
        //             hasLineOfSight = false;
        //         }
        //     }
        // }
        
        Debug.DrawRay(transform.position, player.transform.position - transform.position, hasLineOfSight ? Color.green : Color.red);
    }
    private void EnemyDirection(){
        float currentX = transform.position.x; 
        if(currentX > previousX) spriteRenderer.flipX = false;
        else if(currentX < previousX) spriteRenderer.flipX = true;
        previousX = currentX;
    }
    private bool InRange(){
        if(distance < distanceBetween/2){
            return true;
        }else{
            return false;
        }
        
    }
    private void StartMove(){
        animator.SetBool("isMoving",true);
    }
    private void StopMove(){
        animator.SetBool("isMoving",false);
    }
}
