using System;
using System.Collections;
using UnityEngine;

public abstract class GunHolder : ItemHolder
{
    public static Action OnGunAim;
    public static Action OnGunFiring;
    public static Action OnGunReload;
    public static Action OnHit;

    public int currentAmunition;
    public int maxAmunition;
    public AmunitionType amunitionType;
    public float forcerCollision = 30;

    public Transform firingPoint;
    
    public bool reload;
    public bool aim;
    public bool firing;

    public abstract override void UsingItem(Inventory inventory);
    public abstract void Firing(Inventory inventory);
    public abstract void Aim();
}
