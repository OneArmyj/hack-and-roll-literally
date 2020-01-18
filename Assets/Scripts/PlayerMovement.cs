using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public PlayerControls m_PlayerControls;

    [SerializeField]
    private float m_Torque = 7;
    [SerializeField]
    private float m_MaxAngularVelocity = 200;
    [SerializeField]
    private float m_MoveSpeed = 4;
    [SerializeField]
    private float m_JumpSpeed = 4;

    [SerializeField]
    private Rigidbody2D m_Rigidbody;
    [SerializeField]
    private GameObject m_PlayerCharacter;
    [SerializeField]
    private GameObject m_PlayerBall;
    [SerializeField]
    private GameObject m_GroundCheck;

    private bool m_Hit;
    private float m_TimeLastHit;
    private float m_MoveDirection;
    private Vector3 m_JumpVelocity;
    // 0 = not jumping, 1 = should jump, 2 = jumped
    private int m_JumpState = 0;
    private bool m_Attached = true;

    void Awake()
    {
        m_PlayerControls = GetComponent<PlayerControls>();
        m_PlayerControls.m_MoveAction.performed += ctx => {
            m_MoveDirection = ctx.ReadValue<float>();
        };
        m_PlayerControls.m_JumpAction.performed += ctx => {
            if (m_JumpState == 0)
                m_JumpState = 1;
        };
    }

    void Update()
    {
        if (m_Hit && Time.time > m_TimeLastHit + 0.3f)
            m_Hit = false;

        if (!m_Hit)
            CheckMovementPressed();

        CheckJumpPressed();

        HandleJump();
        HandleMove();
    }

    void CheckMovementPressed()
    {
        m_MoveDirection = m_PlayerControls.m_MoveAction.ReadValue<float>();
    }

    void CheckJumpPressed()
    {
        if (m_PlayerControls.m_JumpAction.triggered && m_JumpState == 0)
        {
            m_JumpState = 1;
        }
    }

    void HandleJump()
    {
        if (m_JumpState == 1)
        {
            // decouple character, shift controller to character
            m_Attached = false;

            // launch character into air
            m_JumpVelocity = new Vector3(0, m_JumpSpeed, 0);
            m_JumpState = 2;
        }
        else if (m_JumpState == 2)
        {
            // Adjust player height
            m_PlayerCharacter.transform.position += m_JumpVelocity * Time.deltaTime;
            m_JumpVelocity += Physics.gravity * Time.deltaTime;

            // is falling down
            if (m_JumpVelocity.y <= 0)
            {
                bool grounded = CheckIfGrounded();

                // not on ground, can check for ball landing
                if (!grounded)
                {
                    Vector3 landingPos = m_PlayerBall.transform.position + new Vector3(0, 0.5f, 0);
                    Vector3 offset = m_GroundCheck.transform.position - landingPos;
                    if (offset.magnitude <= 0.2f)
                    {
                        m_PlayerCharacter.transform.position = landingPos + new Vector3(0, 0.5f, 0);
                        m_JumpState = 0;
                        m_Attached = true;
                    }
                }
                // on ground, no longer jumping
                else
                {
                    m_JumpState = 0;
                }
            }
        }
    }

    void HandleMove()
    {
        if (m_Attached)
        {
            m_Rigidbody.AddTorque(-m_Torque * m_MoveDirection * Time.deltaTime);
            if (m_Rigidbody.angularVelocity > m_MaxAngularVelocity)
                m_Rigidbody.angularVelocity = m_MaxAngularVelocity;
            else if (m_Rigidbody.angularVelocity < -m_MaxAngularVelocity)
                m_Rigidbody.angularVelocity = -m_MaxAngularVelocity;
            m_PlayerCharacter.transform.position = m_PlayerBall.transform.position + new Vector3(0, 1, 0);
        }
        else
        {
            m_PlayerCharacter.transform.position += new Vector3(m_MoveDirection * m_MoveSpeed * Time.deltaTime, 0, 0);
        }
    }

    bool CheckIfGrounded()
    {
        return m_GroundCheck.transform.position.y < 0.01f;
    }

    public void OnHit(int direction)
    {
        if (m_Attached)
        {
            Vector2 force = new Vector2(direction * 3, 0);
            m_MoveDirection = 0;
            m_Rigidbody.AddForce(force, ForceMode2D.Impulse);
            m_PlayerCharacter.transform.position = m_PlayerBall.transform.position + new Vector3(0, 1, 0);
        }
        else
        {
            m_MoveDirection = direction;
        }
        m_Hit = true;
        m_TimeLastHit = Time.time;
    }

    void OnEnable()
    {
        m_PlayerControls.m_MoveAction.Enable();
        m_PlayerControls.m_JumpAction.Enable();
    }

    void OnDisable() {
        m_PlayerControls.m_MoveAction.Disable();
        m_PlayerControls.m_JumpAction.Disable();
    }
}
