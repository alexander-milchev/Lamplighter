using System;
using System.Collections;
using UnityEngine;

public class HazardsController : MonoBehaviour
{
    [Header("Hazard shoot up values")]
    [Range(0f, 30f)]
    [SerializeField] private float range = 5f;
    [Range(0.001f, 10f)]
    [SerializeField] private float maxTimeInLightBeforeAttack = 3f;
    [Range(0.001f, 10f)]
    [SerializeField] private float attackSpeed = 3f;
    [Range(0.001f, 10f)]
    [SerializeField] private float retreatSpeed = 1f;
    private float windup = 0f;
    private bool inLight = false;

    public void FixedUpdate()       // Called 50 times per sec
    {
        windup += 0.02f;
    }

    public void EnterLight()
    {
        inLight = true;
        windup = 0f;
    }

    public void ExitLight()
    {
        inLight = false;
    }

    private IEnumerator AttackRoutine()
    {


        return null;
    }
}
