using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour, ISteering, IGeneralParameters {

    public float health = 100;
    public float speed;
    public float maxSpeed;
    public Vector3 velocity;
    public float mass;
    public GameObject player;
    public float maxForce;
    // private bool isEvading = false;
    public GameObject target;
    //public GameObject t;
    public GameObject gun;
    public GameObject projectile;
    private IGuns gunScript;
    private bool runDecideAction = true;
    SteeringBehavior steeringBehavior;
    public float turningRadius;
    public float bounce;
    public Rigidbody2D rb;
    public float force;
    private bool canMove = true;
    public float rotateVelocity;
    private Vector2 ve = new Vector2(0,0);
    public float breakForce;
    public bool isAttacking = false;
    public float maxAngularVelocity;

    private bool canBeHit = true;

    private void Start() {
        velocity = Vector3.Normalize(new Vector3(1, 1, 0)) * maxSpeed * Time.deltaTime;
        steeringBehavior = new SteeringBehavior(this);
        //steeringBehavior.t = t;
        gunScript = gun.GetComponent<IGuns>();
        StartCoroutine(DecideActionRoutine());
    }


    void Update() {
       // velocity = steeringBehavior.GoToTarget(target.transform.position, maxTurningRadius: turningRadius);

        //velocity += GetSeparation();
        //velocity += steeringBehavior.GoToTarget((GetSeparation() + transform.position));

        //Vector3 v1 = target.transform.position;
        //Vector3 v2 = transform.position;
        //v1.z = v1.y;
        //v1.y = 0;
        //v2.z = v2.y;
        //v2.y = 0;
        //Vector3 cross = Vector3.Cross(transform.rotation * Vector3.forward, target.transform.position);
        //bool object2IsToTheRight = (Vector3.SignedAngle(target.transform.position, transform.position, velocity)) > 0;


        //Debug.Log(AngleBetween(target.transform.position) + " " + transform.rotation.eulerAngles

        //velocity = steeringBehavior.Wander();
        //velocity = steeringBehavior.fleeTarget(target.transform.position);
        //velocity = steeringBehavior.PursuitTarget(player.transform.position, player.GetComponent<PlayerMovement>().velocity);
        //FightPlayer();
        //velocity = steeringBehavior.Wander(a);
        //DecideAction();


        //Debug.Log("angle1: " + Vector3.Angle(transform.position, velocity));
        //float angle = Vector3.Angle(transform.position, velocity);
        //if (angle > 90) {
        //    angle = 90;
        //}

        //desiredVelocity = Quaternion.AngleAxis(angle, desiredVelocity) * desiredVelocity;
        //velocity = Quaternion.Euler(0, 0, angle) * velocity;
       // float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
       // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), Time.deltaTime * 8f);
       // Vector2 v = transform.position + (velocity * speed * Time.deltaTime);

        //rb.MovePosition(new Vector2(v.x, v.y));
       // rb.velocity = ((target.transform.position - transform.position) * speed * Time.deltaTime);
        
    }

    private void FixedUpdate() {
        //rb.angularVelocity = maxAngularVelocity;
        DecideAction();
    }


    public void moveInDirection(Vector2 direction) {
        Vector2 v1 = new Vector2(transform.position.x, transform.position.y);
        Vector2 v2 = new Vector2(target.transform.position.x, target.transform.position.y);

        float angle = AngleBetween(direction, rb.velocity);
        if (angle < 0) {
            angle += 360;
        }

        float diffAngle = Mathf.Min(Mathf.Abs(angle - GetEulerRotation().z), 360 - Mathf.Abs(angle - GetEulerRotation().z));
        if (diffAngle > turningRadius) {
            if (IsAngleToRight(angle, GetEulerRotation().z)) {
                direction = Quaternion.Euler(0, 0, (diffAngle + ((180 - diffAngle) * 2)) + turningRadius) * direction;
            } else {
                direction = Quaternion.Euler(0, 0, diffAngle - turningRadius) * direction;
            }
        }

        Vector3 sep = GetSeparation();
        direction += new Vector2(sep.x, sep.y);
        //ve += v * speed * Time.fixedDeltaTime;
        //ve = Vector2.ClampMagnitude(ve, maxSpeed


        // Debug.Log(Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg + " - " + rb.rotation);
        float a = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        if ((rb.rotation > a + 20 || rb.rotation < a - 20) && rb.velocity.magnitude > maxSpeed / 2) {
            direction = rb.velocity;
            direction *= breakForce * -1;

            rb.velocity += direction * Time.fixedDeltaTime;
            //rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed/2);
        } else {
            rb.velocity += direction * speed * Time.fixedDeltaTime;
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

    public bool IsAngleToRight(double angle1, double angle2) {
        double diff = angle2 - angle1;
        if (diff < 0)
            diff += 360;
        return diff > 180;
    }

    float AngleBetween(Vector2 v1, Vector2 v2) {
        Vector2 t = new Vector3();
        t.x = v1.x - v2.x;
        t.y = v1.y - v2.y;

        float angle = Mathf.Atan2(t.y, t.x) * Mathf.Rad2Deg;
        return angle;
        //rotation = Quaternion.LookRotation(relativePos);
    }

    public Vector3 GetSeparation() {
        Vector3 f = Vector3.zero;
        int neighborCount = 0;

        Collider2D[] coll = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 5);
        foreach (Collider2D c in coll) {
            f.x += c.transform.position.x - transform.position.x;
            f.y += c.transform.position.y - transform.position.y;
            neighborCount++;
        }

        if(neighborCount != 0) {
            f.x /= neighborCount;
            f.y /= neighborCount;

            f *= -1;
        }

        f.Normalize();
        f *= bounce;

        return f;
    }

    public Vector3 GetEulerRotation() {
        return transform.rotation.eulerAngles;
    }

    private void DecideAction() {
        float minDistance = 10000f;
        Vector2 targetDirection = new Vector2(player.transform.position.x, player.transform.position.y) - new Vector2(transform.position.x, transform.position.y);
        Collider2D[] coll = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 80);

        foreach(Collider2D c in coll) {
            if (c.tag == "Friendly") {
                float d = Vector3.Distance(c.gameObject.transform.position, transform.position);
                if(d < minDistance) {
                    target = c.gameObject;
                    minDistance = d;
                }
            }
        }
        //Debug.Log(minDistance);
        if(minDistance > 50f){
            isAttacking = true;
        }else if(minDistance < 10f){
            isAttacking = false;
        }

        if(isAttacking){
            //isAttacking = true;
            moveInDirection(targetDirection);

            if(minDistance < 35f){
                gunScript.Shoot(transform.rotation, "Enemy");
            }
            //Debug.Log("Attacking");
        }else if(!isAttacking){
            targetDirection = -targetDirection;
            moveInDirection(targetDirection);
           // isAttacking = false;
            //Debug.Log("Fleeing");
        }

        RotateEnemy(targetDirection);

        // if (IsAttackRange(minDistance) && false) {
        //     FightPlayer();
        // }else if (IsPursuitRange(minDistance) || true) {
        //     moveToTarget(new Vector2(player.transform.position.x, player.transform.position.y));
        // } else {

        // }
    }

    private IEnumerator DecideActionRoutine() {
        while(runDecideAction) { 
            yield return new WaitForSeconds(.1f);
            //DecideAction();
        }
    }

    private IEnumerator StunTime() {
        canMove = false;
        yield return new WaitForSeconds(.01f);
        velocity = rb.velocity;
        canMove = true;
        //yield return new WaitForSeconds(2f);
        //canRotate = true;
    }

    private bool IsAttackRange(float d) {
        return d < 55f;
    }

    private bool IsPursuitRange(float d) {
        return d < 80f;
    }

    private void RotateEnemy(Vector2 direction){
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        float angle2 = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        

        if(angle2 < angle + 20 && angle2 > angle - 20){
            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, angle2, rotateVelocity * Time.fixedDeltaTime));
        }else{
            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, angle, rotateVelocity * Time.fixedDeltaTime));
        }
    }

    private void FightPlayer() {
        Vector3 v = target.transform.position - transform.position;

        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), Time.deltaTime * 8f);
        gunScript.Shoot(transform.rotation, "Enemy");

        float d = Vector3.Distance(target.transform.position, transform.position);

        if (d > 55f) {
            //Debug.Log("Mi Avvicino");
            //velocity = steeringBehavior.Wander(angle);
            moveInDirection(new Vector2(player.transform.position.x, player.transform.position.y));
        } else if (d < 10f) {
            //Debug.Log("Mi Allontano");
            //velocity = steeringBehavior.Wander(-angle);
            moveInDirection(new Vector2(-player.transform.position.x, -player.transform.position.y));
        } else {
           // velocity = steeringBehavior.Wander();
        }

        //velocity = v * .001f;
        //velocity = steeringBehavior.Wander(a);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Collsion detected");
        Vector2 dir = new Vector2(transform.position.x, transform.position.y) - new Vector2(collision.transform.position.x, collision.transform.position.y);

        StartCoroutine(StunTime());
       // velocity = Vector3.zero;
        //Debug.Log("vel1 " + rb.velocity + " - " + velocity);
        //rb.AddForce(dir * force, ForceMode2D.Impulse);
        //Debug.Log("vel2 " + rb.velocity + " - " + velocity);
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

    public float GetHealth() {
        return health;
    }

    public void DealDamage(float damage) {
        if(canBeHit){
            health -= damage;
            if (health <= 0) {
                Destroy(gameObject);
            }
        }
    }
}
