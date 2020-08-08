using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chaser : MonoBehaviour {

    public Transform targetTransform;
    public float speed = 7f;

    void Update() {
        Vector3 distanceFromTarget = targetTransform.localPosition - transform.localPosition;
        Vector3 velocity = distanceFromTarget.normalized * speed;
        Vector3 moveAmount = velocity * Time.deltaTime;

        if ((distanceFromTarget.magnitude < 2.2f) &&
             distanceFromTarget.magnitude > 1.8f) {
            moveAmount = Vector3.zero;
        } else if (distanceFromTarget.magnitude < 1.8f) {
            moveAmount = -moveAmount * 2;
        }

        transform.Translate(moveAmount);

    }
}
