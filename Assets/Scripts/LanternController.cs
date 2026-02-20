using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LanternController : MonoBehaviour
{
    public static LanternController instance;
    public event EventHandler OnFuelEmpty;
    
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
    [SerializeField] private Light2D spotLight;
    [SerializeField] public GameObject lightSprite;
    private GameObject target;                  // At end of level, update target to the pedestal and let it go on top.
    private float targetRot = 0f;
    private float rotVelocity = 0f;

    [Header("Sprites")]
    [SerializeField] private GameObject spriteFull;
    [SerializeField] private GameObject spriteLow;
    [SerializeField] private GameObject spriteDead;

    [Header("Light meter")]
    [SerializeField] private float maxLightMeter = 10f;
    [SerializeField] private float fullThreshold = 5f;
    [SerializeField] private float maxFuelUse= 1f;
    [SerializeField] private float lightRegenRate = 1f;
    [SerializeField] private float flickerTime = 0.2f;
    private float currentLightMeter;
    private Coroutine lightDecreaseRoutine;
    private bool noFuel;
    
    private float distance;
    private bool chaseFlag = true;
    private float lightRadius;
    private bool increasingIntensity = false;
    private bool decreasingIntensity = false;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLightMeter = maxLightMeter;
        player = GameObject.Find("_Player");
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
        PlayerHealth.instance.OnDeath += LightDead;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log("Current fuel: " + currentLightMeter);
        UpdateDistance();
        if (chaseFlag && distance > maxTargetDistance)
        {
            ChaseTarget();
            UpdateTargetRot();
        }
        UpdateTilt();       // Even if we aren't moving, let the Tilt resume back to 0.
        LightIntensity();
        UpdateLights();
        RegenerateLight();
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
        if (PlayerHealth.instance.isDead){return;}
        if (increasingIntensity && !decreasingIntensity)
        {
            lightRadius += changeRate;
        }
        else if(!increasingIntensity && decreasingIntensity)
        {
            lightRadius -= changeRate;
        }
        lightRadius = Mathf.Clamp(lightRadius, minLightRange, maxLightRange);
        if (!noFuel && lightRadius > minLightRange){UseFuel();}
    }

    private void UpdateLights()
    {
        float innerRadius = lightRadius/2;
        
        spotLight.pointLightInnerRadius = innerRadius;
        spotLight.pointLightOuterRadius = lightRadius;

        lightCollider.radius = lightRadius;
        lightSprite.transform.localScale = new Vector3(lightRadius * 2, lightRadius * 2, 1);
    }

    private void Lantern(object sender, EventArgs e)
    {
        if (PlayerHealth.instance.isDead){return;}
        chaseFlag = !chaseFlag;
        targetRot = 0f;
    }

    private void IntensityUp(object sender, EventArgs e)
    {
        if(noFuel){return;}
        increasingIntensity = true;
    }

    private void IntensityUpCancel(object sender, EventArgs e)
    {
        if(noFuel){return;}
        increasingIntensity = false;
    }

    private void IntensityDown(object sender, EventArgs e)
    {
        if(noFuel){return;}
        decreasingIntensity = true;
    }

    private void IntensityDownCancel(object sender, EventArgs e)
    {
        if(noFuel){return;}
        decreasingIntensity = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ButtonController>(out ButtonController buttonController))
        {
            buttonController.DoorToggle();
        }
        else if(collision.TryGetComponent<HazardsController>(out HazardsController hazardsController))
        {
            hazardsController.EnterLight();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ButtonController>(out ButtonController buttonController))
        {
            buttonController.DoorToggle();
        } 
        else if(collision.TryGetComponent<HazardsController>(out HazardsController hazardsController))
        {
            hazardsController.ExitLight();
        }
    }


    // Sprite meter + death stuff
    private void LightDead(object sender, EventArgs e)
    {
        lightRadius = 0;
        SetSprite(spriteDead);
    }

    public void SetSprite(GameObject sprite)
    {
        spriteFull.SetActive(false);
        spriteLow.SetActive(false);
        spriteDead.SetActive(false);

        sprite.SetActive(true);
    }

    private void UseFuel()
    {
        if (lightDecreaseRoutine != null)
        {
            StopCoroutine(lightDecreaseRoutine);
            lightDecreaseRoutine = null;
        }
        if (currentLightMeter < fullThreshold){SetSprite(spriteLow); spotLight.intensity = 2.5f;}
        float lightPercentageLimit = maxLightRange - minLightRange;
        float currentLightPercentage = lightRadius - minLightRange;

        float fuelUseRate = currentLightPercentage/lightPercentageLimit;
        float fuelUsePerSecond = fuelUseRate * maxFuelUse;
        
        lightDecreaseRoutine = StartCoroutine(DecreaseLightMeter(fuelUsePerSecond));
        StartCoroutine(DecreaseLightToMinimum());
    }
    
    private IEnumerator DecreaseLightMeter(float decreaseRate)
    {
        currentLightMeter = Mathf.Clamp(currentLightMeter, 0f, maxLightMeter);
         while (currentLightMeter > 0)
         {
              currentLightMeter -= decreaseRate;
              yield return new WaitForSeconds(1);
          }
        noFuel = true;
        OnFuelEmpty?.Invoke(this, EventArgs.Empty);
    }
    
    private IEnumerator DecreaseLightToMinimum()
    {
        if (currentLightMeter == 0 && lightRadius > minLightRange)
        {
            increasingIntensity = false;
            decreasingIntensity = true;
        }
        
        yield return new WaitUntil(() => lightRadius == minLightRange);
        StartCoroutine(FlickerLights());
        yield return new WaitWhile(() => currentLightMeter != maxLightMeter);
        noFuel = false;
        decreasingIntensity = false;
    }

    private void RegenerateLight()
    {
        currentLightMeter = Mathf.Clamp(currentLightMeter, 0f, maxLightMeter);
        if (lightRadius == minLightRange && currentLightMeter < maxLightMeter)
        {
            currentLightMeter += lightRegenRate * Time.deltaTime;
        }
        if (currentLightMeter >= fullThreshold)
        {
            SetSprite(spriteFull);
            spotLight.intensity = 3.15f;
        }
    }

    private IEnumerator FlickerLights()
    {
        while (noFuel)
        {
        spotLight.gameObject.SetActive(false);
        yield return new WaitForSeconds(flickerTime);
        spotLight.gameObject.SetActive(true);
        yield return new WaitForSeconds(flickerTime*10);
        }
    }
}
