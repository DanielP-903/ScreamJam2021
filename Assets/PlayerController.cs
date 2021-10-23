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
    internal bool m_moveForward = false;
    internal bool m_moveBackward = false;
    internal bool m_rotLeft = false;
    internal bool m_rotRight = false;
    internal bool m_interact = false;
    private float m_inputTimer = 0.0f;

    [SerializeField] private float m_timeBetweenInputs = 0.0f;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_rotationSpeed;

    private readonly Vector3 m_gravity = new Vector3(0, -9.8f, 0);
    private CharacterController m_characterController;
    [SerializeField] private GameObject darkness;
    [SerializeField] private GameObject hands;
    internal bool IsHoldingMatch { get; set; }

    //[SerializeField] private float m_matchFizzleDuration = 0.0f;
    [SerializeField] private float m_matchWaitTimer = 0.0f;
    private float m_matchTimer = 0.0f;

    [SerializeField] private GameObject m_matchObject;
    //[SerializeField] private TMPro.TextMeshProUGUI m_deathText;
    [SerializeField] private TMPro.TextMeshProUGUI m_oilText;
    [SerializeField] private GameObject m_deathScreen;
    [SerializeField] private GameObject m_winScreen;
    //[SerializeField] private GameObject m_safe;
    [SerializeField] private float m_lightDistanceThreshold;

    [SerializeField] private GameObject _pAdd;
    [SerializeField] private GameObject _pAlpha;
    [SerializeField] private GameObject _pGlow;
    [SerializeField] private AudioSource ghostVoice;
    [SerializeField] private AudioSource ambience1;
    [SerializeField] private AudioSource ambience2;
    private bool m_iAmDead = false;
    private bool m_win = false;

    private int m_oilCans = 0;
    private int m_litLamps = 0;
    private int m_depositedOil = 0;
    private float m_deathChance = 0.0f;

    private Light m_light;

    void Start()
    {
        m_win = false;
        m_deathScreen.SetActive(false);
        m_winScreen.SetActive(false);
        m_characterController = GetComponentInParent<CharacterController>();
        m_light = m_matchObject.GetComponentInChildren<Light>();
        IsHoldingMatch = true;
        m_iAmDead = false;
        m_deathChance = 0.0f;
        m_light.intensity = 3.0f;
        _pAdd.transform.localScale = new Vector3((m_light.intensity / 3.0f), (m_light.intensity / 3.0f), (m_light.intensity / 3.0f));
        _pAlpha.transform.localScale = new Vector3((m_light.intensity / 3.0f), (m_light.intensity / 3.0f), (m_light.intensity / 3.0f));
        _pGlow.transform.localScale = new Vector3((m_light.intensity / 3.0f), (m_light.intensity / 3.0f), (m_light.intensity / 3.0f));
        darkness.transform.localScale = new Vector3(1.0f - (m_light.intensity / 3.0f), 1.0f - (m_light.intensity / 3.0f), 1.0f - (m_light.intensity / 3.0f));
        hands.transform.localScale = new Vector3(1.0f - (m_light.intensity / 3.0f), 1.0f - (m_light.intensity / 3.0f), 1.0f - (m_light.intensity / 3.0f));
        ghostVoice.volume = 1.0f - (m_light.intensity / 3.0f);
        ambience1.volume = (m_light.intensity / 3.0f);
        ambience2.volume = (m_light.intensity / 3.0f);
        ghostVoice.volume = 0.0f;
        ghostVoice.loop = true;

        m_matchTimer = m_matchWaitTimer;
        m_oilCans = 0;
        m_oilText.text = m_oilCans.ToString();
        m_litLamps = 0;
        m_depositedOil = 0;
        Physics.IgnoreCollision(GetComponentInParent<Collider>(), GetComponent<Collider>());
    }

    private bool CheckNearLightSources()
    {
        GameObject[] lightSources = GameObject.FindGameObjectsWithTag("LightableLamp");
        foreach (var source in lightSources)
        {
            if (source.GetComponent<LightableLamp>())
            {
                if (Vector3.Distance(source.transform.position, transform.parent.position) < m_lightDistanceThreshold &&
                    source.GetComponent<LightableLamp>().Lit)
                {
                    return true;
                }
            }

        }
        GameObject[] campfires = GameObject.FindGameObjectsWithTag("CampFire");
        foreach (var source in campfires)
        {
            if (Vector3.Distance(source.transform.position, transform.parent.position) < m_lightDistanceThreshold + 2.0f)
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateMatch()
    {
        if (!m_iAmDead)
        {
            bool isCloseToLightSource = CheckNearLightSources();
            if (IsHoldingMatch && !isCloseToLightSource)
            {
                //m_safe.SetActive(false);
                if (m_matchTimer < 0.01f)
                {
                    if (ghostVoice.isPlaying == false)
                    {
                        ghostVoice.Play();
                    }

                    m_light.intensity = Mathf.Lerp(m_light.intensity, 0.0f, Time.deltaTime / 1.0f);
                    _pAdd.transform.localScale = new Vector3((m_light.intensity / 3.0f), (m_light.intensity / 3.0f), (m_light.intensity / 3.0f));
                    _pAlpha.transform.localScale = new Vector3((m_light.intensity / 3.0f), (m_light.intensity / 3.0f), (m_light.intensity / 3.0f));
                    _pGlow.transform.localScale = new Vector3((m_light.intensity / 3.0f), (m_light.intensity / 3.0f), (m_light.intensity / 3.0f));

                    darkness.transform.localScale = new Vector3(1.0f - (m_light.intensity/3.0f), 1.0f - (m_light.intensity / 3.0f), 1.0f - (m_light.intensity / 3.0f));
                    hands.transform.localScale = new Vector3(1.0f - (m_light.intensity/3.0f), 1.0f - (m_light.intensity / 3.0f), 1.0f - (m_light.intensity / 3.0f));

                    ghostVoice.volume = 1.0f - (m_light.intensity / 3.0f);
                    ambience1.volume = (m_light.intensity / 3.0f);
                    //ambience2.volume = (m_light.intensity / 3.0f);

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
            else if (isCloseToLightSource)
            {
                //m_safe.SetActive(true);
                m_light.intensity = 3.0f;
                _pAdd.transform.localScale = new Vector3((m_light.intensity / 3.0f), (m_light.intensity / 3.0f), (m_light.intensity / 3.0f));
                _pAlpha.transform.localScale = new Vector3((m_light.intensity / 3.0f), (m_light.intensity / 3.0f), (m_light.intensity / 3.0f));
                _pGlow.transform.localScale = new Vector3((m_light.intensity / 3.0f), (m_light.intensity / 3.0f), (m_light.intensity / 3.0f));
                darkness.transform.localScale = new Vector3(1.0f - (m_light.intensity / 3.0f), 1.0f - (m_light.intensity / 3.0f), 1.0f - (m_light.intensity / 3.0f));
                hands.transform.localScale = new Vector3(1.0f - (m_light.intensity / 3.0f), 1.0f - (m_light.intensity / 3.0f), 1.0f - (m_light.intensity / 3.0f));
                ghostVoice.volume = 1.0f - (m_light.intensity / 3.0f);
                ambience1.volume = (m_light.intensity / 3.0f);
                ambience2.volume = (m_light.intensity / 3.0f);
                m_matchTimer = m_matchWaitTimer;
                if (ghostVoice.isPlaying == true)
                {
                    ghostVoice.Stop();
                }
            }
        
            m_deathChance = 100.0f - ((m_light.intensity * 100.0f) / 3.0f);
            //m_deathText.text = "How fucked you are: " + (int)m_deathChance + "%";
        }

        if ((int)m_deathChance == 100)
        {
            m_deathScreen.SetActive(true);
            m_iAmDead = true;
            ghostVoice.loop = false;
            if(ambience1.isPlaying) {ambience1.Stop();}
            if(ambience2.isPlaying) {ambience2.Stop();}
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
                Vector3 move = m_moveForward ? (transform.forward * m_speed * Time.deltaTime) : (-transform.forward * (m_speed / 1.5f) * Time.deltaTime);
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

        if (m_win)
        {
            if (m_interact && m_inputTimer == 0.0f)
            {
                SceneManager.LoadScene(0);
            }
        }
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
            if (collider.gameObject.GetComponent<LightableLamp>())
            {
                if (!collider.gameObject.GetComponent<LightableLamp>().Lit)
                {
                    collider.gameObject.GetComponent<LightableLamp>().Lit = true;
                    m_litLamps++;
                }

                m_light.intensity = 3.0f;
                m_matchTimer = m_matchWaitTimer;
            }
            else if (collider.gameObject.GetComponent<OilCanPickup>())
            {
                if (!collider.gameObject.GetComponent<OilCanPickup>().PickedUp)
                {
                    collider.gameObject.GetComponent<OilCanPickup>().PickedUp = true;
                    m_oilCans++;
                    m_oilText.text = m_oilCans.ToString();
                    collider.gameObject.SetActive(false);
                }
            }
            else if (collider.tag == "Well")
            {
                m_depositedOil = m_oilCans;
                if (m_depositedOil >= 3)
                {
                    m_win = true;
                    m_iAmDead = true;
                    m_winScreen.SetActive(true);
                    ambience1.Stop();
                    ambience2.Stop();
                    ghostVoice.Stop();
                    Debug.Log("WIN!");
                }
            }
        }
    }
}
