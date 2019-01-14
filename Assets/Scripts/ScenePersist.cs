using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour {

    int startingSceneIndex;

    private void Awake()
    {
        int numbScenePersist = FindObjectsOfType<ScenePersist>().Length;
        if (numbScenePersist > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Use this for initialization
    //Get scene index at which you start
    void Start () {
        startingSceneIndex = SceneManager.GetActiveScene().buildIndex;
                                         
	}
	
	// Update is called once per frame
    //get the current scene index --> as long as it is not the same, destroy the current gameObject, which implies reset progress
    //of the level so far
	void Update () {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex != startingSceneIndex) 
        {
            Destroy(gameObject);
        }
	}
}
