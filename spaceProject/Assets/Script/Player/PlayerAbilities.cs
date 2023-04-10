using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour {
    // [SerializeField] PlayerMovement playerMovement;
    // [SerializeField] PlayerManager playerManager;
    // private bool canDash = true;

    // void Update() {
    //     if(Input.GetKeyDown(KeyCode.Space)){
    //         if(canDash){
    //             StartCoroutine(Dash());
    //         }
    //     }

    //     if(Input.GetKeyDown(KeyCode.Alpha1)){
    //         Debug.Log("1 pressed");
    //         IncreaseGunSpeed();
    //     }

    // }

    // private void IncreaseGunSpeed(){
    //     IGuns[] iguns = GetComponents<IGuns>();
    //     foreach(IGuns i in iguns){
    //         if(i.GetGunID() == "gun1"){
    //             StartCoroutine(((Gun1)i).IncreaseShootSpeed());
    //         }
    //     }
    // }

    // private IEnumerator Dash() {
    //     canDash = false;
    //     playerManager.setCanBeHit(false);
    //     float ms = playerMovement.maxSpeed;
    //     playerMovement.maxSpeed = playerMovement.maxSpeed * 5;
    //     yield return new WaitForSeconds(.2f);
    //     playerMovement.maxSpeed = ms;
    //     playerManager.setCanBeHit(true);
    //     StartCoroutine(DashCooldown());
    // }

    // private IEnumerator DashCooldown() {
    //     canDash = false;
    //     yield return new WaitForSeconds(2f);
    //     canDash = true;
    // }
}
