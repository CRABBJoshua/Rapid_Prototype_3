using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : ScriptableObject
{
    [Header("General")]
    public float minAngleToRotate;
    public float minSpeedToSlide;
    public float minAngleToSlide;
    public float minAngleToFall;
    public float pushBack;
    public float diePushUp;
    public float invincibleTime;
    public float controlLockTime;
    public float topSpeed;
    public float maxSpeed;

    [Header("Ground")]
    public float acceleration;
    public float deceleration;
    public float friction;
    public float slope;
    public float minSpeedToBrake;
    public float turnSpeed;

    [Header("Air")]
    public float airAcceleration;
    public float gravity;

    [Header("Jump")]
    public float minJumpHeight;
    public float maxJumpHeight;

    [Header("Roll")]
    public float minSpeedToRoll;
    public float minSpeedToUnroll;
    public float rollDeceleration;
    public float rollFriction;
    public float slopeRollUp;
    public float slopeRollDown;
}
