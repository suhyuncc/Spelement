using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class winlose_panel : MonoBehaviour
{
    public bool is_Win;

    [SerializeField]
    private Text winlose_txt;
    [SerializeField]
    private GameObject book;
    [SerializeField]
    private GameObject Ele_pool;

    private void OnEnable()
    {
        book.SetActive(false);
        Ele_pool.SetActive(false);
        Time.timeScale = 0;
        if(is_Win)
        {
            winlose_txt.color = Color.blue;
            winlose_txt.text = "½Â¸®!!!!!";
        }
        else
        {
            winlose_txt.color = Color.red;
            winlose_txt.text = "ÆÐ¹è....";
        }
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        book.SetActive(true);
        Ele_pool.SetActive(true);
    }
}
