using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    public GameObject player;
    public float sensitivity;
    public float minFov, maxFov;
    
    public GameObject background1, background2;

    void Update() {
        MoveCamera();
        ZoomCamera();
    }
    
    private void MoveCamera(){
        Vector3 playerPos = player.transform.position;
        playerPos.z = -10f;
        transform.position = playerPos;
    }

    private void ZoomCamera(){
        float size = Camera.main.orthographicSize;
        size += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        size = Mathf.Clamp(size, minFov, maxFov);
        Camera.main.orthographicSize = size;

        // Changing background scale so that it's always the same size as the camera size
        background1.transform.localScale = new Vector3(Camera.main.orthographicSize * Camera.main.aspect * 2, Camera.main.orthographicSize * 2, 0);
        background2.transform.localScale = background1.transform.localScale;
    }
}
