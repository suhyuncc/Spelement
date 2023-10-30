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
    private GameObject spell;
    [SerializeField]
    private GameObject[] costs;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("mouse over");
        description.gameObject.SetActive(true);

        Name.gameObject.SetActive(false);
        spell.SetActive(false);

        for (int i = 0; i < costs.Length; i++) {
            costs[i].SetActive(false);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("mouse Exit");

        description.gameObject.SetActive(false);

        Name.gameObject.SetActive(true);
        spell.SetActive(true);
        for (int i = 0; i < costs.Length; i++)
        {
            costs[i].SetActive(true);
        }
    }

    public void getair() {
        int count = 0;
        for(int i = 0; i < costs.Length; i++)
        {
            if (costs[i].GetComponent<SpriteRenderer>().sprite.name == "원소_바람")
            {
                count++;
            }
        }
        Debug.Log($"{count}개 있습니다");
    }
}
