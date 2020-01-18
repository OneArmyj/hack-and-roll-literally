using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private Collider2D m_Attack1;
    [SerializeField]
    private Collider2D m_Attack2;

    [SerializeField]
    private PlayerMovement m_PlayerMovement;
    [SerializeField]
    private PlayerControls m_PlayerControls;

    [SerializeField]
    private float m_AnimationDuration = 0.4f;

    private float m_LastAttackTime;

    void Awake()
    {
        m_PlayerControls.m_Attack1Action.performed += ctx => {
            if (!m_Attack1.enabled && !m_Attack2.enabled)
            {
                m_Attack1.enabled = true;
                m_LastAttackTime = Time.time;
            }
        };
        m_PlayerControls.m_Attack2Action.performed += ctx => {
            if (!m_Attack1.enabled && !m_Attack2.enabled) {
                m_Attack2.enabled = true;
                m_LastAttackTime = Time.time;
            }
        };
    }

    void Update()
    {
        CheckAttackPressed();

        if (Time.time > m_LastAttackTime + m_AnimationDuration)
        {
            m_Attack1.enabled = false;
            m_Attack2.enabled = false;
        }
    }

    void CheckAttackPressed()
    {
        if (!m_Attack1.enabled && !m_Attack2.enabled)
        {
            PlayerControls controls = m_PlayerMovement.m_PlayerControls;
            if (controls.m_Attack1Action.triggered)
            {
                m_Attack1.enabled = true;
                m_LastAttackTime = Time.time;
            }
            else if (controls.m_Attack2Action.triggered)
            {
                m_Attack2.enabled = true;
                m_LastAttackTime = Time.time;
            }
        }
    }

    // Our character gets hit
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Attack")
        {
            int direction = transform.position.x < other.transform.position.x ? -1 : 1;
            m_PlayerMovement.OnHit(direction);
        }
    }

    void OnEnable()
    {
        m_PlayerControls.m_Attack1Action.Enable();
        m_PlayerControls.m_Attack2Action.Enable();
    }

    void OnDisable() {
        m_PlayerControls.m_Attack1Action.Disable();
        m_PlayerControls.m_Attack2Action.Disable();
    }
}
