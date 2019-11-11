using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class flashLight : MonoBehaviour
{

    public Material reveal;
    public Light fLight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        reveal.SetVector("_LightPosition",fLight.transform.position);
        reveal.SetVector("_LightDirection", -fLight.transform.forward);
         reveal.SetVector("_Strengthmagnitude", -fLight.transform.position);
        reveal.SetFloat("_LightAngle", fLight.spotAngle);

    


        // Debug.Log(reveal.GetFloat("_Strengthmagnitude"));

    }
}
