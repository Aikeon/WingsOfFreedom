using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetPosition : MonoBehaviour
{
    [SerializeField] private GameObject target;

    private Vector3 _vectorToTarget;

    private void Awake()
    {
        _vectorToTarget = target.transform.position - transform.position;
    }
    
    void Update()
    {
        transform.position = target.transform.position - _vectorToTarget;
    }
}
