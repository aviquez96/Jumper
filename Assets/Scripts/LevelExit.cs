using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class LevelExit : MonoBehaviour {

    [SerializeField] float LevelLoadDelay = 2f;
    [SerializeField] float LevelExitSlowMoFactor = 0.2f;

    //Function whenever an object collides --> more built-in functions from the SceneManagement namespace that load the next 
    //level based on the current index, which is specified in the build&run settings
    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(LoadNextLevel());        
    }

    //Interface 
    IEnumerator LoadNextLevel()
    {
        //Adds the slow mo effect when triggering an exit gameobject
        Time.timeScale = LevelExitSlowMoFactor;
        yield return new WaitForSecondsRealtime(LevelLoadDelay);
        Time.timeScale = 1f;

        //Gets the current scene index
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
