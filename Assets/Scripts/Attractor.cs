using UnityEngine;

public class Attractor : MonoBehaviour
{
    public static Vector3 Position = Vector3.zero;

    [Header("Inscribed")]
    public Vector3 range = new(40, 10, 40);
    public Vector3 phase = new(0.5f, 0.4f, 0.1f);

    private void FixedUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Sin(phase.x * Time.time) * range.x;
        pos.y = Mathf.Sin(phase.y * Time.time) * range.y;
        pos.z = Mathf.Sin(phase.z * Time.time) * range.z;

        transform.position = pos;
        Position = pos;
    }
}
