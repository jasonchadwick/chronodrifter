using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class LevelScreen : MonoBehaviour {
    int nlevels;
    public int rows;
    public int cols;
    Canvas canvas;

    void Start() {
        nlevels = SceneManager.sceneCountInBuildSettings;
        canvas = GetComponent<Canvas>();
        Rect pixelRect = canvas.pixelRect;

        float topLeftX = pixelRect.center.x - pixelRect.width / 2;
        float topLeftY = pixelRect.center.y + pixelRect.height * 0.3f;
        float tileWidth = pixelRect.width / cols;
        float tileHeight = (pixelRect.height * 0.8f) / rows;

        float xbuf = tileWidth * 0.1f;
        float ybuf = tileHeight * 0.1f;

        string[] thumbnailNames = {
            "level0",
            "level1",
            "level2"
        };

        for (int r = 0; r < rows; r++) {
            float x = topLeftX + tileWidth  * r;
            for (int c = 0; c < cols; c++) {
                float y = topLeftY - tileWidth * c;
                Rect newRect = new Rect(x+xbuf, y-ybuf, tileWidth-2*xbuf, tileHeight-2*ybuf);
                int levelidx = r*cols + c;
                string levelName = thumbnailNames[levelidx];
            }
        }

        // get screenshot of each level and save in Assets/Scenes/LevelThumbnails.
        // each level is a button, set texture of button to be the thumbnail indexed by the level index
    }


}