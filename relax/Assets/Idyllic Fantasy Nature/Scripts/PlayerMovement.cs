using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;


    public class PlayerMovement : MonoBehaviour
    {
        [Range(1f, 20f)]
        [SerializeField] private float _movementSpeed;
        [Tooltip("run multiplier of the movement speed")]
        [Range(1f, 20f)]
        [SerializeField] private float _runMultiplier;
        [SerializeField] private float _gravity = -9.81f;
        [Range(1f, 20f)]
        [SerializeField] private float _jumpHeight;

        public CharacterController characterController;
        Vector3 _controllerVelocity;
        public Transform cam;
        public float turnsmoothTime;
        float turnsmoothVelocity;
        Vector3 moveDir;

        public Animator _animator;

        private bool canDetectInput = false;  // Variable to control input detection


        // Start is called before the first frame update
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            StartCoroutine(DelayInputDetection(3f));  // Start the coroutine to delay input

            // Hide the mouse cursor
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
        }

        IEnumerator DelayInputDetection(float delay)
        {
            yield return new WaitForSeconds(delay);  // Wait for 'delay' seconds
            canDetectInput = true;  // Allow input detection after delay
        }

        // Update is called once per frame
        void Update()
        {
            if (!canDetectInput) return;

            // stops the y velocity when player is on the ground and the velocity has reached 0
            if (characterController.isGrounded && _controllerVelocity.y < 0)
            {
                _controllerVelocity.y = 0;
            }

            // get the movement input
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(moveX,0f,moveZ).normalized;
            
            if(direction.magnitude >= 0.1f && canDetectInput)
            {
                float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle, ref turnsmoothVelocity, turnsmoothTime);
                moveDir = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward;
                transform.rotation = Quaternion.Euler(0f,angle,0f);
                characterController.Move(moveDir.normalized*_movementSpeed*Time.deltaTime);
                _animator.SetBool("camina", true);
            }
            else{
                _animator.SetBool("camina", false);
            }

            // gravity affects the controller on the y-axis
            _controllerVelocity.y += _gravity * Time.deltaTime;

            // moves the controller on the y-axis
            characterController.Move(_controllerVelocity * Time.deltaTime);
            
            // the controller is able to jump when on the ground
            if (Input.GetButton("Jump") && characterController.isGrounded)
            {
                _controllerVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            }

            // the controller is able to run
            if (Input.GetKey(KeyCode.LeftShift))
            {
                characterController.Move(moveDir.normalized * Time.deltaTime * _runMultiplier);
                _animator.SetBool("camina", false);
                _animator.SetBool("corre", true);
            }
            else
            {
                _animator.SetBool("corre", false);
            }
            
        }
    }

