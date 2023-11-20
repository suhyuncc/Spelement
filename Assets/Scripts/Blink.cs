using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer color;

    private Vector3 orign_Vec;

    private void Awake()
    {
    }

    private void OnEnable()
    {
        StartCoroutine("Get");
    }

    IEnumerator Get()
    {
        float F_time = 1f;
        float time = 0f;

        Color alpha = color.color;
        alpha.a = 0f;
        color.color = alpha;

        while (alpha.a < 1)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            color.color = alpha;
            yield return null;
        }
        this.gameObject.SetActive(false);
    }
}
