using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class SteeringBehavior {
    
    private ISteering host;
    private float wanderCircleDistance = 300f;
    private float wanderAngle = 100;
    private float angleChange = 0.1f;
    //public GameObject t;

    public SteeringBehavior(ISteering host) {
        this.host = host;
    }

    public Vector3 GoToTarget(Vector3 target, float slowingRadius = 0, float maxTurningRadius = 180) {
        Vector3 velocity = host.GetVelocity();
        Vector3 desiredVelocity = Vector3.Normalize(target - host.GetPosition());
        float distance = Vector3.Distance(host.GetPosition(), target);
        
        if (distance < slowingRadius) {
            desiredVelocity = desiredVelocity * host.GetMaxSpeed() * (distance / slowingRadius);
        } else {
            desiredVelocity = desiredVelocity * host.GetMaxSpeed();
        }
        
       

        //Debug.Log("angle1: " + Vector3.Angle(desiredVelocity, velocity));
        float angle = AngleBetween(target);
        if(angle < 0) {
            angle += 360;
        }
       // Debug.Log(angle + " - " + host.GetEulerRotation().z);

        float diffAngle = Mathf.Min(Mathf.Abs(angle - host.GetEulerRotation().z), 360 - Mathf.Abs(angle - host.GetEulerRotation().z));

        

        //Debug.Log("diff: " + diffAngle);
        //angle += 360;

        //float a = host.GetEulerRotation().z;
        //if (host.GetEulerRotation().z < 180) {
        //    a += 180;
        //} else {
        //    a += 180;
        //}
        //a += 360;


        if (diffAngle > maxTurningRadius) {
            if (IsAngleToRight(angle, host.GetEulerRotation().z)) {
                desiredVelocity = Quaternion.Euler(0, 0, (diffAngle + ((180 - diffAngle) * 2)) + maxTurningRadius) * desiredVelocity;
                //Debug.Log("Left");
            } else {
                desiredVelocity = Quaternion.Euler(0, 0, diffAngle - maxTurningRadius) * desiredVelocity;
                //Debug.Log("Right");
            }
        }

        //t.transform.position = host.GetPosition() + (desiredVelocity * 4);

        Vector3 steering = desiredVelocity - velocity;
        steering /= host.GetMass();
        steering = truncate(steering, host.GetMaxForce());

        velocity = velocity + steering;
        velocity = truncate(velocity, host.GetMaxSpeed());
        return velocity;
    }


    public static bool IsAngleToRight(double angle1, double angle2) {
        double diff = angle2 - angle1;
        if (diff < 0)
            diff += 360;
        return diff > 180;
    }

    bool angle_is_between_angles(float N, float a, float b) {
        N = angle_1to360(N); //normalize angles to be 1-360 degrees
        a = angle_1to360(a);
        b = angle_1to360(b);

        if (a < b)
            return a <= N && N <= b;
        return a <= N || N <= b;
    }


    float angle_1to360(float angle) {
        angle = ((int)angle % 360); //converts angle to range -360 + 360
        if (angle > 0.0)
            return angle;
        else
            return angle + 360.0f;
    }

    public Vector3 FleeTarget(Vector3 target) {
        Vector3 velocity = host.GetVelocity();
        Vector3 desiredVelocity = Vector3.Normalize(target - host.GetPosition()) * host.GetMaxSpeed();
        desiredVelocity = -desiredVelocity;

        Vector3 steering = desiredVelocity - velocity;
        steering /= host.GetMass();
        steering = truncate(steering, host.GetMaxForce());

        velocity = velocity + steering;
        velocity = truncate(velocity, host.GetMaxSpeed());
        return velocity;
    }

    public Vector3 PursuitTarget(ISteering target, float prediction = 10f) {
        return GoToTarget((target.GetPosition() + (target.GetVelocity() * prediction)));
    }

    public Vector3 PursuitTarget(Vector3 targetPosition, Vector3 targetVelocity = default(Vector3), float prediction = 10f) {
        return GoToTarget((targetPosition + (targetVelocity * prediction)));
    }

    public Vector3 Wander(float wAngle = 0f) {
        wanderAngle = wAngle;
        Vector3 velocity = host.GetVelocity();
        Vector3 steering = GetWanderForce(velocity);
        steering /= host.GetMass();
        steering = truncate(steering, host.GetMaxForce());

        velocity = velocity + steering;
        velocity = truncate(velocity, host.GetMaxSpeed());
        return velocity;
    }

    //public Vector3 Wander() {
    //    return Wander();
    //}

    private Vector3 GetWanderForce(Vector3 velocity) {
        Vector3 circleCenter = new Vector3(velocity.x, velocity.y);
        circleCenter.Normalize();
        circleCenter *= wanderCircleDistance;

        Vector3 displacement = new Vector3(0f, -1f);
        displacement *= 3;
        displacement = setAngle(displacement, wanderAngle);
        wanderAngle += Random.Range(0f, 1f) * angleChange - (angleChange * .5f);
        Vector3 wanderForce = circleCenter + displacement;
        return wanderForce;
    }

    float AngleBetween(Vector3 t) {
        t.x = t.x - host.GetPosition().x;
        t.y = t.y - host.GetPosition().y;

        float angle = Mathf.Atan2(t.y, t.x) * Mathf.Rad2Deg;
        return angle;
        //rotation = Quaternion.LookRotation(relativePos);

    }

    private Vector3 setAngle(Vector3 v, float n) {
        float len = v.magnitude;
        v.x = Mathf.Cos(n) * len;
        v.y = Mathf.Sin(n) * len;
        return v;
    }

    private Vector3 truncate(Vector3 v, float n) {
        float i = n / v.magnitude;
        if (i >= 1.0f) {
            i = 1.0f;
        }
        v *= i;
        return v;
    }
}
