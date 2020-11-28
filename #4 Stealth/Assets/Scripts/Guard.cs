using System;
using System.Collections;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public Transform pathHolder;
    public Transform player;
    public LayerMask viewMask;

    public float speed = 3;
    public float timeAtWaypoint = .1f;
    public float turnSpeed = 90f;

    public float timeToSpotPlayer = .5f;
    public Light spotlight;
    public float viewDistance;
    Color originalSpotlightColour;
    float viewAngle;
    float timePlayerSpotted = 0;

    public static event Action OnPlayerSpotted;
    IEnumerator currentPathRoutine;

    void Start() {
        viewAngle = spotlight.spotAngle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalSpotlightColour = spotlight.color;

        if(pathHolder != null) {
            Vector3[] waypoints = new Vector3[pathHolder.childCount];
            for(int i = 0; i < waypoints.Length; i++) {
                waypoints[i] = pathHolder.GetChild(i).position;
                waypoints[i].y = transform.position.y;
            }
            currentPathRoutine = FollowPath(waypoints);
            StartCoroutine(currentPathRoutine);
        }
    }

    void Update() {
        if (spotPlayer()) {
            timePlayerSpotted += Time.deltaTime;
        }
        else {
            timePlayerSpotted -= Time.deltaTime;
        }

        timePlayerSpotted = Mathf.Clamp(timePlayerSpotted, 0, timeToSpotPlayer);
        spotlight.color = Color.Lerp(originalSpotlightColour, Color.red, timePlayerSpotted / timeToSpotPlayer);

        if (timePlayerSpotted == timeToSpotPlayer && OnPlayerSpotted != null) {
            OnPlayerSpotted();
        }
    }

    bool spotPlayer() {
        if (Vector3.Distance(transform.position, player.position) < viewDistance) {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f) {
                if (!Physics.Linecast(transform.position, player.position, viewMask)) {
                    return true;
                }
            }
        }
        return false;
    }

    // My implementation of spotPlayer. It is pretty good, but less elegant for our purposes
    bool mySpotPlayer() {
        const int raycastDensity = 20;
        float leapAngle = viewAngle / raycastDensity;
        float rayAngle = -viewAngle * 0.5f;
        RaycastHit hitInfo;
        bool isHit = false;

        for (int i = 0; i < raycastDensity && !isHit; i++) {
            Vector3 targetPos = (Quaternion.Euler(0, rayAngle, 0) * transform.forward).normalized * viewDistance;

            if(Physics.Linecast(transform.position, targetPos, out hitInfo) &&
                hitInfo.collider.gameObject.transform.Equals(player)) {
                isHit = true;
            } else {
                rayAngle += leapAngle;
            }
        }

        return isHit;
    }

    IEnumerator FollowPath(Vector3[] waypoints) {
        transform.position = waypoints[0];
        int targetWaypointIndex = 1;
        transform.LookAt(waypoints[targetWaypointIndex]);
        
        while (true) {
            yield return StartCoroutine(RotateTowardsWaypoint(waypoints[targetWaypointIndex]));
            yield return StartCoroutine(MoveToWaypoint(waypoints[targetWaypointIndex]));
            targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
        }
    }

    IEnumerator RotateTowardsWaypoint(Vector3 lookTarget) {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.2f) {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null; 
        }
    }

    IEnumerator MoveToWaypoint(Vector3 waypoint) {
        while (transform.position != waypoint) {
            transform.position = Vector3.MoveTowards(transform.position, waypoint, speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(timeAtWaypoint);
    }

    void OnDrawGizmos() {
        if (pathHolder != null) {
            Vector3 startPosition = pathHolder.GetChild(0).position;
            Vector3 previousPosition = startPosition;

            foreach(Transform waypoint in pathHolder) {
                Gizmos.DrawSphere(waypoint.position, .3f);
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }

            Gizmos.DrawLine(previousPosition, startPosition);
        }
    }
}
