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
    [SerializeField]
    private Vector3 InitPosition;


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
        if (spell != null) {
            switch (element)
            {
                case Element.Air:
                    spell.gameObject.GetComponent<SpellAction>().getair();
                    spell = null;
                    break; 

                case Element.Earth:
                    spell.gameObject.GetComponent<SpellAction>().getearth();
                    spell = null;
                    break;

                case Element.Water:
                    spell.gameObject.GetComponent<SpellAction>().getwater();
                    spell = null;
                    break;

                case Element.Fire:

                    spell.gameObject.GetComponent<SpellAction>().getfire();
                    spell = null;
                    break;

                case Element.Null:

                    spell.gameObject.GetComponent<SpellAction>().getnull();
                    spell = null;
                    break;
            }
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
    }

    private void OnMouseDrag()
    {

        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);

    }

    private void OnMouseUp()
    {
        if(Onspell && !spell.gameObject.GetComponent<SpellAction>().isDone)
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
