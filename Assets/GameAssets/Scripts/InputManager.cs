using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instances;

    [Header("inventory Settings")]
    public KeyCode collect = KeyCode.E;
    public KeyCode interact = KeyCode.E;
    public KeyCode drop = KeyCode.Q;

    [Space(10)]
    public KeyCode firing = KeyCode.Mouse0;
    public KeyCode aim = KeyCode.Mouse1;
    public KeyCode reload = KeyCode.R;
    
    private void Awake()
    {
        instances = this;
    }

}
