using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun1 : MonoBehaviour, IGuns {
    public GameObject projectile;
    public float shootDelay;
    private string id = "gun1";
    private bool canShoot = true;
    
    public string GetGunID(){
        return id;
    }

    public void Shoot(Quaternion r, string layerName) {
        if (canShoot) {
            GameObject p = Instantiate(projectile, transform.position, r);
            p.layer = LayerMask.NameToLayer(layerName);
            canShoot = false;
            StartCoroutine(ShootDelay());
        }
    }

    public IEnumerator IncreaseShootSpeed(float cooldown) {
        float sd = shootDelay;
        shootDelay /= 5;
        // Debug.Log("sd: " + shootDelay);
        yield return new WaitForSeconds(cooldown);
        shootDelay = sd;
    }

    private IEnumerator ShootDelay() {
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    public void DoAbility(string idAbility, float cooldown){
        switch(idAbility){
            case "fastFire":
                StartCoroutine(IncreaseShootSpeed(cooldown));
                break;
            default:
                break;
        }
    }
}
