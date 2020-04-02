using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] GameObject fireworkPrefab;
    [SerializeField] float exitWait = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine("LoadNextLevel");
    }

    IEnumerator LoadNextLevel()
    {
        Instantiate(fireworkPrefab, FindObjectOfType<Player>().transform);
        yield return new WaitForSeconds(exitWait);

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex+1);
    }

}
