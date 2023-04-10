using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour {

    public float projectileSpeed = 10f;
    public int damage = 20;
    private float despawnTime = 2f;

    void Start() {
        Destroy(gameObject, despawnTime);
    }

    void Update() {
        transform.position += transform.right * Time.deltaTime * projectileSpeed;

    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.layer != gameObject.layer) {
            IGeneralParameters g = col.GetComponent<IGeneralParameters>();
            if(g != null) {
                g.DealDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
