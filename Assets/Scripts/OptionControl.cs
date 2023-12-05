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
            option();
        }
    }
    
    public GameObject GetOptionPanel() 
    {
        return optionPanel;
    }

    public void option()
    {
        if (optionPanel.activeSelf)
        {
            Time.timeScale = 1;
            optionPanel.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            optionPanel.SetActive(true);
        }
    }
}
