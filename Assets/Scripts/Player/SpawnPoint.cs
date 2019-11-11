using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject player;


    public void SpawnPLayerAtSpawnPoint()
    {
        player.GetComponent<Transform>().position = GetComponent<Transform>().position;
    }


    public void SetNewSpawnPoint(Transform newPos)
    {
        player.SetActive(false);
        GetComponent<Transform>().position = newPos.position;
        player.SetActive(true);
    }
}
