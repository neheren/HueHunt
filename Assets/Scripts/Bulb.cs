using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulb : MonoBehaviour
{
    // Start is called before the first frame update
    public Color color;
    void Start() {   
        
    }

    // Update is called once per frame
    void Update() {
        GetComponent<Renderer>().material.color = color;
        GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
    }
}
