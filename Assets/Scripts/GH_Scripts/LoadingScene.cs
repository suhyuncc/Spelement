using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//gh
public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    private Image filledArea;
    [SerializeField]
    private string sceneName;


    public void GetSceneName(string _sceneName)
    {
        sceneName = _sceneName;
    }
    private void Start()
    {
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        float timer = 0f;

        while(!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if(op.progress < 0.9f)
            {
                filledArea.fillAmount = Mathf.Lerp(filledArea.fillAmount, op.progress, timer);
                if(filledArea.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                filledArea.fillAmount = Mathf.Lerp(filledArea.fillAmount, 1f, timer);
                if(filledArea.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
