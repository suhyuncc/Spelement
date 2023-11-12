using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCustom_Manager : MonoBehaviour
{
    public static SpellCustom_Manager instance;

    [SerializeField]
    private Button next;
    [SerializeField]
    private Button back;

    [SerializeField]
    private Skill_icon[] icons;

    public int stage_lv;
    public int[] page_list;
    private int page_index;

    private void Awake()
    {
        if(page_list == null)
        {
            if(stage_lv > 10)
            {
                page_list = new int[12];
            }
            else
            {
                page_list = new int[stage_lv + 2];
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        page_index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch(page_index)
        {
            case 0:
                back.gameObject.SetActive(false);
                next.gameObject.SetActive(true);
                break; 
            case 1:
                back.gameObject.SetActive(true);
                next.gameObject.SetActive(true);
                break;
            case 2:
                back.gameObject.SetActive(true);
                next.gameObject.SetActive(false);
                break;
        }
    }

    public void nextBtn()
    {
        page_index++;
    }

    public void backBtn()
    {
        page_index--;
    }
}
