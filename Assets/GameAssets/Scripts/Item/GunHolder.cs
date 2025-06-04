using System;
using UnityEngine;

public abstract class GunHolder : ItemHolder
{
    public static Action OnGunAim;
    public static Action OnGunFiring;
    public static Action OnGunReload;

    public int currentAmunition;
    public int maxAmunition;
    public int magazineAmunition;

    public float forcerCollision = 30;

    public Transform firingPoint;
    public Animator gunAnimator;

    [SerializeField] protected Animator arms;
    [SerializeField] protected Camera cam;

    public bool reload;
    public bool aim;
    public bool firing;
    private void Start()
    {
        arms = GlobalReferences.instances.arms;
        cam = Camera.main;
        input = InputManager.instances;
        gunAnimator = GetComponent<Animator>();
    }
    public abstract override void UsingItem(Inventory inventory);
    public abstract void Firing(Inventory inventory);
    public abstract void Aim();
}
