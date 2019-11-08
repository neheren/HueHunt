using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class PseudoLight : MonoBehaviour {
    public enum LightType // your custom enumeration
    {
        Static,
        Pulse,
        Strobe
    };
    public LightType outerType = LightType.Static; 
    public LightType innerType = LightType.Static; 
    private AudioSource audio;
    public Color LightColor;
    public float intensity = 1.0f;
    public float flashPrSecond = 2;
    public float outerDistanceThreshold = 10.0f;
    public float innerDistanceThreshold = 3.0f;
    public bool increaseFlashesWithDistance;
    public float distanceToPlayer = 0f;
    private Renderer renderReference;
    public bool enableDebug = true;
    void Start() {
        renderReference = GetComponent<Renderer>();
        LightColor *= intensity;
        renderReference.material.color = LightColor;
        audio = GetComponent<AudioSource>();
    }

    private float ellipseFaster = 0f;

    void Update() {
        if(distanceToPlayer < outerDistanceThreshold) {
            LightType chosenLightType = distanceToPlayer < innerDistanceThreshold ? innerType : outerType;
            float currentFlashesPrSecond = increaseFlashesWithDistance 
                ? flashPrSecond
                : flashPrSecond;

            if(chosenLightType == LightType.Pulse) {
                float frameCount = (float)Time.frameCount;
                intensity = (1 + Mathf.Sin((Time.fixedTime + ellipseFaster) * currentFlashesPrSecond)) / 2;
                renderReference.material.color = LightColor * intensity;
                if(audio != null) {
                    audio.volume = intensity;
                }
            } else if(chosenLightType == LightType.Strobe) {
                float frameCount = (float)Time.frameCount;
                intensity = (int)(Time.fixedTime * currentFlashesPrSecond % 2);
                renderReference.material.color = LightColor * intensity;
            }
        }

        if(enableDebug){
            DrawCircle(transform.position, outerDistanceThreshold);
        }
    }

    int drawSteps = 10;
    public void DrawCircle(Vector3 pos, float radius, bool gizmo = false) {
        for (int i = 0; i < 360; i += drawSteps) {
            Vector3 from = new Vector3(Mathf.Sin(Mathf.Deg2Rad * i), 0, Mathf.Cos(Mathf.Deg2Rad * i));
            Vector3 to = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (i + drawSteps / 2)), 0, Mathf.Cos(Mathf.Deg2Rad * (i + drawSteps / 2)));
            if(gizmo){
                Gizmos.DrawLine(from * outerDistanceThreshold + pos, to * outerDistanceThreshold + pos);
                Gizmos.DrawLine(from * innerDistanceThreshold + pos, to * innerDistanceThreshold + pos);
            }else{
                Debug.DrawLine(from * outerDistanceThreshold + pos, to * outerDistanceThreshold + pos, LightColor * intensity, Time.deltaTime);
                Debug.DrawLine(from * innerDistanceThreshold + pos, to * innerDistanceThreshold + pos, LightColor * intensity, Time.deltaTime);
            }
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = LightColor;
        DrawCircle(transform.position, outerDistanceThreshold, true);
    }
    float calculateDistanceFactor (float distanceToPlayer, float distanceThreshold) {
        return 1 - Mathf.Min(distanceToPlayer / distanceThreshold, 1);
    }
}
