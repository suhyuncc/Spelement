using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon_storm : MonoBehaviour
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
        //Debug.Log(sfx.clip.length);


        anim.SetBool("Hit", true);
        yield return null;
    }
}
