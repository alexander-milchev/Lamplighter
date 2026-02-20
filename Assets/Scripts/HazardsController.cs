using System;
using System.Collections;
using UnityEngine;

public class HazardsController : MonoBehaviour
{
    [Header("Hazard shoot up values")]
    [Range(0f, 30f)]
    [SerializeField] private float range = 5f;              // Max Range Scale = this + 1 (range 5 -> Scale 6)
    [Range(0.001f, 10f)]
    [SerializeField] private float maxTimeInLightBeforeAttack = 3f;
    [Range(0.001f, 10f)]
    [SerializeField] private float attackSpeed = 1f;        // Seconds it will take to reach max Range
    [Range(0f, 1f)]
    [SerializeField]private float holdDuration = 0.25f;
    [Range(0.001f, 10f)]
    [SerializeField] private float retreatSpeed = 3f;
    private float windup = 0f;
    private bool inLight = false;
    private bool attacking = false;
    private bool growing = false;
    private bool retreating = false;
    private float changePerTick;

    public void FixedUpdate()       // Called 50 times per sec
    {
        if (inLight)
        {
            windup += 0.02f;
        }

        if (windup >= maxTimeInLightBeforeAttack && !attacking)
        {
            StartCoroutine(AttackRoutine());
        }

        if (growing || retreating)
        {
            Vector3 pos = gameObject.transform.position;
            Vector3 temp = gameObject.transform.localScale;
            gameObject.transform.position = new Vector3(pos.x, pos.y + (changePerTick/2), pos.z);
            gameObject.transform.localScale = new Vector3(temp.x, temp.y + changePerTick, temp.z);
        }
    }

    public void EnterLight()
    {
        Debug.Log("Enter");
        inLight = true;
        windup = 0f;
    }

    public void ExitLight()
    {
        Debug.Log("Exit");
        inLight = false;
    }

    private IEnumerator AttackRoutine()
    {
        // Begin attacking
        attacking = true;
        growing = true;
        changePerTick = range / (attackSpeed * 50);
        Debug.Log("Growing");
        yield return new WaitForSeconds(attackSpeed);
        // Hold it
        growing = false;
        Debug.Log("Holding");
        yield return new WaitForSeconds(holdDuration);
        // Retreat back to original
        retreating = true;
        changePerTick = -range / (retreatSpeed * 50);
        Debug.Log("Retreating");
        yield return new WaitForSeconds(retreatSpeed);
        retreating = false;
        windup = 0;
        attacking = false;
    }
}
