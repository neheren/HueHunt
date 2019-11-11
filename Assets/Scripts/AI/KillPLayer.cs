using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPLayer : MonoBehaviour
{
    public SpawnPoint spawnPoint;



    public void KillThePlayer()
    {
        spawnPoint.SpawnPLayerAtSpawnPoint();
    }
}
