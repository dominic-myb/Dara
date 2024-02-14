using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //*OBJECTS*//
    private Animator animator;
    public HealthBar healthBar;
    public ExpBarGradient expBarGradient;
    public bool isAlive = true;

    //*PRIVATE*//
    [SerializeField] private int _currentHealth, _maxHealth = 100, _currentExp, _maxExp = 100, _currentLvl = 1; //!Have a manager of this

    private void Start()
    {
        _currentHealth = _maxHealth;
        animator = GetComponent<Animator>();
        healthBar.SetMaxHealth(_maxHealth);
        expBarGradient.SetMaxExp(_maxExp, _currentExp);
    }
    private void HandleExpChange(int newExp)
    {
        _currentExp += newExp;
        expBarGradient.SetExp(_currentExp);
        if (_currentExp >= _maxExp)
        {
            LevelUp();
        }
    }
    public void TakeDamage(int amount)
    {
        //Sound Effect Here
        _currentHealth -= amount;
        healthBar.SetHealth(_currentHealth);
        if (_currentHealth <= 0)
        {
            isAlive = false;
            Defeated();
        }
    }
    public void Heal(int amount)
    {
        //Sound Effect here
        _currentHealth += amount;

        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }
    private void LevelUp()
    {
        _maxHealth += 10;                            // add 10 to max health
        _currentHealth = _maxHealth;                  //full health after kill
        _currentLvl++;                               //level up
        _currentExp %= _maxExp;                       //currentExp updates to remaining exp
        _maxExp += 100;                              //Expbar updates
        expBarGradient.SetMaxExp(_maxExp, _currentExp);//Set max Expbar to new
        expBarGradient.SetExp(_currentExp);          //to show the exp updates when lvl up
        healthBar.SetHealth(_currentHealth);         //to show the healthbar updates when lvl up
        print("Level: " + _currentLvl);              //!delete this if final
    }
    private void OnEnable()
    {
        ExpManager.Instance.OnExpChange += HandleExpChange;
    }
    private void OnDisable()
    {
        ExpManager.Instance.OnExpChange -= HandleExpChange;
    }
    public void RemovePlayer()
    {
        if (gameObject != null) Destroy(gameObject);
        else Debug.Log("Player not Set!");
    }
    private void Defeated()
    {
        animator.SetTrigger("Defeated");
    }
    IEnumerator Damage()//!delete this if final
    {
        yield return null;
    }

}
