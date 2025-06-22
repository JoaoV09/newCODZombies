using UnityEngine;

public abstract class ItemHolder : MonoBehaviour
{
    public ItemSBJ itemInfo;

    [SerializeField] protected InputManager input;
    [SerializeField] protected Animator itemAnimator;
    [SerializeField] protected Animator arms;
    [SerializeField] protected Camera cam;

    private void Start()
    {
        input = InputManager.instances;
        itemAnimator = GetComponent<Animator>();
        arms = GlobalReferences.instances.arms;
        cam = Camera.main;
    }

    public abstract void UsingItem(Inventory inventory);
    public abstract void SetHUD(Inventory inventory);
    
    public abstract void Infos(GameObject oldObj, GameObject NewObj);

}
