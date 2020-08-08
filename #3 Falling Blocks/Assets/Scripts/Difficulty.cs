using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Difficulty {
    
    public static float maxDifficultyTime = 60f;

    public static float GetDifficulty() {
        return Mathf.InverseLerp(0, maxDifficultyTime, Time.timeSinceLevelLoad);
    }

}
