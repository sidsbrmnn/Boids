using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SphereCollider))]
public class Neighborhood : MonoBehaviour
{
    private SphereCollider _collider;
    private List<Boid> _neighbors;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _neighbors = new List<Boid>();
    }

    private void Start()
    {
        _collider.radius = Spawner.Instance.Settings.neighborDistance * 0.5f;
    }

    private void FixedUpdate()
    {
        var radius = Spawner.Instance.Settings.neighborDistance * 0.5f;
        if (Mathf.Approximately(_collider.radius, radius))
            _collider.radius = radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Boid b)) return;
        if (!_neighbors.Contains(b)) _neighbors.Add(b);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Boid b)) _neighbors.Remove(b);
    }

    public Vector3 AveragePosition => _neighbors.Count == 0
        ? Vector3.zero
        : _neighbors.Select(b => b.Position).Aggregate((acc, p) => acc + p) / _neighbors.Count;

    public Vector3 AverageVelocity => _neighbors.Count == 0
        ? Vector3.zero
        : _neighbors.Select(b => b.Velocity).Aggregate((acc, v) => acc + v) / _neighbors.Count;

    public Vector3 AverageNearPosition
    {
        get
        {
            var near = _neighbors.Where(b =>
                Vector3.Distance(b.Position, transform.position) <= Spawner.Instance.Settings.nearDistance).ToList();

            return near.Count == 0
                ? Vector3.zero
                : near.Select(b => b.Position).Aggregate((acc, p) => acc + p) / near.Count;
        }
    }
}
