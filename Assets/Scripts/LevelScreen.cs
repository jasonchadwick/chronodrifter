using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;

class LevelScreen : MonoBehaviour {
    int nlevels;
    public int rows;
    public int cols;
    Canvas canvas;
    public GameObject buttonPrefab;
    public Button mainMenuButton;

    void Start() {
        // assume there are 2 non-level scenes in the game (main menu and level menu)
        nlevels = SceneManager.sceneCountInBuildSettings - 2;
        canvas = GetComponent<Canvas>();
        Rect pixelRect = canvas.pixelRect;

        Vector3 botLeft = Camera.main.ViewportToWorldPoint(new Vector3(-39, -39, 10));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(40, 30, 10));
        float topLeftX = botLeft.x;
        float topLeftY = topRight.y;
        float tileWidth = (topRight.x - botLeft.x) / cols;
        float tileHeight = (topRight.y - botLeft.y) / rows;

        float xbuf = tileWidth * 0.05f;
        float ybuf = tileHeight * 0.05f;

        // automatically place level buttons based on how many levels there are
        for (int r = 0; r < rows; r++) {
            float y = topLeftY - tileHeight * r;
            for (int c = 0; c < cols; c++) {
                int levelidx = r*cols + c;
                if (levelidx < nlevels) {
                    float x = topLeftX + tileWidth * c;
                    float leftX = x;
                    float topY = y;
                    float rightX = x+tileWidth;
                    float botY = y-tileHeight;

                    GameObject b = Instantiate(buttonPrefab, new Vector3((leftX+rightX)/2, (topY+botY)/2, 0), Quaternion.identity);
                    b.transform.SetParent(canvas.transform, false);
                    b.GetComponent<RectTransform>().sizeDelta = new Vector2(tileWidth-2*xbuf, tileHeight-2*xbuf);
                    b.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(tileWidth-4*xbuf, tileHeight-4*xbuf);
                    b.GetComponentInChildren<Text>().text = Utils.formLevelName(levelidx, 1);
                    b.GetComponent<Button>().onClick.AddListener(() => LoadLevel(levelidx));
                }
            }
        }

        mainMenuButton.onClick.AddListener(MainMenu);

        // get screenshot of each level and save in Assets/Scenes/LevelThumbnails.
        // each level is a button, set texture of button to be the thumbnail indexed by the level index
    }

    public void LoadLevel(int levelidx) {
        Destroy(FindObjectOfType<MainMenuMusic>().gameObject);
        TimeEventManager.Reset();
        SceneManager.LoadScene(levelidx + 2);
    }

    public void MainMenu() {
        TimeEventManager.Reset();
        SceneManager.LoadScene("MainMenu");
    }
}