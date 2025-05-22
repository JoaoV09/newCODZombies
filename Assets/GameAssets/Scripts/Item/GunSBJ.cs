using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GunSBJ", menuName = "Scriptable Objects/GunSBJ")]
public class GunSBJ : ItemSBJ
{
    public int damager;

    public float distToFiring;
    public LayerMask maks;

    public float timeToReload;
    public float timeToSwitch;
    public float TimeToFiring;

    public gunType gunType;
    public FiringType firingType;

    public BulletsHitCollider[] bulletHits;
    public GameObject firingPartical;
    public AudioClip firingSound;
    public RuntimeAnimatorController controller;
}

public enum FiringType { automatic, semi }
[Serializable]
public class BulletsHitCollider
{
    public string tag;
    public LayerMask layer;
    public GameObject bulletHole;
    public GameObject particalCollider;
}
