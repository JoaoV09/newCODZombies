using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences instances;

    [Header("References")]
    public Animator arms;
    public PlayerMovementAdvanced playMove;
    public GameObject hitPopups;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instances = this;
    }
}
