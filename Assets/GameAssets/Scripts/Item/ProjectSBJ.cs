using UnityEngine;
[CreateAssetMenu(fileName = "ProjectSBJ", menuName = "Item/Projects")]
public class ProjectSBJ : ItemSBJ
{
    public int MaxAmount;
    public float timeToSwitch;
    public float timeToFiring;
    public RuntimeAnimatorController controller;
}
