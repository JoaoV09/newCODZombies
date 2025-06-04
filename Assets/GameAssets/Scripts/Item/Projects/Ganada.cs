using UnityEngine;

public class Ganada : ProjectHolder
{

    public float force = 5f;
    public override void UsingItem(Inventory inventory)
    {
        if (Input.GetKey(input.aim))
            Aim(inventory);

    }
    void Aim(Inventory inventory)
    {

    }
    void Jogar(Inventory inventory)
    {
        
    }
}
