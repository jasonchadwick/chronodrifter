using System.Collections.Generic;
using UnityEngine;

class Utils {
    public static string[] levelNames = {
            "Your Education Begins",
            "Some Basic Time Travel",
            "Up and Down",
            "Falling Upwards",
            "Please Pick Up After Yourself"
    };

    public static string formLevelName(int idx, int numNewLines, bool addLevelWord = false) {
        string levelName = "";
        if (addLevelWord) {
            levelName += "LEVEL ";
        }
        levelName += idx.ToString();
        for (int i = 0; i < numNewLines; i++) {
            levelName += "\n";
        }
        levelName += levelNames[idx];
        return levelName;
    }

    public static float AngleLerp(float angleA, float angleB, float t) {
        // in degrees
        if (angleA - angleB > 180) {
            angleB += 360;
        }
        else if (angleB - angleA > 180) {
            angleA += 360;
        }

        return Mathf.Lerp(angleA, angleB, t);
    }

    public static Vector2 Vector3to2(Vector3 v) {
        return new Vector2(v.x, v.y);
    }
}