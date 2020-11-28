using System;
using UnityEngine;

public class MainCamera : MonoBehaviour {
    GameObject player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        Guard.OnPlayerSpotted += PlayerSpotted;
    }

    void FixedUpdate() {
        const int cameraStillRadius = 4;
        Vector3 cameraAtPlayerHight = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        Vector3 playerVelocity = player.GetComponent<Player>().velocity;
        Vector3 playerAfterMovement = player.transform.position + playerVelocity * Time.deltaTime;
        if (Vector3.Distance(playerAfterMovement, cameraAtPlayerHight) > cameraStillRadius) {
            transform.position = transform.position + playerVelocity * Time.deltaTime;
        }
    }

    void PlayerSpotted() {
        if (!GameObject.FindObjectOfType<GameUI>().isHardModeOn) {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        }
    }

    void OnDestroy() {
        Guard.OnPlayerSpotted -= PlayerSpotted;
    }
}
