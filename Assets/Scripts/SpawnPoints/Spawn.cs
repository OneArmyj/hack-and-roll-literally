using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    void SpawnPlayer(GameObject gameObject)
    {
        Instantiate(gameObject, transform);
    }
}
