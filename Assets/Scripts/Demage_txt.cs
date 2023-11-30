using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demage_txt : MonoBehaviour
{
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
        StartCoroutine("Get");
    }

    IEnumerator Get()
    {
        float F_time = 0.5f;
        float time = 0f;

        Color alpha = demage.color;
        Vector3 up = this.transform.position;

        float init_y = up.y;

        while (alpha.a > 0)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            up.y = Mathf.Lerp(init_y, init_y+50, time);
            demage.color = alpha;
            this.transform.position = up;
            yield return null;
        }
        this.gameObject.SetActive(false);
    }
}
