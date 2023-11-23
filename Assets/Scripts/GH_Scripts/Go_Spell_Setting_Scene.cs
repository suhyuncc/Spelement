using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Go_Spell_Setting_Scene : MonoBehaviour
{
    public void ChangeIdleScene()
    {
        GameObject gm = GameObject.Find("GameManager");

        if (gm != null)
        {
            gm.GetComponent<GameManager>().IdleSceneChange();
        }
    }
}
