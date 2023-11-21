using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//gh
public class EndTriangleBlinking : MonoBehaviour
{
    private Color toChangeAlpha;//���İ� �ٲٴµ� ����� ����
    private void OnEnable()//Ȱ��ȭ �� ��
    {
        toChangeAlpha = this.gameObject.GetComponent<RawImage>().color;
        StartCoroutine("Blinking");//�ڷ�ƾ���� ���� ����
    }

    IEnumerator Blinking()
    {
        while (true)
        {
            if(toChangeAlpha.a == 0f)
            {
                toChangeAlpha.a = 1f;
                this.gameObject.GetComponent<RawImage>().color = toChangeAlpha;
            }
            else
            {
                toChangeAlpha.a = 0f;
                this.gameObject.GetComponent<RawImage>().color = toChangeAlpha;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
