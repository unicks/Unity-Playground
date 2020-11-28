using System.Collections;
using UnityEngine;

public class PointPlatform : MonoBehaviour {
    public static event System.Action OnPlayerWin;
    public static event System.Action<PointPlatform> OnGainPoint;
    public static int points;
    private static int maxPoints;
    public Material material;
    private bool isAcive = true;

    private void Start() {
        points = 0;
        maxPoints = GameObject.FindGameObjectsWithTag("PointPlatform").Length;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.name == "Player" && isAcive) {
            points++;

            if (OnGainPoint != null) {
                isAcive = false;
                StartCoroutine(transitionColor());
                OnGainPoint(this);
            }
            
            if (points == maxPoints && OnPlayerWin != null) {
                OnPlayerWin();
            }
        }
    }

    IEnumerator transitionColor() {
        const float transitionTime = 0.6f;
        float timeSinceDisable = 0;
        Color originalColor = material.color;
        
        while(timeSinceDisable < 1) {
            timeSinceDisable += Time.deltaTime;
            gameObject.GetComponent<Renderer>().material.color = Color.Lerp(originalColor, Color.green, timeSinceDisable / transitionTime);
            yield return null;
        }
        yield return null;
    }
}
