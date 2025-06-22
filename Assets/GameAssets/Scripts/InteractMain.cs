using UnityEngine;

public abstract class InteractMain : MonoBehaviour
{
    public string textToInterac;
    public abstract void Interact(GameObject other); 

}
