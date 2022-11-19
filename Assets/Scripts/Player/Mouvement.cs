using System;
using UnityEngine;

namespace Player
{
    public class Mouvement : MonoBehaviour
    {
        private StateMachine<Mouvement> stateMachine;
        
        private void Start()
        {
            stateMachine = new StateMachine<Mouvement>(this);
            stateMachine.setState(new Grounded(stateMachine));
        }

        private void Update()
        {
            if (Input.GetKey("up"))
            {
                Debug.Log("Hello");
            }
            stateMachine.update();
        }

        private void FixedUpdate()
        {}
    }
}
