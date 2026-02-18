using System;
using UnityEngine;

public class LanternController : MonoBehaviour
{
    [Header("Player Balancing")]
    [Range(0f, 10f)]
    [SerializeField] private float minChaseSpeed = 3f;
    [Range(0f, 2f)]
    [SerializeField] private float maxTargetDistance = 0.1f;
    [Range(0f, 90f)]
    [SerializeField] private float maxPitch = 45f;
    [Range(0f, 0.5f)]
    [SerializeField] private float smoothTime = 0.1f;
    [Range(0f, 5f)]
    [SerializeField] private float minLightRange = 2f;
    [Range(5f, 20f)]
    [SerializeField] private float maxLightRange = 10f;

    [Header("Components")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject lanternHolder;
    private GameObject target;                  // At end of level, update target to the pedestal and let it go on top.
    private float targetRot = 0f;
    private float rotVelocity = 0f;
    
    private float distance;
    private bool chaseFlag = true;
    private float lightRadius;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        target = player.transform.GetChild(1).gameObject;
        lanternHolder = gameObject;

        lightRadius = minLightRange;

        GameInput.instance.OnLantern += Lantern;
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
    }

    private void UpdateDistance()
    {
        distance = Vector2.Distance(lanternHolder.transform.position, target.transform.position);
    }

    private void ChaseTarget()
    {
        lanternHolder.transform.position = Vector2.Lerp(lanternHolder.transform.position, target.transform.position, minChaseSpeed*Time.deltaTime);
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
        targetRot = Mathf.Clamp(GetHorizDist() / minChaseSpeed * maxPitch, -maxPitch, maxPitch);
    }

    private void UpdateTilt()
    {
        float newAngle = Mathf.SmoothDampAngle(lanternHolder.transform.eulerAngles.z, targetRot, ref rotVelocity, smoothTime);
        lanternHolder.transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }

    private void Lantern(object sender, EventArgs e)
    {
        chaseFlag = !chaseFlag;
        targetRot = 0f;
    }

    private void IntensityUp(object sender, EventArgs e)
    {
        
    }

    private void IntensityDown(object sender, EventArgs e)
    {
        
    }
}
