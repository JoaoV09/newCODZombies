using UnityEngine;

public class EntitisSpawnLocate : InteractMain
{
    [SerializeField] private GameObject entitiPrefab;
    [SerializeField] private Vector3 poitionSpawn;
    [SerializeField] private bool IsDebug;
    public override void Interact(GameObject other)
    {
        Instantiate(entitiPrefab, poitionSpawn, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        if (entitiPrefab == null || !IsDebug) return;

        Gizmos.color = Color.green;
        var size = new Vector3(3, 3, 3);
        var pos = new Vector3(poitionSpawn.x, poitionSpawn.y + (size.y / 2), poitionSpawn.z);
        Gizmos.DrawCube(pos, size);
    }
}
