using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] Points;
    [SerializeField]
    private GameObject[] Fires;
    [SerializeField]
    private GameObject[] Waters;

    public int player_lv;

    private Transform Initpos;

    void Awake()
    {
        Initpos = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reroll()
    {
        int fire_index = 0;
        int water_index = 0;

        PoolReset();
        for(int i = 0; i < player_lv + 2; i++)
        {
            float random = Random.Range(0.0f, 1.0f);
            
            if(random < 0.5f) 
            {
                Fires[fire_index].transform.position = Points[i].position;
                Fires[fire_index].SetActive(true);
                fire_index++;
            }
            else
            {
                Waters[water_index].transform.position = Points[i].position;
                Waters[water_index].SetActive(true);
                water_index++;
            }
        }
    }

    private void PoolReset()
    {
        for(int i = 0; i < Fires.Length; i++)
        {
            Fires[i].transform.position = Initpos.position;
            Fires[i].SetActive(false);
        }
        for (int i = 0; i < Waters.Length; i++)
        {
            Waters[i].transform.position = Initpos.position;
            Waters[i].SetActive(false);
        }
    }
}
