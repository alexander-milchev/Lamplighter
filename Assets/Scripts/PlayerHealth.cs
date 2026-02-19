using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    public event EventHandler OnTakeDamage;
    public event EventHandler OnDeath;

    [SerializeField]private int playerHealth = 3;
    [HideInInspector]public bool isDead;

    private PlayerController playerController;
    private int hazardsLayer;

    private void Start()
    {
        SingletonPattern();
        playerController = gameObject.GetComponent<PlayerController>();
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
            Debug.Log(playerHealth);
        }
    }

    private void TakeDamage()
    {
        Debug.Log("Taking Damage");
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
        // implement respawn stuff here
    }
}
