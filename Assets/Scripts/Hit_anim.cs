using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hit_anim : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private float during_time;

    private void OnEnable()
    {
        StartCoroutine("Hit");
    }

    IEnumerator Hit()
    {
        anim.SetBool("Hit", true);
        yield return new WaitForSeconds(during_time);

        this.gameObject.SetActive(false);
    }
}