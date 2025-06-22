using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Ganada : ProjectHolder
{
    public Vector3 offSet;
    public float force = 5f;
    public float timeToExploder;

    [Range(.1f, 20f)]
    public float ranger;

    [Header("Refernces")]

    public GameObject explosionPrefab;

    public override void UsingItem(Inventory inventory)
    {
        Aim(inventory);
    }
    public override void SetHUD(Inventory inventory)
    {
    }
    void Aim(Inventory inventory)
    {
        if (Input.GetKey(input.aim))
        {
            arms.SetBool("Aim", true);

            if (Input.GetKeyUp(input.firing))
                StartCoroutine(Jogar(inventory));
        }
        else
            arms.SetBool("Aim", false);
    }
    IEnumerator Jogar(Inventory inventory)
    {
        var gnInfo = ((ProjectSBJ)itemInfo);
        arms.CrossFade("Firing", .2f);
        
        yield return new WaitForSeconds(gnInfo.timeToFiring);

        var gn = Instantiate(gnInfo.itemPrefab, inventory.holder.transform.position, Quaternion.identity);


        var F = cam.transform.forward + cam.transform.rotation * offSet;
        gn.GetComponent<Rigidbody>().AddForce(F * force, ForceMode.Impulse);
        StartCoroutine(gn.GetComponent<Ganada>().exploder());
    }
    public IEnumerator exploder()
    {
        yield return new WaitForSeconds(timeToExploder);

        var collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = ranger;

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        gameObject.AddComponent<ExplodeGranade>();

        Destroy(gameObject, 1f);
    }
        
}
