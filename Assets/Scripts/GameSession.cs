using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] float deathDelayTimer = 1f;

    //Scores
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;

    //Text fields
    [SerializeField] Text livesText;
    [SerializeField] Text scoreText;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

    public void ProcessPlayerDeath()
    {
        StartCoroutine("HandleDeath");
    }

    IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(deathDelayTimer);
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameProgress();
        }
    }

    private void TakeLife()
    {
        playerLives--;
        livesText.text = playerLives.ToString();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetGameProgress()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
