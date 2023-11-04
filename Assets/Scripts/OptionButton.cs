using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//gh
public class OptionButton : MonoBehaviour
{
    [SerializeField]
    private GameObject optionPanel;
    public void XButtonClick()
    {
        Time.timeScale = 1;
        optionPanel.SetActive(false);
    }

    public void GameEndButtonClick()
    {
        Application.Quit();
    }

    public void AdviceButtonClick()
    {

    }
}
