using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EleManager : MonoBehaviour
{
    public static EleManager instance;

    [SerializeField]
    private Transform[] Points;
    [SerializeField]
    private GameObject[] Fires;
    [SerializeField]
    private GameObject[] Waters;
    [SerializeField]
    private GameObject[] Airs;
    [SerializeField]
    private GameObject[] Earths;
    [SerializeField]
    private GameObject[] Nulls;

    public int player_lv;

    private Transform Initpos;

    void Start()
    {
        Initpos = this.transform;
    }

    // Update is called once per frame
    void Awake()
    {
        instance = this;
    }

    public void Reroll()
    {
        switch (player_lv)
        {
            case 1:
                Null();
                break;

            case 2:
                Air();
                break;

            case 3:
                Earth();
                break;

            case 4:
                Water();
                break;

            case 5:
                Fire();
                break;
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

        for (int i = 0; i < Earths.Length; i++)
        {
            Earths[i].transform.position = Initpos.position;
            Earths[i].SetActive(false);
        }

        for (int i = 0; i < Airs.Length; i++)
        {
            Airs[i].transform.position = Initpos.position;
            Airs[i].SetActive(false);
        }

        for (int i = 0; i < Nulls.Length; i++)
        {
            Nulls[i].transform.position = Initpos.position;
            Nulls[i].SetActive(false);
        }
    }

    private void Null() {
        PoolReset();
        for (int i = 0; i < player_lv + 2; i++)
        {
            Nulls[i].transform.position = Points[i].position;
            Nulls[i].SetActive(true);
        }
    }

    private void Earth()
    {
        int null_index = 0;
        int air_index = 0;
        int earth_index = 0;

        PoolReset();
        for (int i = 0; i < player_lv + 2; i++)
        {
            float random = Random.Range(0.0f, 1.0f);

            if (random < 0.2f)
            {
                Earths[earth_index].transform.position = Points[i].position;
                Earths[earth_index].SetActive(true);
                earth_index++;
            }
            else if (random >= 0.2f && random < 0.4f)
            {
                Airs[air_index].transform.position = Points[i].position;
                Airs[air_index].SetActive(true);
                air_index++;
            }
            else
            {
                Nulls[null_index].transform.position = Points[i].position;
                Nulls[null_index].SetActive(true);
                null_index++;
            }
        }
    }
    private void Air()
    {
        int null_index = 0;
        int air_index = 0;

        PoolReset();
        for (int i = 0; i < player_lv + 2; i++)
        {
            float random = Random.Range(0.0f, 1.0f);

            if (random < 0.2f)
            {
                Airs[air_index].transform.position = Points[i].position;
                Airs[air_index].SetActive(true);
                air_index++;
            }
            else
            {
                Nulls[null_index].transform.position = Points[i].position;
                Nulls[null_index].SetActive(true);
                null_index++;
            }
        }
    }
    private void Water()
    {
        int null_index = 0;
        int earth_index = 0;
        int air_index = 0;
        int water_index = 0;

        PoolReset();
        for (int i = 0; i < player_lv + 2; i++)
        {
            float random = Random.Range(0.0f, 1.0f);

            if (random < 0.2f)
            {
                Earths[earth_index].transform.position = Points[i].position;
                Earths[earth_index].SetActive(true);
                earth_index++;
            }
            else if (random >= 0.2f && random < 0.4f)
            {
                Airs[air_index].transform.position = Points[i].position;
                Airs[air_index].SetActive(true);
                air_index++;
            }
            else if (random >= 0.4f && random < 0.6f)
            {
                Waters[water_index].transform.position = Points[i].position;
                Waters[water_index].SetActive(true);
                water_index++;
            }
            else
            {
                Nulls[null_index].transform.position = Points[i].position;
                Nulls[null_index].SetActive(true);
                null_index++;
            }
        }
    }
    private void Fire()
    {
        int null_index = 0;
        int earth_index = 0;
        int air_index = 0;
        int water_index = 0;
        int fire_index = 0;

        PoolReset();
        for (int i = 0; i < player_lv + 2; i++)
        {
            float random = Random.Range(0.0f, 1.0f);

            if (random < 0.2f)
            {
                Earths[earth_index].transform.position = Points[i].position;
                Earths[earth_index].SetActive(true);
                earth_index++;
            }
            else if (random >= 0.2f && random < 0.4f)
            {
                Airs[air_index].transform.position = Points[i].position;
                Airs[air_index].SetActive(true);
                air_index++;
            }
            else if (random >= 0.4f && random < 0.6f)
            {
                Waters[water_index].transform.position = Points[i].position;
                Waters[water_index].SetActive(true);
                water_index++;
            }
            else if (random >= 0.6f && random < 0.8f)
            {
                Fires[fire_index].transform.position = Points[i].position;
                Fires[fire_index].SetActive(true);
                fire_index++;
            }
            else
            {
                Nulls[null_index].transform.position = Points[i].position;
                Nulls[null_index].SetActive(true);
                null_index++;
            }
        }
    }

    public void All_earth()
    {
        PoolReset();
        for (int i = 0; i < player_lv + 2; i++)
        {
            Earths[i].transform.position = Points[i].position;
            Earths[i].SetActive(true);
        }
    }

    public void All_air()
    {
        PoolReset();
        for (int i = 0; i < player_lv + 2; i++)
        {
            Airs[i].transform.position = Points[i].position;
            Airs[i].SetActive(true);
        }
    }

    public void All_water()
    {
        PoolReset();
        for (int i = 0; i < player_lv + 2; i++)
        {
            Waters[i].transform.position = Points[i].position;
            Waters[i].SetActive(true);
        }
    }

    public void All_fire()
    {
        PoolReset();
        for (int i = 0; i < player_lv + 2; i++)
        {
            Fires[i].transform.position = Points[i].position;
            Fires[i].SetActive(true);
        }
    }
}
