using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hit_anim : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    private void OnEnable()
    {
        StartCoroutine("Hit");
    }

    IEnumerator Hit()
    {
        anim.SetBool("Hit", true);
        yield return new WaitForSeconds(0.2f);

        this.gameObject.SetActive(false);
    }
}
