using System;
using Unity.Cinemachine;
using UnityEngine;

public class ScreenEffects : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private float impulseForce = 0.25f;

    private void Start()
    {
        PlayerHealth.instance.OnTakeDamage += PlayScreenShake;
    }

    void OnDestroy()
    {
        PlayerHealth.instance.OnTakeDamage -= PlayScreenShake;
    }

    private void PlayScreenShake(object sender, EventArgs e)
    {
        impulseSource.GenerateImpulse(impulseForce);
    }
}
