using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GunSBJ", menuName = "Item/Guns")]
public class GunSBJ : ItemSBJ
{
    public int damager;

    public float distToFiring;
    public LayerMask maks;

    [Space(10)]
    public float timeToReload;
    public float timeToSwitch;
    public float timeToFiring;

    [Space(10)]
    public Vector3 recoil;
    public Vector3 aimRecoil;

    public float snappiness;
    public float returnSpeed;
    
    [Space(10)]
    public gunType gunType;
    public FiringType firingType;

    [Space(10)]
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
