using UnityEngine;

public class CopyPOs : MonoBehaviour
{
    [SerializeField] private Transform pos;
    [SerializeField] private Vector3 Offset;

    private void LateUpdate()
    {
        transform.position = pos.position + pos.rotation * Offset;
    }
}
