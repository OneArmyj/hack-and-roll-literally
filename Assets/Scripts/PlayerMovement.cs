using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
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

    private float m_MoveDirection;
    private Vector3 m_JumpVelocity;
    // 0 = not jumping, 1 = should jump, 2 = jumped
    private int m_JumpState = 0;
    private bool m_Attached = true;

    void Update()
    {
        HandleJump();
        HandleMove();
    }

    public void HandleJump()
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

    public void HandleMove()
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

    public bool CheckIfGrounded()
    {
        return m_GroundCheck.transform.position.y < 0.01f;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_MoveDirection = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (m_JumpState == 0)
            m_JumpState = 1;
    }
}
