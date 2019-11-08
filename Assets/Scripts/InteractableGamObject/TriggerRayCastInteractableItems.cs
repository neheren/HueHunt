using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRayCastInteractableItems : MonoBehaviour
{
  

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RaycastToFindObjects>() != null)
            other.GetComponent<RaycastToFindObjects>().findObject = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<RaycastToFindObjects>() != null)
            other.GetComponent<RaycastToFindObjects>().findObject = false;
    }
}
