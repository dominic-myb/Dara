using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    //*OBJECTS*//
    private Vector2 _rightAttackOffset;
    public Collider2D swordCollider;

    //*PUBLIC*//
    public int damage = 20; //!have a manager of this

    private void Start()
    {
        _rightAttackOffset = transform.position;
    }
    public void AttackRight()
    {
        swordCollider.enabled = true;
        transform.localPosition = _rightAttackOffset;
    }
    public void AttackLeft()
    {
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(_rightAttackOffset.x * -1, _rightAttackOffset.y);
    }
    public void StopAttack()
    {
        swordCollider.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
