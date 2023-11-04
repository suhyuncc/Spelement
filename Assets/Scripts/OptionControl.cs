using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OptionControl : MonoBehaviour
{
    [SerializeField]
    private GameObject optionPanel;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionPanel.activeSelf)
            {
                Time.timeScale = 0;
                optionPanel.SetActive(false);
            }
            else
            {
                Time.timeScale = 1;
                optionPanel.SetActive(true);
            }
        }
    }

}
