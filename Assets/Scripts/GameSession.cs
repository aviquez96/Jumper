using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//A game session is an object that keeps track of ongoing data that doesn't live on any element
public class GameSession : MonoBehaviour {

    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;

    [SerializeField] Text scoreText;
    [SerializeField] Text livesText;

    //Singleton --> if there is already one that exists, it destroys itself 
    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions > 1) 
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        //Display whatever we have as players lives to lives text; same for score
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
	}
	
    //Gets parameter invocation from another script (coin pickup)
    public void AddToScore (int pointsToAdd) 
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

    //
	public void ProcessPlayerDeath() 
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    //Send the game to the begining (index 0)
    private void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    private void TakeLife()
    {
        //Takes player lives, displays it, and returns the player to the current scene
        playerLives--;
        livesText.text = playerLives.ToString();
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex) ;
    }
}
