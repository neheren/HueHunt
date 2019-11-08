using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class orientation : MonoBehaviour
{
    public Camera camera;
    public PseudoLight[] pseudoLights;
    public Bulb[] bulbs;
    public bool enableDebug = true;
    public int hueUpdateRate = 10;

    // Start is called before the first frame update
    void Start() {
        pseudoLights = FindObjectsOfType<PseudoLight>();
        bulbs = FindObjectsOfType<Bulb>();
    }

    void Update() {
        Vector3 playerPosition = camera.transform.position;
        for (int l = 0; l < bulbs.Length; l++) {
            // Each bulb:
            bulbs[l].color = Color.black;
            List<Color> colors = new List<Color>();     
            Vector3 bulbOrientation = Vector3.Normalize(bulbs[l].transform.position - playerPosition);

            for (int i = 0; i < pseudoLights.Length; i++) {
                float distanceFactor = calculateDistanceFactor(playerPosition, pseudoLights[i].transform.position, pseudoLights[i].outerDistanceThreshold);
                if(distanceFactor > 0) {
                    pseudoLights[i].distanceToPlayer = Vector3.Distance(playerPosition, pseudoLights[i].transform.position);
                    Vector3 objectOrientation = Vector3.Normalize(pseudoLights[i].transform.position - playerPosition);
                    float simularity = (calculateSimularity(objectOrientation, bulbOrientation)); 
                    Color colorWeight = pseudoLights[i].LightColor * pseudoLights[i].intensity * simularity * distanceFactor;
                    colors.Add(colorWeight);
                    if(enableDebug){
                        debug(playerPosition, pseudoLights[i].transform.position);
                    }
                }
            }

            Color avgColor = new Color(0, 0, 0);
            foreach(Color c in colors) {
                avgColor += c;
            }
            avgColor = (avgColor / pseudoLights.Length) * pseudoLights.Length;
            bulbs[l].color = avgColor;
            output(avgColor, l);
        }
    }

    void output(Color col, int bulbIndex) {
        float h, s, v;
        Color.RGBToHSV(col, out h, out s, out v);
//        print("s" + s);
        h *= 65535.0f;
        s *= 254.0f;
        v *= 254.0f;
        HueMessenger.currentMessage.lights[bulbIndex].setvalues(true, (int)h, (int)s, (int)v, 1);
    }

    void debug(Vector3 _playerPosition, Vector3 _objectOrientation) {
        Vector3 down = new Vector3(0, -1, 0);
        Debug.DrawLine(_playerPosition + down,  _playerPosition, Color.red, Time.deltaTime);
        Debug.DrawLine(_playerPosition + down, _objectOrientation, Color.blue, Time.deltaTime);
    }

    float calculateSimularity(Vector3 lamp, Vector3 dir) {
        return (Mathf.Max(Vector3.Dot(lamp, dir), 0f));
    }

    float calculateDistanceFactor (Vector3 from, Vector3 to, float distanceThreshold) {
        return 1 - Mathf.Min(Vector3.Distance(from, to) / distanceThreshold, 1);
    }
}
