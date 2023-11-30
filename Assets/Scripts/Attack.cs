using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int direct;
    public int speed;
    public float during_time;

    private Transform init_trans;

    private void OnEnable()
    {
        init_trans = this.transform;
        Rigidbody2D rigid = this.GetComponent<Rigidbody2D>();

        if(direct == -1)
        {
            this.transform.eulerAngles = new Vector3(0, 180, -1 *transform.rotation.eulerAngles.z);
        }

        rigid.AddForce(new Vector2(1.15f, 0.57f) * speed * direct, ForceMode2D.Impulse);
        StartCoroutine("disapear");
    }

    private void OnDisable()
    {
        transform.position = new Vector2(init_trans.position.x, init_trans.position.y);

        if (direct == -1)
        {
            this.transform.eulerAngles = new Vector3(0, 0, -1 * transform.rotation.eulerAngles.z);
        }
    }

    IEnumerator disapear()
    {
        yield return new WaitForSeconds(during_time);
        this.gameObject.SetActive(false);
    }
}
