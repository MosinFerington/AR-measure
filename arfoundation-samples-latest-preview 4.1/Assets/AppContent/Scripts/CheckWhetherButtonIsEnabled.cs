using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWhetherButtonIsEnabled : MonoBehaviour
{
    public GameObject BackgroundAddButton;

    void OnEnable()
    {
        if (BackgroundAddButton)
        {
            BackgroundAddButton.SetActive(true);
        }
    }

    void OnDisable()
    {
        if (BackgroundAddButton)
        {
            BackgroundAddButton.SetActive(false);
        }
    }
}
