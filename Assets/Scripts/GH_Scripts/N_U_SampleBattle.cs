using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_U_SampleBattle : MonoBehaviour
{
    public void isClicked()
    {
        GameObject GM = GameObject.Find("GameManager");
        GM.GetComponent<GameManager>().currentStageCleared = true;
    }
    public void isDClicked()
    {
        GameObject GM = GameObject.Find("GameManager");
        GM.GetComponent<GameManager>().GetEventName("Example");
        GM.GetComponent<GameManager>().currentStageCleared = true;

    }
}
