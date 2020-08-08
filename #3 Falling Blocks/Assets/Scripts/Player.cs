using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Vector2 minMaxPlayrSpeed;
    public event System.Action OnPlayerDeath;

    float screenHalfWidthInWorldUnits;
    float halfPlayerSize;

    void Update() {
        float input = Input.GetAxisRaw("Horizontal");
        float velocity = input * Mathf.Lerp(minMaxPlayrSpeed.x, minMaxPlayrSpeed.y, Difficulty.GetDifficulty());
        transform.Translate(Vector2.right * velocity * Time.deltaTime);

        if ((transform.position.x > screenHalfWidthInWorldUnits + halfPlayerSize) ||
            (transform.position.x < -screenHalfWidthInWorldUnits - halfPlayerSize)) {
            transform.position = new Vector2(transform.position.x * -0.97f, transform.position.y);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Falling Block") {
            if(OnPlayerDeath != null) {
                OnPlayerDeath();
            }
            Destroy(gameObject);
        }
    }

    void Start() {
        transform.position = new Vector3(0, -Camera.main.orthographicSize + 1, 0);
        screenHalfWidthInWorldUnits = Camera.main.aspect * Camera.main.orthographicSize;
        halfPlayerSize = transform.localScale.x / 2f;
    }
}
