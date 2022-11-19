using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Player
{
    public class Mouvement : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 20f;
        [SerializeField] float turnSmoothTime = 0.1f;
        [SerializeField] float usualGravityVel = 10f;
        [SerializeField] float glideGravityVel = 10f;
        [SerializeField] float jumpVel = 10f;

        private float turnSmoothVel;
        private bool _canJump = true;
        private float distToGround;
        
        private float _velocityY;
        private float inputH;
        private float inputV;
        private bool _jumpCancel;
        private bool _isGrounded;
        private Animator animator;

        private Rigidbody rigidbody;

        private void Awake() {
            rigidbody = GetComponent<Rigidbody>();
            _velocityY = 0;
            animator = GetComponent<Animator>();
            
            distToGround = 0f;
            // TODO : Modifier la valeur de DistToGround quand on aura le vrai personnage
        }

        public void Update()
        {
            
            _isGrounded = Physics.Raycast(transform.position + 0.01f * Vector3.up, -Vector3.up, distToGround + 0.1f);

            if (_isGrounded)
            {
                _canJump = true;
            }

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

            inputH = Input.GetAxis("Horizontal");
            inputV = Input.GetAxis("Vertical");

            move.y = 0;
            move.Normalize();

            animator.SetBool("Run", move != Vector3.zero);
            animator.SetBool("IsGrounded", _isGrounded);

            if(move != Vector3.zero)
            {
                float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle + 180, ref turnSmoothVel, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            if (Input.GetKeyDown(KeyCode.Space) && _canJump)
            {
                //rigidbody.AddForce(Vector3.up * jumpVel, ForceMode.Impulse);
                StartCoroutine(Jump());
                animator.SetTrigger("Jump");
                
                _canJump = _isGrounded;
            }

            // Physique de la chute
            animator.SetBool("Gliding", rigidbody.velocity.y <= 0 && !_isGrounded && Input.GetKey(KeyCode.Space));

            if (rigidbody.velocity.y <= 0 && !_isGrounded)
            {
                var newVelocity = rigidbody.velocity;
                newVelocity.y = Math.Max(newVelocity.y - 60 * Time.deltaTime, (Input.GetKey(KeyCode.Space)) ? (-glideGravityVel) : (-usualGravityVel));

                rigidbody.velocity = newVelocity;
            }

            var movement = new Vector3(inputH,move.y,inputV); 

            //rigidbody.AddForce(Vector3.down * curGravityVel * Time.deltaTime, ForceMode.Acceleration);
            rigidbody.MovePosition(rigidbody.position + movement * Time.deltaTime * moveSpeed);
            
        }

        IEnumerator Jump()
        {
            var jumpProgress = 0f;

            while (!_jumpCancel && jumpProgress < 0.5f)
            {
                _velocityY = jumpVel * (1-jumpProgress)*(1-jumpProgress);
                var newVelocity = rigidbody.velocity;
                newVelocity.y = _velocityY;

                rigidbody.velocity = newVelocity;

                jumpProgress += Time.deltaTime;
                yield return null;
            }

            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
            
            yield return null;
        }


    }
}
