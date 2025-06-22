using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Item")]
public class ItemSBJ : ScriptableObject
{
    public string itemName;
    public GameObject itemPrefab;
    public Sprite itemSprite;

    public SlotType slotType;
    
    public float timeToSwitch;

    public RuntimeAnimatorController controller;
    
}
