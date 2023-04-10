using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    [SerializeField] PlayerManager playerManager;
    [SerializeField] PlayerMovement playerMovement;
    public GameObject projectile;
    public GameObject leftGun;
    public GameObject rightGun;
    public GameObject rightClickObj;
    public bool canShoot = true;
    public bool rightAngle = false;
    public float shootDelay;
    private IGuns gunsScriptL, gunsScriptR;
    private List<IGuns> playerGuns = new List<IGuns>();
    public List<GameObject> abilities;


    private bool canDash = true;
    //public float health = 100f;

    void Start() {
        gunsScriptL = leftGun.GetComponent<IGuns>();
        gunsScriptR = rightGun.GetComponent<IGuns>();
        playerGuns.Add(gunsScriptL);
        playerGuns.Add(gunsScriptR);
    }

    void Update() {
        Vector3 targ = rightClickObj.transform.position;
        targ.z = 0f;

        targ.x = targ.x - transform.position.x;
        targ.y = targ.y - transform.position.y;

        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        if(angle < 0) {
           angle += 360; 
        }

        // if(transform.rotation.eulerAngles.z < angle + 20 && transform.rotation.eulerAngles.z > angle - 20){
            
        // }
        
        if (Input.GetMouseButton(1)) {
            gunsScriptL.Shoot(transform.rotation, "Friendly");
            gunsScriptR.Shoot(transform.rotation, "Friendly");
        }





        // --------------- ABILITIES -------------------------
        if(Input.GetKeyDown(KeyCode.Space)){
            if(canDash){
                StartCoroutine(Dash());
            }
        }


        ExecuteAbilities();




        // Debug.Log("angle: " + (angle) + " - " + transform.eulerAngles.z + " - " + (angle - transform.eulerAngles.z));
    }

    private void ExecuteAbilities(){
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            abilities[0].GetComponentInChildren<IAbility>().ExecuteAbility(playerGuns);
        }
    }

    private void IncreaseGunSpeed(){
        //StartCoroutine(((Gun1)gunsScriptL).IncreaseShootSpeed());
        //StartCoroutine(((Gun1)gunsScriptR).IncreaseShootSpeed());
        // IGuns[] iguns = GetComponents<IGuns>();
        // foreach(IGuns i in iguns){
        //     if(i.GetGunID() == "gun1"){
        //         StartCoroutine(((Gun1)i).IncreaseShootSpeed());
        //     }
        // }
    }

    private IEnumerator Dash() {
        canDash = false;
        playerManager.setCanBeHit(false);
        float ms = playerMovement.maxSpeed;
        playerMovement.maxSpeed = playerMovement.maxSpeed * 5;
        yield return new WaitForSeconds(.2f);
        playerMovement.maxSpeed = ms;
        playerManager.setCanBeHit(true);
        StartCoroutine(DashCooldown());
    }

    private IEnumerator DashCooldown() {
        canDash = false;
        yield return new WaitForSeconds(2f);
        canDash = true;
    }
    // public float GetHealth() {
    //     return health;
    // }

    // public void RemoveHealth(float damage) {
    //     health -= damage;
    //     if(health <= 0) {
    //         Debug.Log("Player destroyed");
    //     }
    // }
}
