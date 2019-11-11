using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTrapDoor : MonoBehaviour, ItemInterface
{
    public GameObject trapdoor;
    TrapDoor td;

      void ItemInterface.ItemEnteractedWith()
    {
        td.openDoor();
    }

    void Start()
    {
        td = trapdoor.GetComponent<TrapDoor>();
    }

}
