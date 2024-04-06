﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UIElementState
{
    DISABLED,
    ALIVE,
    ATTACKED,
    DEAD
}

[System.Serializable]
public struct StateColors
{
    public Color alive;
    public Color attacked;
    public Color dead;
}

[System.Serializable]
public class UIElement
{
    private Color inactive = new Color(0F, 0F, 0F, 0F);
    private UIElementState state;
    public TextMeshProUGUI textOutput;
    public Image underlay;
    public StateColors stateColorsText;
    public StateColors stateColorsUnderlay;

    public string SetText
    {
        set => textOutput.text = value; 
    }

    public void Setup() => SetState(UIElementState.DISABLED);

    public void SetState(UIElementState state)
    {
        if (this.state == state)
            return;

        switch (state)
        {
            case UIElementState.DISABLED:
                textOutput.color = inactive;
                underlay.color = inactive;
                textOutput.text = "";
                break;
            case UIElementState.ALIVE:
                underlay.color = stateColorsUnderlay.alive;
                textOutput.color = stateColorsText.alive;
                break;
            case UIElementState.ATTACKED:
                underlay.color = stateColorsUnderlay.attacked;
                textOutput.color = stateColorsText.attacked;
                break;
            case UIElementState.DEAD:
                underlay.color = stateColorsUnderlay.dead;
                textOutput.color = stateColorsText.dead;
                break;
            default:
                textOutput.color = inactive;
                underlay.color = inactive;
                textOutput.text = "";
                break;
        }
        this.state = state;
    }
}

namespace UI
{
    public class UI_TargetDisplay : MonoBehaviour
    {
        string targetName; // The name of the target. 
        int targetIndex; // The index of the target (Imagine this as the target number in a list). 
        int currentHealth; // The current health of the player. 
        int healthMax; //The maximum health of the player.

        public GameObject rootObject;
        public UIElement nameOutput;
        public UIElement targetNumberOutput;
        public UIElement currentHealthOutput;
        public UIElement maxHealthOutput;

        private void OnEnable()
        {
            nameOutput.Setup();
            targetNumberOutput.Setup();
            currentHealthOutput.Setup();
            maxHealthOutput.Setup();
        }

        #region  Base System stuff. 
        internal void InitData(TargetData data)
        {
            targetName = name;
            targetIndex = data.Index;
            currentHealth = data.HealthCurrent;
            healthMax = data.HealthMax;
            OnIdle();
        }

        internal void UpdateData(TargetData data, bool idle, bool attacked, bool dead)
        {
            currentHealth = data.HealthCurrent;
            healthMax = data.HealthMax;
            UpdateUI();
            if (idle) OnIdle();
            if (attacked) OnAttack();
            if (dead) OnDeath();
        }
        #endregion

        internal void OnActivation()
        {
            //Do UI stuff here. 
            rootObject.SetActive(true);
            nameOutput.SetState(UIElementState.ALIVE);
            targetNumberOutput.SetState(UIElementState.ALIVE);
            currentHealthOutput.SetState(UIElementState.ALIVE);
            maxHealthOutput.SetState(UIElementState.ALIVE);
        }

        internal void UpdateUI()
        {
            //Do general UI updates here. 
            nameOutput.SetText = targetName;
            targetNumberOutput.SetText = targetIndex.ToString();
            currentHealthOutput.SetText = currentHealth.ToString();
            maxHealthOutput.SetText = healthMax.ToString();
        }

        public void OnIdle()
        {
            //Do any updates related to Idle UI stuff here.
            rootObject.SetActive(true);
            nameOutput.SetState(UIElementState.ALIVE);
            targetNumberOutput.SetState(UIElementState.ALIVE);
            currentHealthOutput.SetState(UIElementState.ALIVE);
            maxHealthOutput.SetState(UIElementState.ALIVE);
        }

        public void OnAttack()
        {
            //Do any updates related to Attack UI stuff here.
            rootObject.SetActive(true);
            nameOutput.SetState(UIElementState.ATTACKED);
            targetNumberOutput.SetState(UIElementState.ATTACKED);
            currentHealthOutput.SetState(UIElementState.ATTACKED);
            maxHealthOutput.SetState(UIElementState.ATTACKED);
        }

        public void OnDeath()
        {
            //Do any updates related to Death UI stuff here.
            nameOutput.SetState(UIElementState.DEAD);
            targetNumberOutput.SetState(UIElementState.DEAD);
            currentHealthOutput.SetState(UIElementState.DEAD);
            maxHealthOutput.SetState(UIElementState.DEAD);

            StartCoroutine(DisableTimer());
        }

        private IEnumerator DisableTimer()
        {
            yield return new WaitForSeconds(1.5F);
            nameOutput.SetState(UIElementState.DISABLED);
            targetNumberOutput.SetState(UIElementState.DISABLED);
            currentHealthOutput.SetState(UIElementState.DISABLED);
            maxHealthOutput.SetState(UIElementState.DISABLED);
        }
    }
}