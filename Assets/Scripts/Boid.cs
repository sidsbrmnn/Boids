using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TrailRenderer))]
public class Boid : MonoBehaviour
{
    private Rigidbody _rb;
    private Neighborhood _neighborhood;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _neighborhood = GetComponent<Neighborhood>();
    }

    private void Start()
    {
        Velocity = Random.onUnitSphere * Spawner.Instance.Settings.velocity;

        LookAhead();
        Colorize();
    }

    private void FixedUpdate()
    {
        var settings = Spawner.Instance.Settings;
        var sum = Vector3.zero;

        // ___ATTRACTOR___
        var delta = Attractor.Instance.Position - Position;
        if (delta.magnitude > settings.attractPushDistance)
            sum += delta.normalized * settings.attractorPull;
        else
            sum -= delta.normalized * settings.attractorPush;

        // ___COLLISION AVOIDANCE___
        if (_neighborhood.AverageNearPosition != Vector3.zero)
        {
            var avoid = Position - _neighborhood.AverageNearPosition;
            sum += avoid.normalized * settings.collisionAvoidance;
        }

        // ___VELOCITY MATCHING___
        if (_neighborhood.AverageVelocity != Vector3.zero)
            sum += _neighborhood.AverageVelocity.normalized * settings.velocityMatching;

        // ___FLOCK CENTERING___
        if (_neighborhood.AveragePosition != Vector3.zero)
            sum += Vector3.Normalize(_neighborhood.AveragePosition - Position) * settings.flockCentering;

        // ___INTERPOLATE VELOCITY___
        Velocity = Vector3.Lerp(Velocity.normalized, sum.normalized, settings.maxSteerForce) * settings.velocity;

        LookAhead();
    }

    private void LookAhead()
    {
        transform.LookAt(Position + Velocity);
    }

    private void Colorize()
    {
        var color = Random.ColorHSV(0, 1, 0.5f, 1, 0.5f, 1);

        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            r.material.color = color;
        }

        var trail = GetComponent<TrailRenderer>();
        trail.startColor = color;
        color.a = 0f;
        trail.endColor = color;
        trail.endWidth = 0f;
    }

    public Vector3 Position => transform.position;

    public Vector3 Velocity
    {
        get => _rb.velocity;
        private set => _rb.velocity = value;
    }
}
