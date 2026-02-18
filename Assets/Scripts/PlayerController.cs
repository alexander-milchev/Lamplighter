using System;
using Unity.VisualScripting;
using UnityEngine;
using static GameInput;

public class PlayerController : MonoBehaviour
{
    [Header("Player Balancing")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;

    [Header("Components")]
    [SerializeField] private Animator playerAnimator;

    private Rigidbody2D playerRB;

    private void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        FlipSprite();
    }

    private void Jump()
    {
        
    }

    private void Move()
    {
        Vector2 moveVector = GameInput.instance.GetMoveVector();

        Vector2 playerVelocity = new Vector2 (moveVector.x * moveSpeed, playerRB.linearVelocityY);
        bool hasHorizontalSpeed = Mathf.Abs(playerRB.linearVelocityX) > Mathf.Epsilon;

        playerRB.linearVelocity = playerVelocity;
        if (hasHorizontalSpeed)
        {
            playerAnimator.SetBool("Walking", true);
        }
        else
        {
            playerAnimator.SetBool("Walking", false);
        }
    }

    private void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(playerRB.linearVelocityX) > Mathf.Epsilon;

        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(playerRB.linearVelocityX), 1f);
        }
    }

}
