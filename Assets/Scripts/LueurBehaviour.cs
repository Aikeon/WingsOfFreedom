using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LueurBehaviour : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float baseSpeed;
    [SerializeField] float speedCap;
    [SerializeField] float distMult;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var distSpeed = distMult / Vector3.Distance(transform.position, player.position);
        var trueSpeed = Mathf.Min(baseSpeed * Time.deltaTime * distSpeed, speedCap);
        transform.position += Vector3.forward * trueSpeed;
    }
}
