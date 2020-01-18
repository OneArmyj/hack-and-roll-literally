using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float m_Torque = 40;

    [SerializeField]
    private float m_MaxAngularVelocity = 500;

    private Rigidbody2D m_Rigidbody;

    private float m_MoveDirection;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Rigidbody.AddTorque(-m_Torque * m_MoveDirection * Time.deltaTime);
        if (m_Rigidbody.angularVelocity > m_MaxAngularVelocity)
            m_Rigidbody.angularVelocity = m_MaxAngularVelocity;
        else if (m_Rigidbody.angularVelocity < -m_MaxAngularVelocity)
            m_Rigidbody.angularVelocity = -m_MaxAngularVelocity;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_MoveDirection = context.ReadValue<float>();
    }
}
