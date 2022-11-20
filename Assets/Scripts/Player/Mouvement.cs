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
        [SerializeField] private float dashDistance = 10f;
        
        [SerializeField] bool worldDir;

        [SerializeField] private AudioManager _audioManager;
        

        [SerializeField] private float minHeight = 160f;
        [SerializeField] private float dashBaseCoolDown = 2f;
        private float dashCoolDown;

        private float turnSmoothVel;
        private float distToGround;
        
        private float inputH;
        private float inputV;
        private Vector3 move;
        private bool _isGrounded;
        private bool _dashing;
        private bool _gliding;
        private bool _jumping;
        private bool _landed;

        private Animator animator;

        private Rigidbody rigidbody;
        
        private Vector3 _lastSafePosition;

        private void Awake() {
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            distToGround = 0f;
            // TODO : Modifier la valeur de DistToGround quand on aura le vrai personnage
        }

        public void Update()
        {

            _isGrounded = Physics.Raycast(transform.position + 0.01f * Vector3.up, -Vector3.up, out var hitInfo ,distToGround + 0.1f);
            Debug.DrawRay(transform.position, - 0.1f * Vector3.up, Color.red, Mathf.Infinity);
            
            // Sauvegarde de la position du joueur si il est au sol
            if (_isGrounded)
            {
                if (Vector3.Distance(transform.position,hitInfo.collider.gameObject.transform.position + Vector3.up * hitInfo.collider.bounds.extents.y) < 10f) _lastSafePosition = hitInfo.collider.gameObject.transform.position + Vector3.up * hitInfo.collider.bounds.extents.y;
                //_canDash = true;
                if (_gliding)
                {
                    _audioManager.Stop("WindGliding");
                    _gliding = false;
                }

                if (_landed)
                {
                    _audioManager.Play("Landing");
                }

            }

            move = Vector3.zero;

            _landed = !_isGrounded;

                // Tp du joueur en sécurité si il est tombé trop bas
            if (transform.position.y < minHeight)
            {
                transform.position = _lastSafePosition;
            }

            if(Input.GetKey(KeyCode.Q))
            {
                move -= worldDir ? Vector3.right : Camera.main.transform.right;
            }
            if(Input.GetKey(KeyCode.D))
            {
                move += worldDir ? Vector3.right : Camera.main.transform.right;
            }
            if(Input.GetKey(KeyCode.Z))
            {
                move += worldDir ? Vector3.forward : Camera.main.transform.forward;
            }
            if(Input.GetKey(KeyCode.S))
            {
                move -= worldDir ? Vector3.forward : Camera.main.transform.forward;
            }

            inputH = Input.GetAxis("Horizontal");
            inputV = Input.GetAxis("Vertical");

            move.y = 0;
            move.Normalize();

            animator.SetBool("Run", move != Vector3.zero);
            animator.SetBool("IsGrounded", _isGrounded);

            if(move != Vector3.zero && !_dashing)
            {
                float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle + 180, ref turnSmoothVel, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                StartCoroutine(Jump());
                _jumping = true;
                _audioManager.Play("Jump");
                animator.SetTrigger("Jump");
            }
            
            dashCoolDown -= Time.deltaTime;
            // Lancement du dash
            if (Input.GetKeyDown(KeyCode.RightShift) && dashCoolDown < 0)//&& _canDash)
            {
                animator.SetTrigger("Dash");
                StartCoroutine(Dash());
                _audioManager.Play("Dash");
                _dashing = true;

            }

            // Physique de la chute
            animator.SetBool("Gliding", rigidbody.velocity.y <= 0 && !_isGrounded && Input.GetKey(KeyCode.Space));

            if (!_jumping && !_isGrounded)
            {
                var newVelocity = rigidbody.velocity;
                newVelocity.y = Math.Max(newVelocity.y - 60 * Time.deltaTime, (Input.GetKey(KeyCode.Space)) ? (-glideGravityVel) : (-usualGravityVel));

                if (!_gliding && Input.GetKey(KeyCode.Space))
                {
                    _audioManager.Play("Glide");
                    _audioManager.Play("WindGliding");
                    _gliding = true;
                }

                if (_gliding && !Input.GetKey(KeyCode.Space))
                {
                    _audioManager.Stop("WindGliding");
                    _gliding = false;
                }

                if (!_dashing) rigidbody.velocity = newVelocity;
            }

            // var movement = new Vector3(inputH,move.y,inputV); 

            // rigidbody.MovePosition(rigidbody.position + movement * Time.deltaTime * moveSpeed);
            
        }

        void FixedUpdate()
        {
            var movement = new Vector3(inputH,rigidbody.velocity.y / moveSpeed,inputV); 

            // rigidbody.MovePosition(rigidbody.position + movement * Time.deltaTime * moveSpeed);
            if (!_dashing) rigidbody.velocity = movement * moveSpeed;
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
            _jumping = false;
            
            yield return null;
        }

        IEnumerator Dash()
        {
            var dashProgress = 0f;
            while (dashProgress < 0.75f)
            {
                dashCoolDown = dashBaseCoolDown;
                if (dashProgress < 0.25f) {rigidbody.velocity = Vector3.zero; yield return null;}
                var newVelocity = rigidbody.velocity;

                var directions = new Vector2(
                    Vector3.Dot(transform.forward, Vector3.right),
                    Vector3.Dot(transform.forward, Vector3.forward)
                );

                newVelocity = new Vector3(-directions.x * dashDistance * (1 - dashProgress) * (1 - dashProgress),
                                            0,//newVelocity.y, 
                                            -directions.y * dashDistance * (1 - dashProgress) * (1 - dashProgress));

                rigidbody.velocity = moveSpeed * newVelocity;
                dashProgress += Time.deltaTime;
                yield return null;

            }

            _dashing = false;
            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
            yield return null;
        }


    }
}
