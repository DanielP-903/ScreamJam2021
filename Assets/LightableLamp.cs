using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightableLamp : MonoBehaviour
{
    internal bool Lit { get; set; }

    void Update()
    {
        GetComponentInChildren<Light>().enabled = Lit;
    }
}
