using System;
using UnityEngine;

namespace Player
{
    public class Mouvement : MonoBehaviour
    {
        private StateMachine<Mouvement> stateMachine;

        public class Grounded : State<Mouvement>
        {
            public Grounded(StateMachine<Mouvement> stateMachine) : base(stateMachine)
            {
            }
            
            public override void Enter(){}
            public override void Exit(){}
            public override void Update(){}
            public override void FixedUpdate(){}
        }
        
        private void Start()
        {
            stateMachine = new StateMachine<Mouvement>(this);
            stateMachine.setState(new Grounded(stateMachine));
            
            throw new NotImplementedException();
        }

        private void Update()
        {
            stateMachine.update();
            throw new NotImplementedException();
        }

        private void FixedUpdate()
        {
            throw new NotImplementedException();
        }
        
        
    }
}
