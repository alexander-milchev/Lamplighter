using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    public event EventHandler OnRespawn;
    public event EventHandler OnTakeDamage;
    public event EventHandler OnDeath;

    [SerializeField]private int playerHealth = 3;
    [SerializeField]private float invulnDuration = 1f;
    [HideInInspector]public bool isDead;

    private int hazardsLayer;
    private bool canTakeDamage = true;
    private Vector2 collisionPos;

    private void Awake()
    {
        SingletonPattern();
    }

    private void Start()
    {
        hazardsLayer = LayerMask.GetMask("Hazards");
    }

     private void SingletonPattern()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((hazardsLayer & (1 << collision.gameObject.layer)) != 0)
        {
            OnTakeDamage?.Invoke(this, EventArgs.Empty);
            TakeDamage();
            collisionPos = collision.transform.position;
            Debug.Log(playerHealth);
        }
    }

    private void TakeDamage()
    {
        if(!canTakeDamage){return;}
        StartCoroutine(Invulnerable());
        if (playerHealth > 0)
        {
            playerHealth --;
        }
        if (playerHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
        isDead = true;
    }

    public void Respawn()
    {
        OnRespawn?.Invoke(this, EventArgs.Empty);
        isDead = false;
    }

    private IEnumerator Invulnerable()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(invulnDuration);
        canTakeDamage = true;
    }

    public Vector2 GetCollisionPos()
    {
        return collisionPos;
    }
}
