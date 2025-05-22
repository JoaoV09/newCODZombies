using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences instances;

    [Header("References")]
    public Animator arms;
    public PlayerMovementAdvanced playMove;

    private void Awake()
    {
        instances = this;
    }
}
