using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Show_panel : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField]
    private GameObject discrip_box;
    [SerializeField]
    private int spell_id;

    private bool mouseOn;

    public void OnPointerEnter(PointerEventData eventData)
    {
        SpellCustom_Manager.instance.cur_id = spell_id;
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
            discrip_box.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition + new Vector3(20f, 20f, 0)
                + new Vector3(-960f, -540f, 0);
        }

    }

    private void OnDisable()
    {
        discrip_box.SetActive(false);
    }
}
