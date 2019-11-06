using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpots : MonoBehaviour
{
    public GameObject player;
    DetectionRadius detectionPlayerScript;

    private void Start()
    {
        detectionPlayerScript = player.GetComponent<DetectionRadius>();
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log(detectionPlayerScript == null);
        detectionPlayerScript.Hiding(true);
    }

    private void OnTriggerExit(Collider other)
    {
        detectionPlayerScript.Hiding(false);
    }
}
