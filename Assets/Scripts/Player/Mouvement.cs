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
        
        // TODO : Passser les trois variables des sauts en privé une fois qu'on aura trouvé des valeurs qui nous conviennent
        [SerializeField] float usualGravityVel = 10f;
        [SerializeField] float glideGravityVel = 10f;
        [SerializeField] float jumpVel = 10f;

        private float turnSmoothVel;
        private bool _canJump = true;
        private float distToGround;
        
        private float _velocityY;
        private bool _isGrounded;

        private Rigidbody rigidbody;

        private void Awake() {
            rigidbody = GetComponent<Rigidbody>();
            _velocityY = 0;
            
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

            move.y = 0;
            move.Normalize();

            if(move != Vector3.zero)
            {
                float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            if (Input.GetKeyDown(KeyCode.Space) && _canJump)
            {
                StartCoroutine(Jump());
                
                _canJump = _isGrounded;
            }
            
            // Physique de la chute
            if (rigidbody.velocity.y <= 0 && !_isGrounded)
            {
                var newVelocity = rigidbody.velocity;
                newVelocity.y = Math.Max(newVelocity.y - 60 * Time.deltaTime, (Input.GetKey(KeyCode.Space)) ? (-glideGravityVel) : (-usualGravityVel));

                rigidbody.velocity = newVelocity;
            }

            rigidbody.MovePosition(rigidbody.position + move * Time.deltaTime * moveSpeed);
            
        }

        IEnumerator Jump()
        {
            var jumpProgress = 0f;

            while (jumpProgress < 0.5f)
            {
                var newVelocity = rigidbody.velocity;
                newVelocity.y = jumpVel * (1-jumpProgress)*(1-jumpProgress);

                rigidbody.velocity = newVelocity;

                jumpProgress += Time.deltaTime;
                yield return null;
            }

            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
            
            yield return null;
        }


    }
}
