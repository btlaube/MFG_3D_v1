using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float m_speed = 4.0f;
    [SerializeField] private float m_jumpForce = 7.5f;

    private Animator m_animator;
    private Rigidbody rb;
    private GroundSensor m_groundSensor;
    private bool m_grounded = false;
    private bool m_combatIdle = false;
    private bool m_isDead = false;

    private float _targetRotation = 0.0f;
    [SerializeField] private float RotationSmoothTime = 0.12f;
    private float _rotationVelocity;

    private PlayerInput playerInput;

    private void Awake() {
        m_animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<GroundSensor>();

        playerInput = new PlayerInput();
        playerInput.Player.Enable();
    }

    private void OnEnable() {
        playerInput.Player.Jump.performed += Jump;
        playerInput.Player.Attack.performed += Attack;
    }

    private void OnDisable() {
        playerInput.Player.Jump.performed -= Jump;
        playerInput.Player.Attack.performed -= Attack;
    }
	
	void FixedUpdate () {
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State()) {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if(m_grounded && !m_groundSensor.State()) {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        Vector2 inputVector = playerInput.Player.Movement.ReadValue<Vector2>();
        Vector3 inputDirection = new Vector3(inputVector.x, 0.0f, inputVector.y);

        if (inputVector != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;// +
                                // _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        // Move
        rb.velocity = new Vector3(inputVector.x * m_speed, rb.velocity.y, inputVector.y * m_speed);

        //Set AirSpeed in animator
        // m_animator.SetFloat("AirSpeed", rb.velocity.y);

        // -- Handle Animations --
        //Death
        if (Input.GetKeyDown("t")) {
            if(!m_isDead)
                m_animator.SetTrigger("Death");
            else
                m_animator.SetTrigger("Recover");

            m_isDead = !m_isDead;
        }
            
        //Hurt
        else if (Input.GetKeyDown("q"))
            m_animator.SetTrigger("Hurt");

        //Change between idle and combat idle
        else if (Input.GetKeyDown("f"))
            m_combatIdle = !m_combatIdle;

        //Run
        else if (Mathf.Abs(inputVector.x) > Mathf.Epsilon || Mathf.Abs(inputVector.y) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);
    }

    private void Jump(InputAction.CallbackContext context) {
        if (m_grounded) {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            rb.velocity = new Vector3(rb.velocity.x, m_jumpForce, rb.velocity.z);
            m_groundSensor.Disable(0.2f);
        }
    }

    private void Attack(InputAction.CallbackContext context) {
        rb.velocity = Vector3.zero;
        m_animator.SetTrigger("Attack");
    }
            
}
