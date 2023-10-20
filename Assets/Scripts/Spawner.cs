using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoidSettings
{
    public int velocity = 32;
    public int neighborDistance = 10;
    public int nearDistance = 4;
    public int attractPushDistance = 5;

    [Header("There \"incluences\" are floats, usually from [0..4]")]
    public float velMatching = 1.5f;
    public float flockCentering = 1f;
    public float nearAvoid = 2f;
    public float attractPull = 1f;
    public float attractPush = 20f;

    [Header("This determines how quickly Boids can turn and is [0..1]")]
    public float velocityEasing = 0.03f;
}

public class Spawner : MonoBehaviour
{
    public static List<Boid> Boids;
    public static BoidSettings Settings;

    [Header("Inscribed: Settings for Spawning Boids")]
    public GameObject boidPrefab;
    public Transform boidAnchor;
    public int numBoids = 100;
    public float spawnRadius = 100f;
    public float spawnDelay = 0.1f;

    [Header("Inscribed: Settings for Spawning Boids")]
    public BoidSettings boidSettings;

    private void Awake()
    {
        Settings = boidSettings;

        Boids = new();
        InstantiateBoid();
    }

    void InstantiateBoid()
    {
        GameObject go = Instantiate(boidPrefab);
        go.transform.position = Random.insideUnitSphere * spawnRadius;

        Boid boid = go.GetComponent<Boid>();
        boid.transform.SetParent(boidAnchor);
        Boids.Add(boid);

        if (Boids.Count < numBoids)
        {
            Invoke(nameof(InstantiateBoid), spawnDelay);
        }
    }
}
