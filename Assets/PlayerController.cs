using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool m_moveForward = false;
    private bool m_moveBackward = false;
    private bool m_rotLeft = false;
    private bool m_rotRight = false;
    private bool m_interact = false;
    private float m_inputTimer = 0.0f;

    [SerializeField] private float m_timeBetweenInputs = 0.0f;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_rotationSpeed;

    private readonly Vector3 m_gravity = new Vector3(0, -9.8f, 0);
    private CharacterController m_characterController;

    internal bool IsHoldingMatch { get; set; }

    [SerializeField] private float m_matchDuration = 5.0f;
    [SerializeField] private float m_matchTimer = 0.0f;

    [SerializeField] private GameObject m_matchObject;
    [SerializeField] private TMPro.TextMeshProUGUI m_deathText;
    [SerializeField] private GameObject m_death;

    private bool m_iAmDead = false;

    private int m_oilLamps = 0;

    private float m_deathChance = 0.0f;

    private Light m_light;

    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_light = m_matchObject.GetComponentInChildren<Light>();
        IsHoldingMatch = true;
        m_iAmDead = false;
        m_deathChance = 0.0f;
        m_light.intensity = 3.0f;
        m_matchTimer = 5.0f;
    }

    private void UpdateMatch()
    {
        if (!m_iAmDead)
        {
            if (IsHoldingMatch)
            {
                if (m_matchTimer < 0.01f)
                {
                    m_light.intensity = Mathf.Lerp(m_light.intensity, 0.0f, Time.deltaTime / m_matchDuration);
                    if (m_light.intensity < 0.01f)
                    {
                        m_light.intensity = 0.0f;
                    }
                }
                else
                {
                    m_matchTimer -= Time.deltaTime;
                }
            }
        
            m_deathChance = 100.0f - ((m_light.intensity * 100.0f) / 3.0f);
            m_deathText.text = "How fucked you are: " + (int)m_deathChance + "%";
        }

        if ((int)m_deathChance == 100)
        {
            m_death.SetActive(true);
            m_iAmDead = true;
            if (m_interact && m_inputTimer == 0.0f)
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_iAmDead)
        {
            if (m_moveForward || m_moveBackward)
            {
                Vector3 move = m_moveForward ? (transform.forward * m_speed * Time.deltaTime) : (-transform.forward * (m_speed / 2) * Time.deltaTime);
                m_characterController.Move(move);
            }

            if (m_inputTimer != 0.0f)
            {
                m_inputTimer -= Time.deltaTime * 2;
                m_inputTimer = m_inputTimer < 0.01f ? 0.0f : m_inputTimer;
            }

            if (m_inputTimer == 0.0f)
            {
                if (m_interact)
                {
                    m_inputTimer = m_timeBetweenInputs;
                    m_light.intensity = 3.0f;
                    m_matchTimer = 5.0f;
                }
            }

            m_characterController.Move(m_gravity * Time.deltaTime);

            if (m_rotLeft || m_rotRight)
            {
                float multiplier = m_rotLeft ? -1 : 1;
                transform.Rotate(0, m_rotationSpeed * multiplier * Time.deltaTime * 200.0f, 0);
                m_inputTimer = m_timeBetweenInputs;
            }
        }

        UpdateMatch();
    }

    // Input Actions
    // W
    public void Forward(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        m_moveForward = value > 0;
        //Debug.Log("Forward detected");
    }
    // S
    public void Backward(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        m_moveBackward = value > 0;
        //Debug.Log("Backward detected");
    }
    // A
    public void Left(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        m_rotLeft = value > 0;
        //Debug.Log("Forward detected");
    }
    // D
    public void Right(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        m_rotRight = value > 0;
        //Debug.Log("Backward detected");
    }
    // E or Space
    public void Interact(InputAction.CallbackContext context)
    {
        float button = context.ReadValue<float>();
        m_interact = Math.Abs(button - 1.0f) < 0.1f ? true : false;
        //Debug.Log("Interact detected: " + m_interact);
    }

    void OnTriggerStay(Collider collider)
    {
        if (m_interact && m_inputTimer == 0.0f)
        {
            // add interaction
        }
    }
}
