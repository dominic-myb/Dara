using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]int currentHealth, maxHealth = 100, currentExp, maxExp = 100, currentLvl = 1;
    private Animator animator;
    public HealthBar healthBar;
    public ExpBarGradient expBarGradient;
    private void Start() {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        healthBar.SetMaxHealth(maxHealth);
        expBarGradient.SetMaxExp(maxExp,currentExp);
    }
    private void HandleExpChange(int newExp){
        currentExp += newExp;
        expBarGradient.SetExp(currentExp);
        if(currentExp>=maxExp){
            LevelUp();
        }
    }
    public void TakeDamage(int amount){
        //Sound Effect Here
        currentHealth-=amount;
        healthBar.SetHealth(currentHealth);
        if(currentHealth<=0){
            Defeated();
        }
    }
    public void Heal(int amount){
        //Sound Effect here
        currentHealth+=amount;

        if(currentHealth>maxHealth){
            currentHealth=maxHealth;
        }
    }
    private void LevelUp()
    {
        maxHealth += 10;                            // add 10 to max health
        currentHealth = maxHealth;                  //full health after kill
        currentLvl++;                               //level up
        currentExp %= maxExp;                       //currentExp updates to remaining exp
        maxExp += 100;                              //Expbar updates
        expBarGradient.SetMaxExp(maxExp,currentExp);//Set max Expbar to new
        expBarGradient.SetExp(currentExp);          //to show the exp updates when lvl up
        healthBar.SetHealth(currentHealth);         //to show the healthbar updates when lvl up
        print("Level: " + currentLvl);              //delete this if final
    }
    private void OnEnable() {
        ExpManager.Instance.OnExpChange += HandleExpChange;
    }
    private void OnDisable() {
        ExpManager.Instance.OnExpChange -= HandleExpChange;
    }
    public void RemovePlayer(){
        if(gameObject!=null){
            Destroy(gameObject);
        }else{
            Debug.Log("Player not Set!");
        }
        
    }
    private void Defeated(){
        animator.SetTrigger("Defeated");
    }
    IEnumerator Damage(){
        yield return null;
    }
    
}
