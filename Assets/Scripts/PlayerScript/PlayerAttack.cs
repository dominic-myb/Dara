using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public PlayerController _player;
    private Button attackButton;
    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        _player = playerObject.GetComponent<PlayerController>();
        attackButton = GetComponent<Button>();
        attackButton.onClick.AddListener(OnAttackButtonClick);
    }

    private void OnAttackButtonClick()
    {
        _player._animator.SetTrigger("swordAttack");
        _player.SwordAttack();
    }
}
