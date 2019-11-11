using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    // Start is called before the first frame update
    Animation TrapDoorAnimation;
    void Start() {
        TrapDoorAnimation = GetComponentInChildren<Animation>();
    }

    public void openDoor(){
        TrapDoorAnimation.Play();     
    }



}
