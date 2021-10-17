using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject m_player;
    [SerializeField] private Vector3 m_offset;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(m_player.transform.position.x + m_offset.x, m_player.transform.position.y + m_offset.y, m_player.transform.position.z + m_offset.z);
        transform.position = newPos;
    }
}
