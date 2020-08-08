using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ManageBlocks : MonoBehaviour {

    public GameObject fallingBlockPrefab;
    private List<GameObject> fallingBlocks = new List<GameObject>();

    float screenHalfWidthInWorldUnits;
    public float fallingBlockVelocity;
    public Vector2 minmaxFallingBlockVelocity;
    public Vector2 minmaxFallingBlockCreationDiff;
    public Vector2 minMaxBlockSize;
    public float maxBlockRotation;
    private float nextBlockInvoke = 0f;
    private float blockCreationDiff;

    void Start() {
        screenHalfWidthInWorldUnits = Camera.main.aspect * Camera.main.orthographicSize;
    }

    void Update() {
        fallingBlockVelocity = Mathf.Lerp(minmaxFallingBlockVelocity.x, minmaxFallingBlockVelocity.y, Difficulty.GetDifficulty());
        blockCreationDiff = Mathf.Lerp(minmaxFallingBlockCreationDiff.x, minmaxFallingBlockCreationDiff.y, Difficulty.GetDifficulty());
        print(blockCreationDiff + " " + fallingBlockVelocity);
        if (Time.timeSinceLevelLoad > nextBlockInvoke) {
            CreateFallingBlock();
            nextBlockInvoke += blockCreationDiff;
        }

        foreach(GameObject fallingBlock in fallingBlocks.ToList()) {
            fallingBlock.transform.Translate(Vector2.down * fallingBlockVelocity * Time.deltaTime);
            if (fallingBlock.transform.position.y < -Camera.main.orthographicSize - fallingBlockPrefab.transform.localScale.y) {
                fallingBlocks.Remove(fallingBlock);
                Destroy(fallingBlock);
            }
        }    
    }

    private void CreateFallingBlock() {
        float xAxis = Random.Range(-screenHalfWidthInWorldUnits, screenHalfWidthInWorldUnits);
        Quaternion rotation;
        if (xAxis < 0) {
            rotation = Quaternion.Euler(Vector3.forward * Random.Range(0, maxBlockRotation));
        } else {
            rotation = Quaternion.Euler(Vector3.forward * Random.Range(-maxBlockRotation, 0));
        }
        
        GameObject fallingBlock =  (GameObject)Instantiate(fallingBlockPrefab, new Vector2(xAxis, Camera.main.orthographicSize + fallingBlockPrefab.transform.localScale.y), rotation);
        float size = Random.Range(minMaxBlockSize.x, minMaxBlockSize.y);
        fallingBlock.transform.localScale = new Vector2(size, size);
        fallingBlock.GetComponent<Renderer>().material.SetColor("_Color", Color.Lerp(Color.white, Color.red, Difficulty.GetDifficulty()));
        fallingBlocks.Add(fallingBlock);
    }
}
