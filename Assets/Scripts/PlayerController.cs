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
    private BoxCollider2D playerFeetCollider; // yummy

    private void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        playerFeetCollider = gameObject.GetComponent<BoxCollider2D>();
        GameInput.instance.OnJump += Jump;
    }

    private void Update()
    {
        Move();
        FlipSprite();
    }

    private void Jump(object sender, EventArgs e)
    {
        int groundLayer = LayerMask.GetMask("Ground");

        if (playerFeetCollider.IsTouchingLayers(groundLayer))
        {
            playerRB.linearVelocity += new Vector2 (0f, jumpSpeed);
        }
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
