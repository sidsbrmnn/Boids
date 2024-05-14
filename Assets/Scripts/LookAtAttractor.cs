using UnityEngine;

[RequireComponent(typeof(Camera))]
public class LookAtAttractor : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(Attractor.Instance.Position);
    }
}
