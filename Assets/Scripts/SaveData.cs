using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    /// <summary>
    /// In this file we make all the variables that will be used whenever saving or loading.
    /// </summary>
    public List<float> robotLocationx = new List<float>();
    public List<float> robotLocationy = new List<float>();
    public List<float> robotLocationz = new List<float>();
    public List<int> charge = new List<int>();

    public List<float> treeLocationx = new List<float>();
    public List<float> treeLocationy = new List<float>();
    public List<float> treeLocationz = new List<float>();
    public List<bool> isTree = new List<bool>();
    public List<float> hunger = new List<float>();
    public List<float> thirst = new List<float>();
}
