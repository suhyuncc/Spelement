using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_explainPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject page1;
    [SerializeField]
    private GameObject page2;

    public void Go_page1()
    {
        page1.SetActive(true);
        page2.SetActive(false);
    }

    public void Go_page2()
    {
        page1.SetActive(false);
        page2.SetActive(true);
    }
}
