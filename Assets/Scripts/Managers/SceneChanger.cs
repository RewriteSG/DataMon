using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    float delay = 0;
    public string WhichScene = "Scene Name";

    public void ChangeScene()
    {
        SceneManager.LoadScene(WhichScene);
    }

    public void ChangeSceneDelay(string toScene)
    {
        StartCoroutine(ChangeSceneOnDelay(delay,toScene));
    }
    public static void ChangeScene(string toScene)
    {
        SceneManager.LoadScene(toScene);

    }
    IEnumerator ChangeSceneOnDelay(float delay, string toScene)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(toScene);
    }
}
