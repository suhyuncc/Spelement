using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneButtonClicked : MonoBehaviour
{
    [SerializeField]
    private GameObject creditImg;
    public void CreditButton()
    {
        creditImg.SetActive(true);
    }

    public void Xbutton()
    {
        creditImg.SetActive(false);
    }
}
