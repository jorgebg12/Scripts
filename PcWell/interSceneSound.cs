using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class interSceneSound : MonoBehaviour
{
    private interSceneSound instance;
    public interSceneSound Instance 
    {
        get 
        {
            return instance;
        }
    }
    private string sceneName;
    private string prevSceneName;

    private void Awake() {
        if (FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        }
        sceneName = SceneManager.GetActiveScene().name;
        prevSceneName = sceneName;
    }
    private void Update() 
    {
        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName != "PruebaCosas") {
            DontDestroyOnLoad(gameObject);
            prevSceneName = sceneName;
        }
        else {
            Destroy(gameObject);
        } 
    }
}
