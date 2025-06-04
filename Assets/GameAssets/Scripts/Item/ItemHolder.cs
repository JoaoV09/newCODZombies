using UnityEngine;

public abstract class ItemHolder : MonoBehaviour
{
    public ItemSBJ itemInfo;
    public InputManager input;
    private void Awake()
    {
        input = InputManager.instances;
        
    }

    public abstract void UsingItem(Inventory inventory);
}
