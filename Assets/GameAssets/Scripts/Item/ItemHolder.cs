using UnityEngine;

public abstract class ItemHolder : MonoBehaviour
{
    public ItemSBJ itemInfo;

    public abstract void UsingItem(Inventory inventory);
}
