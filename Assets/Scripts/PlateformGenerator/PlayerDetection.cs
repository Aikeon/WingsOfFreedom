using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private bool isInit = false;
    public PlateformGenerator.PlateformGenerator plateformGenerator;

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInit == false)
        {
            isInit = true;
            plateformGenerator.CreatePillar();
        }
    }
}
