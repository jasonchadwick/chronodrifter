using System.Collections.Generic;
using UnityEngine;

class Utils {
    public static string[] levelNames = {
            "Your Education Begins",
            "Some Basic Time Travel",
            "Up Down Up",
            "Floating",
            "Falling Upwards",
            "Please Pick Up After Yourself",
            "Don't Touch the Red",
            "Good Luck",
            "Hmm...",
            "Cause and Effect",
            "Making Friends",
            "level11"
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

    public static Vector3 v3mult(Vector3 a, Vector3 b) {
        return new Vector3(a.x*b.x, a.y*b.y, a.z*b.z);
    }

    public static Vector3 v3div(Vector3 a, Vector3 b) {
        return new Vector3(a.x/b.x, a.y/b.y, a.z/b.z);
    }

    public static void ScaleFromPoint(Transform transform, Vector3 anchor, Vector3 scale) {
        Vector3 fractionalScale = v3div((scale - transform.localScale), transform.localScale);
        if (fractionalScale.magnitude < 0.001f) {
            return;
        }
        Vector3 delta = transform.localPosition - anchor;
        Vector3 newAnchor = anchor - v3mult(delta,fractionalScale);
        transform.localScale = scale;
        transform.localPosition += (anchor - newAnchor);
    }
}