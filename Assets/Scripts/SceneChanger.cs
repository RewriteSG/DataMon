using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private string WhichScene = "Scene Name";

    public void ChangeScene()
    {
        SceneManager.LoadScene(WhichScene);
    }
}
