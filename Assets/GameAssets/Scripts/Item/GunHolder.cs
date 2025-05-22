using UnityEngine;

public abstract class GunHolder : ItemHolder
{
    public int currentAmunition;
    public int maxAmunition;
    public int magazineAmunition;

    public Transform firingPoint;
    public Animator gunAnimator;

    [SerializeField] protected InputManager input;
    [SerializeField] protected Animator arms;
    [SerializeField] protected Camera cam;

    public bool reload;
    private void Start()
    {
        input = InputManager.instances;
        arms = GlobalReferences.instances.arms;
        cam = Camera.main;

        gunAnimator = GetComponent<Animator>();
    }
    public abstract override void UsingItem(Inventory inventory);
    public abstract void Firing(Inventory inventory);
    public abstract void Aim();
}
