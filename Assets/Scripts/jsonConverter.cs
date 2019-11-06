using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class jsonConverter
{
    const string path = "Assets/JsonTestdoc.txt";


   
    public string stateOfLights (Light _light)
    {
        return JsonUtility.ToJson(_light);

    }


    public string updateScene (Light [] _lights) {

        string Jsonlight1 = JsonUtility.ToJson(_lights[0]);
        string Jsonlight2 = JsonUtility.ToJson(_lights[1]);
        string Jsonlight3 = JsonUtility.ToJson(_lights[2]);

        string lightState =  "{\"lightstates\":{" + "\"1\":" + Jsonlight1 + "," + "\"2\":" + Jsonlight2 + "," + "\"3\":" + Jsonlight3 + "}}";
        
        // StreamWriter writer = new StreamWriter(@path, true);
        // writer.Write(lightState);
        // writer.Close();

 

        return lightState;
    }

}
