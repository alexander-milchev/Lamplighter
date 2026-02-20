using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject spriteOn;
    [SerializeField] private GameObject spriteOff;

    private void Awake()
    {
        spriteOff.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.instance.SetCheckpoint(gameObject);
        TurnOn();
    }

    private void TurnOn()
    {
        SetSprite(spriteOn);
    }

    public void TurnOff()
    {
        SetSprite(spriteOff);
    }

    private void SetSprite(GameObject sprite)
    {
        spriteOff.gameObject.SetActive(false);
        spriteOn.gameObject.SetActive(false);

        sprite.gameObject.SetActive(true);
    }
}
