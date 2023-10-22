using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Text Name;
    [SerializeField]
    private Text description;
    [SerializeField]
    private Image spell;
    [SerializeField]
    private GameObject cost;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("mouse over");
        description.gameObject.SetActive(true);

        Name.gameObject.SetActive(false);
        spell.gameObject.SetActive(false);
        cost.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("mouse Exit");

        description.gameObject.SetActive(false);

        Name.gameObject.SetActive(true);
        spell.gameObject.SetActive(true);
        cost.SetActive(true);
    }
}
