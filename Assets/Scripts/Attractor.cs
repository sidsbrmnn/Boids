using UnityEngine;

[DisallowMultipleComponent]
public class Attractor : MonoBehaviour
{
    public static Attractor Instance { get; private set; }

    [SerializeField] private Vector3 phase = new(0.5f, 0.4f, 0.1f);
    [SerializeField] private Vector3 range = new(40f, 10f, 40f);

    private void Awake()
    {
        if (Instance && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void FixedUpdate()
    {
        var position = Position;
        position.x = Mathf.Sin(Time.time * phase.x) * range.x;
        position.y = Mathf.Sin(Time.time * phase.y) * range.y;
        position.z = Mathf.Sin(Time.time * phase.z) * range.z;

        Position = position;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public Vector3 Position
    {
        get => transform.position;
        private set => transform.position = value;
    }
}
