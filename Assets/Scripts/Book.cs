using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour
{

    [SerializeField]
    private Animator book_anim;
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private GameObject[] spellPages;
    [SerializeField]
    private SpriteRenderer book;
    [SerializeField]
    private Sprite defult;

    private void OnEnable()
    {
        StartCoroutine("OpenBook");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator OpenBook()
    {
        book_anim.SetBool("Open", true);
        canvas.SetActive(false);
        for (int i = 0; i < spellPages.Length; i++)
        {
            spellPages[i].SetActive(false);
        }
        yield return new WaitForSeconds(0.75f);
        
        book_anim.SetBool("Open", false);
        canvas.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            if (i < (BattleManager.instance.page_list.Length - (4 * BattleManager.instance.page_index)))
            {
                spellPages[i].gameObject.SetActive(true);
            }
            else
            {
                spellPages[i].gameObject.SetActive(false);
            }

        }
        book.sprite = defult;
        StopCoroutine("OpenBook");
    }
}
