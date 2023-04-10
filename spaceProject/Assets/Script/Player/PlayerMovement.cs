using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerMovement : MonoBehaviour, ISteering {

    public int rotationSpeed;
    public int movementSpeed;
    public float maxSpeed;
    public float speed;
    public float maxForce = 1;
    public float mass = 150;
    public GameObject playerClick;
    public GameObject rightClickObj;
    private Rigidbody2D rb;
    private PlayerShooting playerShooting;
    public Vector3 velocity;
    private SteeringBehavior steeringBehavior;
    public float velocityMultiplier;

    private int clickNumber = 0;
    private float clickDelay = .3f;
    private float clickTime = 0f;
    private bool isDoubleCLick = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        playerClick.transform.position = transform.position;
        rightClickObj.transform.position = transform.position;
        playerShooting = GetComponent<PlayerShooting>();
        steeringBehavior = new SteeringBehavior(this);
        velocity = new Vector3(0, 0, 0);
    }

    void FixedUpdate() {
        //Debug.Log("ms: " + maxSpeed);
        MovePlayerArcade();
        RotatePlayer();
    }

    private void RotatePlayer() {
        if (Input.GetMouseButton(1)) {
            rightClickObj.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        Vector3 targ = rightClickObj.transform.position;
        targ.z = 0f;

        targ.x = targ.x - transform.position.x;
        targ.y = targ.y - transform.position.y;

        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;


        // targ = new Vector3(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), Time.deltaTime * rotationSpeed);

        //if (angle < 0) {
        //    angle += 360;
        //}

        //playerShooting.rightAngle = (angle - transform.eulerAngles.z < 10f);
    }

    private void MovePlayer() {
        if (Input.GetMouseButton(0) || isDoubleCLick) {
            if (Input.GetMouseButtonDown(0)) {
                if (Time.time - clickTime > clickDelay) {
                    clickNumber = 0;
                }
                clickNumber++;
                isDoubleCLick = IsDoubleClick();
            }

            if (!isDoubleCLick && Time.time - clickTime > clickDelay) {
                clickNumber = 0;
            }

            if (!isDoubleCLick) {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                //playerClick.transform.position = pos;
                rightClickObj.transform.position = pos;

                if (Vector3.Distance(pos, transform.position) > velocityMultiplier) {
                    pos = pos - transform.position;
                    pos.Normalize();
                    pos *= velocityMultiplier;
                    pos += transform.position;
                    playerClick.transform.position = pos;
                }
            }

            velocity = steeringBehavior.GoToTarget(playerClick.transform.position);
        } else {
            velocity *= 0.95f;
        }

        Vector3 newVelocity = (velocity * (Vector3.Distance(playerClick.transform.position, transform.position) / velocityMultiplier)) * Time.deltaTime * speed;
        transform.position += newVelocity;
        playerClick.transform.position += newVelocity;
        rightClickObj.transform.position += newVelocity;
    }

    private bool IsDoubleClick() {
        if (clickNumber == 1) {
            clickTime = Time.time;
        } else if (clickNumber > 1 && Time.time - clickTime <= clickDelay) {
            clickNumber = 0;
            //clickTime = 0;
            return true;
        } else {
            clickNumber = 0;
        }

        return false;
    }

    private void MovePlayerArcade() {
       if (Input.GetMouseButton(0)) {
           Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
           pos.z = 0;
           playerClick.transform.position = pos;
           rightClickObj.transform.position = pos;
       }

       if (Vector3.Distance(transform.position, playerClick.transform.position) > 1f) {
           // transform.position = Vector2.MoveTowards(transform.position, playerClick.transform.position, movementSpeed * Time.deltaTime);
           velocity = Vector3.Normalize(playerClick.transform.position - transform.position) * movementSpeed * Time.fixedDeltaTime * 20;
           rb.velocity += new Vector2(velocity.x, velocity.y);
           rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
           //transform.position += velocity;
       } else {
            rb.velocity *= 0.7f;
            //velocity.x = 0f;
            //velocity.y = 0f;
       }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public Vector3 GetVelocity() {
        return velocity;
    }

    public float GetMaxSpeed() {
        return maxSpeed;
    }

    public float GetMass() {
        return mass;
    }

    public float GetMaxForce() {
        return maxForce;
    }

    public Vector3 GetEulerRotation() {
        return transform.rotation.eulerAngles;
    }
}
