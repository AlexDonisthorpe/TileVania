using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip pickupSound;
    [SerializeField] int pointsValue = 100;
    bool addedToScore = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!addedToScore)
        {
            addedToScore = true;
            AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
            FindObjectOfType<GameSession>().AddToScore(pointsValue);
            Destroy(gameObject);
        }

    }
}
