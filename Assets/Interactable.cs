using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    enum ObjectType
    {
        Default,
        Key,
        Oil,
        Misc
    }

    internal bool IsPickedUp { get; set; }

}
