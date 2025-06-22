using System;
using UnityEngine;

#region Class

#region Inventory Class
[Serializable]
public class ItemSlot
{
    public ItemSBJ itemInfo;
    public GameObject item;
    public SlotType slotType;

    [Space(10)]
    public int amount;
    public bool equipe;
}
[Serializable]
public class currentItem
{
    public GameObject currentPrefab;
    public ItemHolder itemHolder;
    public int index;
}
[Serializable]
public class AmunitionSlots
{
    public AmunitionType amunitionType;
    public int amunition;
}
[Serializable]
public class BulletsHitCollider
{
    public string tag;
    public LayerMask layer;
    public GameObject bulletHole;
    public GameObject particalCollider;
}
#endregion

#region UI
[Serializable]
public class HudSelect 
{
    public GameObject hud;
    public string hudName;
    public bool select;
}
#endregion


#endregion

#region Enums
public enum AmunitionType { None, Pistol, Rifle, Shotiguns }
public enum SlotType { None, Pistol, Rifle, Projects, Medkit }
#endregion