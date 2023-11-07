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

    [SerializeField]
    private Animation animation;

    private void OnEnable()
    {
        StartCoroutine("Hit");
    }

    IEnumerator Hit()
    {
        Debug.Log(anim.runtimeAnimatorController.animationClips[0].length);
        
        anim.SetBool("Hit", true);
        yield return new WaitForSeconds(during_time);

        this.gameObject.SetActive(false);
    }
}
