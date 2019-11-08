using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWall : MonoBehaviour, ItemInterface
{



    void ItemInterface.ItemEnteractedWith()
    {
        Debug.Log("WORKS");
    }
}
