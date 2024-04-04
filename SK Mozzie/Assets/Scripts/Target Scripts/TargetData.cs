﻿using System.Collections;
using UnityEngine;

[System.Serializable]
public struct TargetDataStruct
{
    [SerializeField] public string name;
    [SerializeField] public int index;
    [SerializeField] public int healthCurrent;
    [SerializeField] public int healthMax;
    [SerializeField] public int rateOfExtraction;
}

[System.Serializable]
[RequireComponent(typeof(TargetHealth))]
public class TargetData : MonoBehaviour
{
    public TargetHealth targetHealth;
    public TargetDataStruct tData;

    public string TargetName => tData.name;
    public int Index => tData.index;
    public int HealthMax => tData.healthMax;
    public int HealthCurrent
    {
        get => tData.healthCurrent; 
        set
        {
            tData.healthCurrent = Mathf.Clamp(tData.healthCurrent + value, 0, HealthMax);
        }
    } 
    public int RateOfExtraction => tData.rateOfExtraction;
    public bool playIdleAnim = true; 
    public bool playAttackAnim = true; 
    public bool playDeathAnim = true;
    public bool deathAnimComplete = false; 

    public void OnEnable()
    {

    }

    public void Update()
    {
        if(targetHealth.state == TargetHealthState.ATTACKED)
        {
            targetHealth.UpdateData(ref tData);
        }
        ResolveTargetState(); 
    }

    private void ResolveTargetState()
    {
        if (playDeathAnim)
            return;

        switch (targetHealth.state)
        {
            case TargetHealthState.ALIVE:
                Animation_Idle(); 
                break;
            case TargetHealthState.ATTACKED:
                Animation_Attacked();
                break;
            case TargetHealthState.DEAD:
                Animation_Dead(); 
                break;
        }
    }

    public void Animation_Idle()
    {
        playIdleAnim = true;
        playDeathAnim = playAttackAnim = false;
    }

    public void Animation_Attacked()
    {
        playAttackAnim = true;
        playDeathAnim = playIdleAnim = false;
    }

    public void Animation_Dead()
    {
        playDeathAnim = true;
        playAttackAnim = playIdleAnim = false;
    }

    public void OnDeathAnimCompleted()
    {
        //Do some logic updating here. 
        //Set gameobject inactive. 
    }
}