using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class fortune_icon : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField]
    private GameObject discrip_box;
    [SerializeField]
    private int cur_fortune_index;
    private bool mouseOn;


    public void OnPointerEnter(PointerEventData eventData)
    {
        fortune.instance.Show_F_index = cur_fortune_index;
        mouseOn = true;
        discrip_box.SetActive(true);

    }



    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOn = false;
        discrip_box.SetActive(false);
    }

    private void Update()
    {
        if (mouseOn)
        {
            discrip_box.transform.position = Input.mousePosition + new Vector3(20f, 20f, 0);
        }

    }

    private void OnDisable()
    {
        discrip_box.SetActive(false);
    }
}
