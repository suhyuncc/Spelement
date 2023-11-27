using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nextpage_anim : MonoBehaviour
{
    [SerializeField]
    private GameObject book;
    [SerializeField]
    private float during_time;



    private void OnEnable()
    {
        StartCoroutine("Hit");
    }

    private void OnDisable()
    {
        book.SetActive(true);
    }

    IEnumerator Hit()
    {

        yield return new WaitForSeconds(during_time);

        this.gameObject.SetActive(false);
    }
}
