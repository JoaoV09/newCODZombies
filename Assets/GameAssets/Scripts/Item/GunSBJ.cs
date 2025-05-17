using UnityEngine;

[CreateAssetMenu(fileName = "GunSBJ", menuName = "Scriptable Objects/GunSBJ")]
public class GunSBJ : ItemSBJ
{
    public int damager;

    public float distToFiring;
    public LayerMask maks;

    public float timeToReload;
    public float timeToswitch;

    public gunType gunType;

    public GameObject particalCollider;
    public GameObject bulletHole;
    public GameObject firingPartical;
    public AudioClip firingSound;
}
