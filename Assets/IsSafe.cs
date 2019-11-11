using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class IsSafe : MonoBehaviour
{
    // Start is called before the first frame update
    public bool PlayerIsSafe = false;

    void Start() {
        // GetComponent<Collider>().
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "SafeZone"){
            PlayerIsSafe = true;
            print("Is in SafeZone");
        }
    }
    void OnTriggerExit(Collider other){
        if(other.tag == "SafeZone"){
            PlayerIsSafe = false;
            print("Is NOT in SafeZone");
        }
    }

}
