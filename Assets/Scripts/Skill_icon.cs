using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Skill_icon : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
{
    private Vector3 mousePosition;
    private Vector3 landPosition;
    [SerializeField]
    private Vector3 InitPosition;
    [SerializeField]
    private int spell_id;
    [SerializeField]
    private GameObject My_area;

    public bool Onspell;

    private GameObject spell;

    private RectTransform rect;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D collider;

    [SerializeField]
    private GameObject discrip_box;

    private bool mouseOn;

    private void Awake()
    {
        Onspell = false;
        rect = this.GetComponent<RectTransform>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        collider = this.GetComponent<BoxCollider2D>();
        InitPosition = rect.position;
        landPosition = InitPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOn = true;
        discrip_box.SetActive(true);
        Debug.Log("on");
    }



    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOn = false;
        discrip_box.SetActive(false);
        Debug.Log("off");
        Debug.Log(eventData.position);
        Debug.Log(Input.mousePosition);
    }

    private void Update()
    {
        if (mouseOn)
        {
            discrip_box.transform.position = Input.mousePosition + new Vector3(10f, 10f, 0);
        }

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
        My_area.SetActive(false);
    }

    private void OnMouseUp()
    {
        transform.position = InitPosition;
        My_area.SetActive(true);
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
        My_area.SetActive(false);
    }

    public void Active()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        collider.enabled = true;
        My_area.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("dd");
    }
}
