using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startsceneManager : MonoBehaviour
{
    public void Startbtn()
    {
        GameObject gm = GameObject.Find("GameManager");

        if (gm != null)
        {
            gm.GetComponent<GameManager>().spell_list = new int[3] { 0, 1, 2 };
            gm.GetComponent<GameManager>().FirstGameStart();
            gm.GetComponent<GameManager>().IdleSceneChange();
        }
    }
}
