using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StateManagement : MonoBehaviour
{
    public int[] counts;

    [SerializeField]
    private Text Fire_text;
    [SerializeField]
    private Text Sturn_text;
    [SerializeField]
    private Text WaterRecovery_text;
    [SerializeField]
    private Text Water_text;
    [SerializeField]
    private Text Shield_text;
    [SerializeField]
    private Text Fireskin_text;
    [SerializeField]
    private Text Wind_text;
    [SerializeField]
    private Text Stoneskin_text;
    [SerializeField]
    private Text Addwind_text;

    

    void Awake()
    {
        //초기화
        counts = new int[this.transform.childCount];

        for (int i = 0; i < counts.Length; i++)
        {
            counts[i] = 0;
        }
    }



    // Update is called once per frame
    void Update()
    {
        Fire_text.text = counts[0].ToString();
        Sturn_text.text = counts[1].ToString();
        WaterRecovery_text.text = counts[2].ToString();
        Water_text.text = counts[3].ToString();
        Shield_text.text = counts[4].ToString();
        Fireskin_text.text = counts[5].ToString();
        Wind_text.text = counts[6].ToString();
        Stoneskin_text.text = counts[7].ToString();
        Addwind_text.text = counts[8].ToString();

        for (int i = 0; i < counts.Length; i++)
        {
            if (counts[i] == 0)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void reduceState()
    {
        //줄어드는 상태이상은 총 3개
        for (int i = 0; i < 2; i++)
        {
            if (counts[i] != 0)
            {
                SkillManager.instance.StateAct(i);
                counts[i]--;
            }
        }
    }
}
