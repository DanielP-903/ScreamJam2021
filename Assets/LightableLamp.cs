using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightableLamp : MonoBehaviour
{
    internal bool Lit { get; set; }
    [SerializeField] private ParticleSystem pSystem;
    void Update()
    {
        GetComponentInChildren<Light>().enabled = Lit;
        if (Lit)
        {
            Debug.Log("OIL");
            if (!pSystem.isPlaying) { pSystem.Play(); }
            //pSystem.emission.enabled = true;

        }
        else
        {
            if (pSystem.isPlaying) { pSystem.Stop(); }
            //pSystem.emission.enabled = false;
        }
    }
}
