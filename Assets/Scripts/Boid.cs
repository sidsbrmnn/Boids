using UnityEngine;

public class Boid : MonoBehaviour
{
    private Neighborhood neighborhood;
    private new Rigidbody rigidbody;

    private void Awake()
    {
        neighborhood = GetComponent<Neighborhood>();
        rigidbody = GetComponent<Rigidbody>();

        Velocity = Random.onUnitSphere * Spawner.Settings.velocity;

        LookAhead();
        Colorize();
    }

    private void FixedUpdate()
    {
        BoidSettings settings = Spawner.Settings;

        Vector3 sumVelocity = Vector3.zero;

        // ___ATTRACTOR___
        Vector3 delta = Attractor.Position - Position;
        if (delta.magnitude > settings.attractPushDistance)
        {
            sumVelocity += delta.normalized * settings.attractPull;
        }
        else
        {
            sumVelocity -= delta.normalized * settings.attractPush;
        }

        // ___COLLISION AVOIDANCE___
        if (neighborhood.AverageNearPosition != Vector3.zero)
        {
            Vector3 avoidVelocity = Position - neighborhood.AverageNearPosition;
            avoidVelocity.Normalize();
            sumVelocity += avoidVelocity * settings.nearAvoid;
        }

        // ___VELOCITY MATCHING___
        if (neighborhood.AverageVelocity != Vector3.zero)
        {
            Vector3 alignVelocity = neighborhood.AverageVelocity;
            alignVelocity.Normalize();
            sumVelocity += alignVelocity * settings.velMatching;
        }

        // ___FLOCK CENTERING___
        if (neighborhood.AveragePosition != Vector3.zero)
        {
            Vector3 centerVelocity = neighborhood.AveragePosition -
                transform.position;
            centerVelocity.Normalize();
            sumVelocity += centerVelocity * settings.flockCentering;
        }

        // ___INTERPOLATE VELOCITY___
        sumVelocity.Normalize();
        Velocity = Vector3.Lerp(Velocity.normalized, sumVelocity,
            settings.velocityEasing);
        Velocity *= settings.velocity;

        LookAhead();
    }

    void LookAhead()
    {
        transform.LookAt(Position + rigidbody.velocity);
    }

    void Colorize()
    {
        Color color = Random.ColorHSV(0, 1, 0.5f, 1, 0.5f, 1);

        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = color;
        }

        TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.startColor = color;
        color.a = 0;
        trailRenderer.endColor = color;
        trailRenderer.endWidth = 0;
    }

    public Vector3 Position
    {
        get { return transform.position; }
        private set { transform.position = value; }
    }

    public Vector3 Velocity
    {
        get { return rigidbody.velocity; }
        private set { rigidbody.velocity = value; }
    }
}
