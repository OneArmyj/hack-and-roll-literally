using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public void SpawnPlayer(GameObject gameObject, System.Action<GameObject> action)
    {
        StartCoroutine(Respawn(gameObject, action));
    }

    IEnumerator Respawn(GameObject gameObject, System.Action<GameObject> action)
    {
        yield return new WaitForSeconds(5);
        GameObject player = Instantiate(gameObject, transform.position, Quaternion.identity);
        action(player);
    }
}
