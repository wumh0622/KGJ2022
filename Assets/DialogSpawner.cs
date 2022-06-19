using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSpawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public float spawnRadius;

    private void OnDrawGizmos()
    {
        foreach (var item in spawnPoint)
        {
            Gizmos.DrawWireSphere(item.transform.position, spawnRadius);
        }
    }

    public void StartSpawn()
    {

    }
}
