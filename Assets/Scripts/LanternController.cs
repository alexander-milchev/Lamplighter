using System;
using UnityEngine;

public class LanternController : MonoBehaviour
{
    [Header("Following Fields")]
    [Range(0f, 10f)]
    [SerializeField] private float chaseSpeed = 3f;
    [Range(0f, 2f)]
    [SerializeField] private float maxTargetDistance = 0.1f;
    [Header("Swaying Fields")]
    [Range(0f, 90f)]
    [SerializeField] private float maxPitch = 45f;
    [Range(0f, 0.5f)]
    [SerializeField] private float smoothTime = 0.1f;
    [Range(0f, 5f)]
    [Header("Light Mechanic Fields")]
    [SerializeField] private float minLightRange = 2f;
    [Range(5f, 20f)]
    [SerializeField] private float maxLightRange = 10f;
    [Range(0.0001f, 1f)]
    [SerializeField] private float changeRate = 0.1f;

    [Header("Components")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject lanternHolder;
    [SerializeField] private CircleCollider2D lightCollider;
    [SerializeField] public GameObject lightSprite;
    private GameObject target;                  // At end of level, update target to the pedestal and let it go on top.
    private float targetRot = 0f;
    private float rotVelocity = 0f;
    
    private float distance;
    private bool chaseFlag = true;
    private float lightRadius;
    private bool increasingIntensity = false;
    private bool decreasingIntensity = false;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        target = player.transform.GetChild(1).gameObject;
        lanternHolder = gameObject;
        lightCollider = gameObject.GetComponent<CircleCollider2D>();
        //lightSprite = GameObject.Find("LightSprite");

        lightRadius = minLightRange;

        GameInput.instance.OnLantern += Lantern;
        GameInput.instance.OnIntensityUp += IntensityUp;
        GameInput.instance.CancelIntensityUp += IntensityUpCancel;
        GameInput.instance.OnIntensityDown += IntensityDown;
        GameInput.instance.CancelIntensityDown += IntensityDownCancel;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateDistance();
        if (chaseFlag && distance > maxTargetDistance)
        {
            ChaseTarget();
            UpdateTargetRot();
        }
        UpdateTilt();       // Even if we aren't moving, let the Tilt resume back to 0.
        LightIntensity();
        UpdateLights();
    }

    private void UpdateDistance()
    {
        distance = Vector2.Distance(lanternHolder.transform.position, target.transform.position);
    }

    private void ChaseTarget()
    {
        lanternHolder.transform.position = Vector2.Lerp(lanternHolder.transform.position, target.transform.position, chaseSpeed*Time.deltaTime);
    }

    private float GetHorizDist()
    {
        float totalDist = lanternHolder.transform.position.x - target.transform.position.x;
        if (totalDist > 0)
        {
            totalDist -= maxTargetDistance;
        }
        else
        {
            totalDist += maxTargetDistance;
        }
        return totalDist;
    }

    private void UpdateTargetRot()
    {
        targetRot = Mathf.Clamp(GetHorizDist() / chaseSpeed * maxPitch, -maxPitch, maxPitch);
    }

    private void UpdateTilt()
    {
        float newAngle = Mathf.SmoothDampAngle(lanternHolder.transform.eulerAngles.z, targetRot, ref rotVelocity, smoothTime);
        lanternHolder.transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }

    private void LightIntensity()
    {
        if (increasingIntensity && !decreasingIntensity)
        {
            Debug.Log("Increasing Intensity");
            lightRadius += changeRate;
        }
        else if(!increasingIntensity && decreasingIntensity)
        {
            Debug.Log("Decreasing Intensity");
            lightRadius -= changeRate;
        }
        lightRadius = Mathf.Clamp(lightRadius, minLightRange, maxLightRange);
    }

    private void UpdateLights()
    {
        lightCollider.radius = lightRadius;
        lightSprite.transform.localScale = new Vector3(lightRadius * 2, lightRadius * 2, 1);
    }

    private void Lantern(object sender, EventArgs e)
    {
        chaseFlag = !chaseFlag;
        targetRot = 0f;
    }

    private void IntensityUp(object sender, EventArgs e)
    {
        increasingIntensity = true;
    }

    private void IntensityUpCancel(object sender, EventArgs e)
    {
        increasingIntensity = false;
    }

    private void IntensityDown(object sender, EventArgs e)
    {
        decreasingIntensity = true;
    }

    private void IntensityDownCancel(object sender, EventArgs e)
    {
        decreasingIntensity = false;
    }
}
