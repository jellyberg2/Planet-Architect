﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// GameVariables are parameters that trigger events
/// </summary>
public class GameVariable
{
    public string name;
    public string description;
    public int defaultValue;
    public int value;
    public VariablesManager varManager;

    public void ChangeValue(int newValue, bool shouldUpdateSlider = true)
    {
        value = Mathf.Clamp(newValue, 0, 100); // all variables lie between 0 and 100

        if (shouldUpdateSlider) // the variable has been changed by something other than the slider
        {
            varManager.sliders[name].UpdateSlider();
        }
    }
}


public class VariablesManager : MonoBehaviour
{
    public Dictionary<string, GameVariable> gameVars;
    public GameObject sliderLayout;
    public GameObject sliderPrefab;
    public Dictionary<string, VariableSlider> sliders;

    /// <summary>
    /// Add a new player controlled variable and create a slider for it
    /// </summary>
    /// <param name="jsonString">
    /// Should have properties: "name", "description", "default"
    /// </param>
    public void AddVariable(string jsonString, int variableNum, int numVariables)
    {
        // if this is the first variable, initialise the arrays
        if (gameVars == null)
        {
            gameVars = new Dictionary<string, GameVariable>(numVariables);
            sliders = new Dictionary<string, VariableSlider>(numVariables);
        }

        GameVariable newVar = JsonUtility.FromJson<GameVariable>(jsonString);
        newVar.varManager = this; // so the variable can modify sliders etc
        gameVars[newVar.name] = newVar;

        AddSlider(newVar);
    }

    /// <summary>
    /// Create a new slider as child of sliderLayout, which should stack them vertically
    /// </summary>
    void AddSlider(GameVariable gameVar)
    {
        GameObject newSlider = Instantiate(sliderPrefab);
        newSlider.transform.SetParent(sliderLayout.transform);
        VariableSlider sliderManager = newSlider.GetComponent<VariableSlider>();
        sliderManager.Setup(gameVar);
        sliders[gameVar.name] = sliderManager;
    }
}
