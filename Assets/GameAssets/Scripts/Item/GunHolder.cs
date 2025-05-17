using UnityEngine;

public abstract class GunHolder : ItemHolder
{
    public int currentAmunition;
    public int maxAmunition;
    public int magazineAmunition;

    public abstract override void UsingItem();
    public abstract void Firing();
    public abstract void Aim();
}
