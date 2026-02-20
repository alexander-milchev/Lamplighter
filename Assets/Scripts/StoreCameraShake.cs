using Unity.Cinemachine;
using UnityEngine;

public class StoreCameraShake : MonoBehaviour
{
    public static StoreCameraShake instance;

    private CinemachineImpulseSource camerashakeSource;
    private bool cameraShakeState = true;

    private void Awake()
    {
        SingletonPattern();
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

    private void FixedUpdate()
    {
        //ChangePlayerScreenShake();
    }

    public void ChangePlayerScreenShake()
    {
        camerashakeSource = FindFirstObjectByType<CinemachineImpulseSource>();
        camerashakeSource.enabled = cameraShakeState;
        Debug.Log(cameraShakeState);
    }

    public void ChangeScreenShakeState(bool value)
    {
        cameraShakeState = value;
        ChangePlayerScreenShake();
    }

    public bool GetScreenShakeState()
    {
        return cameraShakeState;
    }
}
