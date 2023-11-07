using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int speed;
    public float during_time;

    private float init_x;
    private float init_y;

    private void OnEnable()
    {
        init_x = transform.position.x;
        init_y = transform.position.y;
        Rigidbody2D rigid = this.GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(1.15f, 0.57f) * speed, ForceMode2D.Impulse);
        StartCoroutine("disapear");
    }

    private void OnDisable()
    {
        transform.position = new Vector2(init_x,init_y);

    }

    IEnumerator disapear()
    {
        yield return new WaitForSeconds(during_time);
        this.gameObject.SetActive(false);
    }
}
