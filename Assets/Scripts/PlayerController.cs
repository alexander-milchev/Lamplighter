using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static GameInput;

public class PlayerController : MonoBehaviour
{
    [Header("Player Balancing")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashDuration;
    [Header("Player Damage and Death")]
    [SerializeField]private Vector2 knockbackForce = new Vector2(10f, 20f);
    [SerializeField]private float knockbackDuration = 0.5f;

    [Header("Components")]
    [SerializeField] private Animator playerAnimator;

    private Rigidbody2D playerRB;
    private BoxCollider2D playerFeetCollider; // yummy

    private int maxJumps = 2;
    private bool isKnockedBack;
    private int jumpCount;
    private int groundLayer;

    private bool canDash = true;


    private void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        playerFeetCollider = gameObject.GetComponent<BoxCollider2D>();
        groundLayer = LayerMask.GetMask("Ground");

        GameInput.instance.OnJump += Jump;
        GameInput.instance.OnDash += Dash;
        PlayerHealth.instance.OnTakeDamage += KnockBackPlayer;
        PlayerHealth.instance.OnDeath += DeathAnimation;
        PlayerHealth.instance.OnRespawn += RespawnTransform;
    }

    void OnDestroy()
    {
        GameInput.instance.OnJump -= Jump;
        GameInput.instance.OnDash -= Dash;
        PlayerHealth.instance.OnTakeDamage -= KnockBackPlayer;
        PlayerHealth.instance.OnDeath -= DeathAnimation;
        PlayerHealth.instance.OnRespawn -= RespawnTransform;
    }

    private void FixedUpdate()
    {
        Move();
        FlipSprite();
    }

    private void Jump(object sender, EventArgs e)
    {
        if (PlayerHealth.instance.isDead){return;}
        // Reset jump count
        if (playerFeetCollider.IsTouchingLayers(groundLayer))
        {
            jumpCount = 0;
        }

        if (jumpCount < maxJumps)
        {
            playerRB.linearVelocity = new Vector2 (0f, jumpSpeed);
            jumpCount++;
            playerAnimator.SetBool("isJumping", true);

            if (jumpCount == 1){StartCoroutine(WaitToLand());}
        }
    }

    private void Dash(object sender, EventArgs e)
    {
        if(!canDash){return;}
        if (PlayerHealth.instance.isDead){return;}

        StartCoroutine(DashRoutine());
    }

    private void Move()
    {
        if (PlayerHealth.instance.isDead){return;}
        if(isKnockedBack){return;}
        Vector2 moveVector = GameInput.instance.GetMoveVector();
        Vector2 playerVelocity = new Vector2 (moveVector.x * moveSpeed, playerRB.linearVelocityY);

        playerRB.linearVelocity = playerVelocity;

        // Sets animations for jumping, falling and running
        playerAnimator.SetFloat("xVelocity", Math.Abs(playerVelocity.x));
    }

    private void FlipSprite()
    {
        if (isKnockedBack){return;}
        bool hasHorizontalSpeed = Mathf.Abs(playerRB.linearVelocityX) > Mathf.Epsilon;

        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(playerRB.linearVelocityX), 1f);
        }
    }

    private void KnockBackPlayer(object sender, EventArgs e)
    {
        StartCoroutine(PlayerKnockbackDuration());

        Vector2 collisionPos = PlayerHealth.instance.GetCollisionPos();
        Vector2 direction = (Vector2)transform.position - collisionPos;

        direction = direction.normalized;
        playerRB.linearVelocity = new Vector2(
            direction.x * knockbackForce.x, direction.y * knockbackForce.y
        );
    }

    private void DeathAnimation(object sender, EventArgs e)
    {
        playerAnimator.SetTrigger("isDead");
        playerRB.constraints = RigidbodyConstraints2D.None;
    }

    private IEnumerator WaitToLand()
    {
        yield return new WaitUntil(() => !playerFeetCollider.IsTouchingLayers(groundLayer));
        yield return new WaitUntil(() => playerFeetCollider.IsTouchingLayers(groundLayer));

        playerAnimator.SetBool("isJumping", false);
    }

    private IEnumerator DashRoutine()
    {
        Debug.Log("Dashing");
        canDash = false;

        moveSpeed += dashForce;
        playerAnimator.SetBool("isDashing", true);
        yield return new WaitForSeconds(dashDuration);

        moveSpeed -= dashForce;
        playerAnimator.SetBool("isDashing", false);
        
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator PlayerKnockbackDuration()
    {
        isKnockedBack = true;
        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false;
        yield return new WaitForSeconds(knockbackDuration);
        playerRB.linearVelocityX = 0f;
    }

    private void RespawnTransform(object sender, EventArgs e)
    {
        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerRB.linearVelocity = Vector2.zero;
        gameObject.transform.rotation = Quaternion.identity;
        playerAnimator.SetTrigger("isAlive");
        
        Vector2 respawnPos = GameManager.instance.GetSpawnPos();
        gameObject.transform.position = respawnPos;
    }
}
