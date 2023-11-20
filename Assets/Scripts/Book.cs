using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour
{

    [SerializeField]
    private Animator book_anim;
    [SerializeField]
    private GameObject[] book_inners;
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
        for(int i = 0; i < book_inners.Length; i++)
        {
            book_inners[i].SetActive(false);
        }
        yield return new WaitForSeconds(0.75f);
        
        book_anim.SetBool("Open", false);
        for (int i = 0; i < book_inners.Length; i++)
        {
            book_inners[i].SetActive(true);
        }
        book.sprite = defult;
        StopCoroutine("OpenBook");
    }
}
