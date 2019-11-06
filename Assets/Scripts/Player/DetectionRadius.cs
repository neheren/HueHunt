using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionRadius : MonoBehaviour
{
    public float detectionRadius = 10f;
    public bool debug;

    public bool lightOn = true;

    private float hidingBuff = 1.5f;


    private void Start()
    {
        debug = true;
    }

    public void Hiding(bool isHiding)
    {
        if (isHiding)
            detectionRadius *= 1 / hidingBuff;
        else
            detectionRadius *= hidingBuff;

    }

    public bool LightOn{
        get { return lightOn; }
        set{
            if (value == true){
                detectionRadius *= 2f;
                lightOn = true;
            }
            else{
                detectionRadius *= .5f;
                lightOn = false;
            }
        }
    }

    void Update()
    {
        if (!lightOn)
        {
            LightOn = false;
            lightOn = true;
        }
    }

    


    private void OnDrawGizmos()
    {
        if(debug)
        Gizmos.DrawWireSphere(GetComponent<Transform>().position, detectionRadius);
    }
}
