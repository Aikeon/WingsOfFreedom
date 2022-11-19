using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    private Vector3 _initialPosition;

    public void Awake()
    {
        _initialPosition = transform.position;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = _initialPosition;
        }
    }
}
