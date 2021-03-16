using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.EventSystems;

public class PowerNodeDisplay : MonoBehaviour
{
    
    protected Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        UpdatePowerNodeDisplay(0);
    }

    public virtual void UpdatePowerNodeDisplay(int nodesCollected)
    {
        text.text = "Power Nodes: " + nodesCollected + "/10";
    }

}
