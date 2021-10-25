using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField] private GameObject m_playerRef;
    private UnityEngine.Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_playerRef.GetComponent<PlayerController>().m_moveForward || m_playerRef.GetComponent<PlayerController>().m_moveBackward)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        if (m_playerRef.GetComponent<PlayerController>().m_interact)
        {
            animator.SetBool("IsInteracting", true);
        }
        else
        {
            animator.SetBool("IsInteracting", false);
        }
    }
}
