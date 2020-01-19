using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player1Prefab;

    [SerializeField]
    private GameObject player2Prefab;

    [SerializeField]
    private Transform startPoint1;

    [SerializeField]
    private Transform startPoint2;

    [HideInInspector]
    public Transform endPoint1;

    [HideInInspector]
    public Transform endPoint2;

    [HideInInspector]
    public GameObject player1;

    [HideInInspector]
    public GameObject player2;

    [SerializeField]
    private Transform spawnPoint1;

    [SerializeField]
    private Transform spawnPoint2;

    public float countdownDuration;



    // Start is called before the first frame update
    void Start()
    {
        InitialisePlayers(); //players movement are disabled

        // Start countdown
        //StartCountdown(); // not done

        // When countdown end enable player movement
        //StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for game end conditions
    }

    void InitialisePlayers()
    {
        player1 = Instantiate(player1Prefab, startPoint1.position, Quaternion.identity);
        player2 = Instantiate(player2Prefab, startPoint2.position, Quaternion.identity);
    }

    void StartCountdown()
    {
        //start animation to count down
        //wait for animation to end
    }

    void StartGame()
    {
        player1.GetComponent<PlayerMovement>().enabled = true;
        player2.GetComponent<PlayerMovement>().enabled = true;
    }

    public void EndGame(int id) 
    {
        if (id == 1)
        {
            //start winner1 animation
            Debug.Log("Player1 won!");
        } else if (id == 2)
        {
            //start winner2 animation
            Debug.Log("Player2 won!");
        } else
        {
            throw new System.Exception("Error, neither player1 or 2 won.");
        }

        StartCoroutine(ExitGame());
    }

    IEnumerator ExitGame()
    {
        yield return new WaitForSeconds(3);

        // change scene back to before game start
        SceneManager.LoadScene("Start Menu");
    }

    public void RespawnPlayer(int id)
    {
        if (id == 1)
        {
            spawnPoint1.GetComponent<Spawn>().SpawnPlayer(player1Prefab, obj => player1 = obj, endPoint1, endPoint2, -22);
        } else if (id == 2)
        {
            spawnPoint2.GetComponent<Spawn>().SpawnPlayer(player2Prefab, obj => player2 = obj, endPoint1, endPoint2, 22);
        } else
        {
            Debug.Log("Respawn error.");
        }
    }

}
