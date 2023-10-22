using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector3 landPosition;
    private Vector3 InitPosition;

    public float count;

    public bool Onspell;

    private void OnEnable()
    {
        InitPosition = transform.position;
        landPosition = InitPosition;
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
