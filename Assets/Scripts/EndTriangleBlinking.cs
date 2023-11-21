using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//gh
public class EndTriangleBlinking : MonoBehaviour
{
    private Color toChangeAlpha;//알파값 바꾸는데 사용할 변수
    private void OnEnable()//활성화 될 때
    {
        toChangeAlpha = this.gameObject.GetComponent<RawImage>().color;
        StartCoroutine("Blinking");//코루틴으로 깜빡 구현
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
