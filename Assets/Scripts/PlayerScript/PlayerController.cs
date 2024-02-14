using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //*OBJECTS*//
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Vector2 _movementInput;
    private Rigidbody2D _rigidBody;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    //*PRIVATE*//
    private bool canMove = true;
    private float collisionOffset = 0.02f;

    //*PUBLIC*//
    public float moveSpeed = 1f;            //!have a manager of this

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }
    private void PlayerMove()
    {
        if (canMove)
        {
            if (_movementInput != Vector2.zero)
            {
                bool success = TryMove(_movementInput);
                if (!success)
                {
                    success = TryMove(new Vector2(_movementInput.x, 0));
                }
                if (!success)
                {
                    success = TryMove(new Vector2(0, _movementInput.y));
                }
                _animator.SetBool("isMoving", success);
            }
            else
            {
                _animator.SetBool("isMoving", false);
            }
            
            if (_movementInput.x < 0)
            {
                _spriteRenderer.flipX = true;
            }
            else if (_movementInput.x > 0)
            {
                _spriteRenderer.flipX = false;
            }
        }
    }
    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = _rigidBody.Cast(
                    direction,
                    movementFilter,
                    castCollisions,
                    moveSpeed * Time.fixedDeltaTime + collisionOffset
                );

            if (count == 0)
            {
                _rigidBody.MovePosition(_rigidBody.position + moveSpeed * Time.fixedDeltaTime * direction);
                return true;
            }

            else return false;
        }

        else return false;
    }

    void OnMove(InputValue movementValue)
    {
        _movementInput = movementValue.Get<Vector2>();
    }

    void OnFire()
    {
        _animator.SetTrigger("swordAttack");
    }

    public void SwordAttack()
    {
        LockMovement();
        if (_spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }
    }
    public void LockMovement()
    {
        canMove = false;
    }
    public void UnlockMovement()
    {
        canMove = true;
    }
    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }
}
