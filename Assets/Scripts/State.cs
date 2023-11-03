using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class State : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("mouse over");
        Debug.Log(eventData.position);


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("mouse Exit");
    }
}
