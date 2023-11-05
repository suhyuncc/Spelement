using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManagement : MonoBehaviour
{
    public int[] counts;

    [SerializeField]
    private Text Fire_text;
    [SerializeField]
    private Text Water_text;
    [SerializeField]
    private Text Wind_text;

    void Awake()
    {
        //√ ±‚»≠
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
        Water_text.text = counts[1].ToString();
        Wind_text.text = counts[2].ToString();

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
        for (int i = 0; i < counts.Length; i++)
        {
            if (counts[i] != 0)
            {
                counts[i]--;
            }
        }
    }
}
