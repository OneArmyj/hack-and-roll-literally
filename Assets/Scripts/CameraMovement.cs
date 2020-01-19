using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameManager gm;

    // Update is called once per frame
    void Update()
    {
        Transform player1 = gm.player1 == null ? null : gm.player1.transform.Find("Ball");
        Transform player2 = gm.player2 == null ? null : gm.player2.transform.Find("Ball");
        float size;
        float finalX;

        if (player1 == null && player2 == null)
        {
            return;
        }
        else if (player1 == null)
        {
            size = 5;
            finalX = player2.position.x - 4;
        }
        else if (player2 == null)
        {
            size = 5;
            finalX = player1.position.x + 4;
        }
        else
        {
            size = (player2.position.x - player1.position.x + 2) / 2;
            finalX = (player1.position.x + player2.position.x) / 2;
        }

        Debug.Log("FinalX: " + finalX + " and currX: " + Camera.main.transform.position.x);
        Camera.main.orthographicSize += (size - Camera.main.orthographicSize) * Time.deltaTime;
        Camera.main.transform.position += new Vector3(finalX - Camera.main.transform.position.x, 0, 0) * Time.deltaTime;
        if (Camera.main.orthographicSize > 10)
            Camera.main.orthographicSize = 10;
        else if (Camera.main.orthographicSize < 5)
            Camera.main.orthographicSize = 5;
    }
}
