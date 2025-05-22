using UnityEngine;

public class TimerToDestroy : MonoBehaviour
{
    [SerializeField] private float time;
    private void Start()
    {
        Destroy(gameObject, time);
    }
}
