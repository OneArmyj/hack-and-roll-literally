using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    public void SpawnPlayer(GameObject gameObject, System.Action<GameObject> action, Transform endPoint1, Transform endPoint2, float offset)
    {
        StartCoroutine(Respawn(gameObject, action, endPoint1, endPoint2, offset));
    }

    IEnumerator Respawn(GameObject gameObject, System.Action<GameObject> action, Transform endPoint1, Transform endPoint2, float offset)
    {
        yield return new WaitForSeconds(5);
        transform.position = new Vector3(Camera.main.transform.position.x + offset, transform.position.y, transform.position.z);
        if (transform.position.x > endPoint1.position.x)
        {
            transform.position += new Vector3(endPoint1.position.x - transform.position.x, 0, 0);
        } else if (transform.position.x < endPoint2.position.x)
        {
            transform.position += new Vector3(endPoint2.position.x - transform.position.x, 0, 0);
        }
        GameObject newPlayer = Instantiate(gameObject, transform.position, Quaternion.identity);
        action(newPlayer);
    }
}
