using Unity.Cinemachine;
using UnityEngine;

public class PlayerScreenShake : MonoBehaviour
{
    private bool screenShake;
    private CinemachineImpulseSource impulseSource;
    private void Start()
    {
        screenShake = StoreCameraShake.instance.GetScreenShakeState();
        impulseSource.enabled = screenShake;
    }
}
