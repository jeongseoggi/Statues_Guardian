using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Slider progressBar;
    private float time;



    void Start()
    {
        UIManager.Instance.gameObject.SetActive(false);
        StartCoroutine(Loading());

    }

    IEnumerator Loading()
    {
        AsyncOperation oper = SceneManager.LoadSceneAsync(GameManager.Instance.nextSceneName);
        oper.allowSceneActivation = false;

        while(!oper.isDone)
        {
            time += Time.deltaTime + Time.deltaTime;

            progressBar.value = time / 10f;

            if(progressBar.value == 1)
            {
                oper.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
