using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class Mouvement : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 20f;
        [SerializeField] float turnSmoothTime = 0.1f;
        [SerializeField] float gravityVel = 10f;
        [SerializeField] float jumpVel = 10f;

        private float turnSmoothVel;

        private CharacterController charController;

        public CharacterController CharController => charController;
        public float MoveSpeed => moveSpeed;
        public float TurnSmoothTime => turnSmoothTime;
        public float GravityVel => gravityVel;

        private void Awake() {
            charController = GetComponent<CharacterController>();
        }

        public void Update()
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

            if(move != Vector3.zero)
            {
                float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, TurnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                Rigidbody.AddForce();
            }
            
            move.y -= GravityVel * Time.deltaTime;
            CharController.Move(move * Time.deltaTime * MoveSpeed);
            
        }


    }
}
