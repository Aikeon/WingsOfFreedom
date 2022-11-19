using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Walk : State<Mouvement>
{
    public Walk(StateMachine<Mouvement> stateMachine) : base(stateMachine)
    {}
            
    public override void Enter(){}
    public override void Exit(){}

    public override void Update()
    {}
    public override void FixedUpdate(){}
}