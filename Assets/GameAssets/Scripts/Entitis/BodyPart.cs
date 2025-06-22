using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] private ZombieBase takeDamager;
    [SerializeField] private float critical;
    private void Start()
    {
        takeDamager = GetComponentInParent<ZombieBase>();
    }

    public void TakeDamager(int Damager)
    {
        takeDamager.Takedamager((int)(Damager * critical));
    }

    public void SpawnPopups(Vector3 pos, int Damager, Transform look)
    {
        var pop = HitPopups.Create(pos, (int)(Damager * critical));
        pop.transform.LookAt(look.position);
    }
}
