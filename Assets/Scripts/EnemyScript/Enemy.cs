using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Enemy : MonoBehaviour
{
    //*OBJECTS*//
    private EnemyChase _enemyChase;
    private CapsuleCollider2D _enemyCollider;
    [SerializeField] private GameObject _player;
    [SerializeField] private Character _character;
    public Animator animator;
    public HealthBar healthBar;

    //*PRIVATE*//
    private int _maxHealth = 100;        //!have manager of this
    private int _expAmount = 30;         //!have manager of this

    //*PUBLIC*//
    public int damage = 5;               //!have manager of this
    public int currentHealth;
    private void Start()
    {
        if (animator == null) Debug.LogError("Animator not assigned in Inspector.");
        if (healthBar == null) Debug.LogError("HealthBar not assigned in Inspector.");
        _enemyCollider = GetComponent<CapsuleCollider2D>();
        _enemyChase = GetComponent<EnemyChase>();
        animator = GetComponent<Animator>();
        currentHealth = _maxHealth;
        healthBar.SetMaxHealth(_maxHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthBar.SetHealth(currentHealth);
        HurtAnimation();
        if (currentHealth <= 0)
        {
            healthBar.enabled = false;
            _enemyChase.enabled = false;
            _enemyCollider.enabled = false;
            EnemyDefeated();
        }
    }
    public void HurtAnimation()
    {
        animator.SetTrigger("Hurt");
    }
    public void EnemyDefeated()
    {
        animator.SetTrigger("Defeated");
        GetComponent<ItemBag>().InstantiateLoot(transform.position);
        ExpManager.Instance.AddExperience(_expAmount);
    }
    public void RemoveEnemy()
    {
        Destroy(gameObject);
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _character.TakeDamage(damage);
        }
    }
}
