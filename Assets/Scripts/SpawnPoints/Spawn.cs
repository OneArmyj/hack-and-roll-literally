using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    void SpawnPlayer(GameObject gameObject)
    {
        StartCoroutine(Respawn(gameObject));
    }

    IEnumerator Respawn(GameObject gameObject)
    {
        yield return new WaitForSeconds(5);
        Instantiate(gameObject, transform.position, Quaternion.identity);
    }
}
