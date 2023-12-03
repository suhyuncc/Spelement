using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startsceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Startbtn()
    {
        GameObject gm = GameObject.Find("GameManager");

        if (gm != null)
        {
            gm.GetComponent<GameManager>().spell_list = new int[3] { 0, 1, 2 };
            gm.GetComponent<GameManager>().IdleSceneChange();
        }
    }
}
