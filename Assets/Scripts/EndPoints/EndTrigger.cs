using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    private GameObject gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    public int id = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            // Some function in game manager to end the game.
            gameManager.GetComponent<GameManager>().EndGame(id);
        }
    }
}
