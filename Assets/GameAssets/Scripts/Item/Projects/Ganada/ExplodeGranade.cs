using UnityEditorInternal;
using UnityEngine;

public class ExplodeGranade : MonoBehaviour
{
    [SerializeField] private LayerMask mask = 0 | 3;
    [SerializeField] private float force = 20f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            var pos = other.transform.position - transform.position ;

            if (Physics.Raycast(transform.position, pos, out RaycastHit hit, mask))
            {
                if (hit.collider.GetComponent<Rigidbody>())
                {
                    hit.collider.GetComponent<Rigidbody>().AddForce(pos * force, ForceMode.Impulse);
                    hit.collider.GetComponent<TakeDamager>()?.Takedamager(500);
                }

                var part = hit.collider.GetComponent<BodyPart>();
                if (part)
                {
                    part.TakeDamager(100);
                    part.SpawnPopups(other.transform.position, 100, GlobalReferences.instances.playMove.transform);
                }
            }
        }
    }
}
