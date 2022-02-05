using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class FadeInOnLoad : MonoBehaviour {
    public List<Graphic> targets;
    private List<(Graphic, Color)> tgtColors;
    public float fadeTime;
    private Color originalColor;
    private bool doneFading;

    public void Start() {
        tgtColors = new List<(Graphic, Color)>();
        foreach (Graphic g in targets) {
            tgtColors.Add((g, g.color));
        }
    }

    public void FixedUpdate() {
        if (!doneFading) {
            if (Time.timeSinceLevelLoad < fadeTime) {
                foreach ((Graphic g, Color c) in tgtColors) {
                    g.color = Color.Lerp(new Color(0, 0, 0, 0), c, Time.timeSinceLevelLoad / fadeTime);
                }
            }
            else {
                foreach ((Graphic g, Color c) in tgtColors) {
                    g.color = c;
                }
                doneFading = true;
            }
        }
    }
}