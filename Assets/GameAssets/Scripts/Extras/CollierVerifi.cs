using Unity.VisualScripting;
using UnityEngine;

public class CollierVerifi : MonoBehaviour
{
    [SerializeField] private int damager;
    [SerializeField] private Collider Collider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<TakeDamager>()?.Takedamager(damager);
            Collider.enabled = false;
        }
    }
}
