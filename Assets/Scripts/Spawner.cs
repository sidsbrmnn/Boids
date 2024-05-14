using UnityEngine;

[DisallowMultipleComponent]
public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }

    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform anchor;
    [SerializeField] private int count = 100;
    [SerializeField] private float spawnRadius = 100f;
    [SerializeField] private float spawnInterval = 0.1f;

    [SerializeField] private BoidSettings settings;

    private int _spawnedCount;

    private void Awake()
    {
        if (Instance && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        InstantiateBoid();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void InstantiateBoid()
    {
        var position = Random.insideUnitSphere * spawnRadius;
        Instantiate(prefab, position, Quaternion.identity, anchor);

        _spawnedCount++;
        if (_spawnedCount < count)
            Invoke(nameof(InstantiateBoid), spawnInterval);
    }

    public BoidSettings Settings => settings;
}
