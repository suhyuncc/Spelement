using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class sys_txt : MonoBehaviour
{
    public bool is_16;

    private Text demage;

    private Color orign_Co;
    private Vector3 orign_Vec;

    private void Awake()
    {
        demage = this.GetComponent<Text>();
        orign_Co = demage.color;
        orign_Vec = this.transform.position;
    }

    private void OnEnable()
    {
        demage.color = orign_Co;
        this.transform.position = orign_Vec;
        if (is_16)
        {
            this.transform.position = this.transform.position - new Vector3(0, 50, 0);
        }
        StartCoroutine("Get");
    }

    private void OnDisable()
    {
        is_16 = false;
    }

    IEnumerator Get()
    {
        float F_time = 1f;
        float time = 0f;

        Color alpha = demage.color;
        Vector3 up = this.transform.position;

        float init_y = up.y;

        while (alpha.a > 0)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, 2*time/3);
            up.y = Mathf.Lerp(init_y, init_y + 50, 0.02f);
            demage.color = alpha;
            this.transform.position = up;
            yield return null;
        }
        this.gameObject.SetActive(false);
    }
}