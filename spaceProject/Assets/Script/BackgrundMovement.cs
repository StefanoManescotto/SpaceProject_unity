using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgrundMovement : MonoBehaviour {
    Material mat;
    public float parallax;
    void Start(){
        mat = GetComponent<MeshRenderer>().material;
    }

    void Update() {
        Vector2 offset = mat.mainTextureOffset;
        offset.x = transform.position.x / 100 / parallax;
        offset.y = transform.position.y / 70 / parallax;
        mat.mainTextureOffset = offset;
    }
}
