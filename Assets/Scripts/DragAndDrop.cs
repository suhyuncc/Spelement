using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Element
{
    Null = 0,
    Air = 1,
    Earth = 2,
    Water = 3,
    Fire = 4,
}

public class DragAndDrop : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector3 landPosition;
    private Vector3 InitPosition;

    public float count;

    public bool Onspell;

    private GameObject spell;
    public Element element;

    private void OnEnable()
    {
        InitPosition = transform.position;
        landPosition = InitPosition;
    }

    private void OnDisable()
    {
        if(element == Element.Air)
        {
            spell.gameObject.GetComponent<SpellAction>().getair();
        }
    }

    private void Awake()
    {
        

        Onspell = false;
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
        if(Onspell)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            transform.position = InitPosition;
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

    public void ReturnPos()
    {
        transform.rotation = Quaternion.identity;
        transform.position = InitPosition;
    }
}
