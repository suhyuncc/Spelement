using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_icon : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector3 landPosition;
    [SerializeField]
    private Vector3 InitPosition;
    [SerializeField]
    private int spell_id;

    public bool Onspell;

    private GameObject spell;

    private RectTransform rect;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D collider;

    private void Awake()
    {
        Onspell = false;
        rect = this.GetComponent<RectTransform>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        collider = this.GetComponent<BoxCollider2D>();
        InitPosition = rect.position;
        landPosition = InitPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 GetmousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetmousePos();
        //GameManager.Instance.Target = this.gameObject;
    }

    private void OnMouseDrag()
    {

        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);

    }

    private void OnMouseUp()
    {
        transform.position = InitPosition;
        if (Onspell)
        {
            anActive();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        landPosition = InitPosition;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spell"))
        {
            Onspell = true;
            spell = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Onspell = false;
    }

    public void anActive()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        collider.enabled = false;
    }

    public void Active()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        collider.enabled = true;
    }
}
