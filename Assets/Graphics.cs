using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graphics : MonoBehaviour
{
    [SerializeField] private GameObject graphicsLow;
    [SerializeField] private GameObject graphicsMed;
    [SerializeField] private GameObject graphicsHigh;

    public void SetGraphics(int level)
    {
        QualitySettings.SetQualityLevel(level);
    }

    void Start()
    {
        switch (QualitySettings.GetQualityLevel())
        {
            case (0):
                {
                    graphicsLow.GetComponent<Button>().Select();
                    break;
                }
            case (2):
                {
                    graphicsMed.GetComponent<Button>().Select();
                    break;
                }
            case (4):
                {
                    graphicsHigh.GetComponent<Button>().Select();
                    break;
                }
            default:
                {
                    graphicsMed.GetComponent<Button>().Select();
                    break;
                }
        }
    }

    void Update()
    {
        switch (QualitySettings.GetQualityLevel())
        {
            case (0):
            {
                graphicsLow.GetComponent<Button>().Select();
                break;
            }
            case (2):
            {
                graphicsMed.GetComponent<Button>().Select();
                break;
            }
            case (4):
            {
                graphicsHigh.GetComponent<Button>().Select();
                break;
            }
            default:
            {
                graphicsMed.GetComponent<Button>().Select();
                break;
            }
        }
    }
}
