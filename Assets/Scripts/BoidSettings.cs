using System;
using UnityEngine;

[Serializable]
public class BoidSettings
{
    public int velocity = 32;
    public int neighborDistance = 10;
    public int nearDistance = 4;
    public int attractPushDistance = 5;

    public float velocityMatching = 1.5f;
    public float flockCentering = 1f;
    public float collisionAvoidance = 2f;
    public float attractorPull = 1f;
    public float attractorPush = 20f;

    [Range(0, 1)] public float maxSteerForce = 0.03f;
}
