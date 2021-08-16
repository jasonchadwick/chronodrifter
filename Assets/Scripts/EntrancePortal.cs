using System.Collections.Generic;
using UnityEngine;

class EntrancePortal : MonoBehaviour {
    public GameObject playerPrefab;
    private GameObject player;

    void Start() {
        player = Instantiate(playerPrefab, transform.position + Vector3.down, Quaternion.identity);
        Camera.main.GetComponent<CameraFollowPlayer>().target = player.transform;
    }
}