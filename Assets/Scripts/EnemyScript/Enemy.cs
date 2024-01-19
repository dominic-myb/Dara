using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public HealthBar healthBar;
    private EnemyChase enemyChase;
    private CapsuleCollider2D capsuleCollider2D;
    [SerializeField]private GameObject player; 
    [SerializeField]private Character character;
    private int maxHealth = 100;        
    public int currentHealth;
    private int expAmount = 30;         //have manager of this
    public int damage = 5;              //have manager of this
    
    private void Start() {
        if (animator == null) {
            Debug.LogError("Animator not assigned in Inspector. Assign it!");
        }
        if (healthBar == null) {
            Debug.LogError("HealthBar not assigned in Inspector. Assign it!");
        }
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        enemyChase = GetComponent<EnemyChase>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }
   
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthBar.SetHealth(currentHealth);
        Hurt();
        if(currentHealth <= 0){
                this.healthBar.enabled = false;
                this.enemyChase.enabled = false;
                this.capsuleCollider2D.enabled = false;
                Defeated(); 
            }
    }
    public void Hurt()
    {
        animator.SetTrigger("Hurt");
    }
    public void Defeated()
    {
        animator.SetTrigger("Defeated");
        GetComponent<ItemBag>().InstantiateLoot(transform.position);
        ExpManager.Instance.AddExperience(expAmount);
    }
    public void RemoveEnemy()
    {
        Destroy(gameObject);
    }
    public void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        { 
            character.TakeDamage(damage);
        }
    }
}
