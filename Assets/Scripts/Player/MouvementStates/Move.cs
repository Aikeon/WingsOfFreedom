using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Move : State<Mouvement>
{
    private float turnSmoothVel;

    public Move(StateMachine<Mouvement> stateMachine) : base(stateMachine)
    {}
            
    public override void Enter(){}
    public override void Exit(){}

    public override void Update()
    {
        Vector3 move = Vector3.zero;

        if(Input.GetKey(KeyCode.Q))
        {
            move -= Camera.main.transform.right;
        }
        if(Input.GetKey(KeyCode.D))
        {
            move += Camera.main.transform.right;
        }
        if(Input.GetKey(KeyCode.Z))
        {
            move += Camera.main.transform.forward;
        }
        if(Input.GetKey(KeyCode.S))
        {
            move -= Camera.main.transform.forward;
        }

        move.y = 0;
        move.Normalize();

        /*if(move != Vector3.zero)
        {
            Owner.transform.rotation = Quaternion.LookRotation(Vector3.up, move);
        }
        move *= Owner.MoveSpeed * Time.deltaTime;
        Owner.CharController.Move(move);*/

        if(move != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(Owner.transform.eulerAngles.y, targetAngle, ref turnSmoothVel, Owner.TurnSmoothTime);
            Owner.transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        if(Input.GetKey(KeyCode.Space))
        {
            
        }
        
        move.y -= Owner.GravityVel * Time.deltaTime;
        Owner.CharController.Move(move * Time.deltaTime * Owner.MoveSpeed);
        
    }
}