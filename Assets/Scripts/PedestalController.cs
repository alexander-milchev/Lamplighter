using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class PedestalController : MonoBehaviour
{
    [SerializeField] private GameObject pedestalTarget;
    [SerializeField] private GameObject globalLight;
    private bool isLanternInRange = false;
    private bool endingLevel = false;

    public void Start()
    {
        GameInput.instance.OnEndLevel += EndLevel;
    }

    public void OnDestroy()
    {
        GameInput.instance.OnEndLevel -= EndLevel;
    }

    public void FixedUpdate()
    {
        if (endingLevel)
        {
            if (globalLight.TryGetComponent<Light2D>(out Light2D light2d))
            {
                light2d.intensity += 0.002f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<LanternController>(out LanternController lanternController))
        {
            Debug.Log("In");
            isLanternInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<LanternController>(out LanternController lanternController))
        {
            Debug.Log("Out");
            isLanternInRange = false;
        }
    }

    private void EndLevel(object sender, EventArgs e)
    {
        if (!isLanternInRange){return;}
        if(endingLevel){return;}
        LanternController.instance.EndLevel(pedestalTarget);
        StartCoroutine(EndLevelCD());
    }

    private IEnumerator EndLevelCD()
    {
        endingLevel = true;
        yield return new WaitForSeconds(10f);
        endingLevel = false;
        GameManager.instance.DebugCollectibles();
    }
}
