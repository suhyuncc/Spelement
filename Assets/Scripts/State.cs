using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class State : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField]
    private GameObject discrip_box;

    private bool mouseOn;


    public void OnPointerEnter(PointerEventData eventData)
    {
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
        if (mouseOn) {
            discrip_box.transform.position = Input.mousePosition + new Vector3(20f,20f,0);
        }
        
    }

    private void OnDisable()
    {
        discrip_box.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData.selectedObject.name);
    }
}
