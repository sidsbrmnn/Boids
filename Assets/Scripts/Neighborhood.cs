using System.Collections.Generic;
using UnityEngine;

public class Neighborhood : MonoBehaviour
{
    [Header("Dynamic")]
    public List<Boid> neighbors;
    private SphereCollider sphereCollider;

    private void Start()
    {
        neighbors = new();
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = Spawner.Settings.neighborDistance / 2;
    }

    private void FixedUpdate()
    {
        float nearRadius = Spawner.Settings.neighborDistance / 2;
        if (!Mathf.Approximately(sphereCollider.radius, nearRadius))
        {
            sphereCollider.radius = nearRadius;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Boid>(out var boid))
        {
            if (!neighbors.Contains(boid))
            {
                neighbors.Add(boid);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Boid>(out var boid))
        {
            neighbors.Remove(boid);
        }
    }

    public Vector3 AveragePosition
    {
        get
        {
            Vector3 average = Vector3.zero;
            foreach (Boid boid in neighbors)
            {
                average += boid.Position;
            }
            if (neighbors.Count > 0)
            {
                average /= neighbors.Count;
            }

            return average;
        }
    }

    public Vector3 AverageVelocity
    {
        get
        {
            Vector3 average = Vector3.zero;
            foreach (Boid boid in neighbors)
            {
                average += boid.Velocity;
            }
            if (neighbors.Count > 0)
            {
                average /= neighbors.Count;
            }

            return average;
        }
    }

    public Vector3 AverageNearPosition
    {
        get
        {
            Vector3 average = Vector3.zero;
            int count = 0;
            foreach (Boid boid in neighbors)
            {
                Vector3 delta = boid.Position - transform.position;
                if (delta.magnitude <= Spawner.Settings.nearDistance) {
                    average += boid.Position;
                    count++;
                }
            }
            if (count > 0)
            {
                average /= count;
            }

            return average;
        }
    }
}
