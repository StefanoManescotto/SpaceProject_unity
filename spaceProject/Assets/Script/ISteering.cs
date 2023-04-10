using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISteering {
    public Vector3 GetPosition();
    public Vector3 GetVelocity();
    public float GetMaxSpeed();
    public float GetMass();
    public float GetMaxForce();
    public Vector3 GetEulerRotation();
}
