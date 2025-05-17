using System;
using System.Collections;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    [SerializeField] private Gun[] gunSlot;
    [SerializeField] private currentGun currentSlot;

    [Header("Collect")]
    [SerializeField] private float distToCollect;
    [SerializeField] private LayerMask colectMask;

    [Header("References")]
    [SerializeField] private Transform holder;
    [SerializeField] private Camera cam;
    private InputManager ip;

    [Header("Debug State")]
    [SerializeField] private bool switchGun;

    private void Start()
    {
        ip = InputManager.instances;
        cam = Camera.main;
    }

    private void Update()
    {
        for (int i = 0; i < gunSlot.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                StartCoroutine(EquipItem(i));
                break;
            }
        }

        UsingItem();
        Collect();
    }

    private void Collect()
    {
        if (!Input.GetKeyDown(ip.collect)) return;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, distToCollect, colectMask))
        {

            if (hit.collider.tag == "Gun")
            {
                var newGun = hit.collider.GetComponent<GunHolder>();
                for (int i = 0; i < gunSlot.Length; i++)
                {
                    if (((GunSBJ)newGun.itemInfo).gunType == gunSlot[i].gunType)
                    {
                        gunSlot[i].gunInfo = ((GunSBJ)newGun.itemInfo);
                        gunSlot[i].currentAmunition = newGun.currentAmunition;
                        gunSlot[i].maxAmunition = newGun.maxAmunition;
                        gunSlot[i].magazineAmunition = newGun.magazineAmunition;
                        
                        Destroy(newGun.gameObject);

                        break;
                    }
                }
            }
        }

    }
    private IEnumerator EquipItem(int _index)
    {
        var newGun = gunSlot[_index].gunInfo;
        if (newGun != null)
        {
            switchGun = true;

            //animação de guardar

            yield return new WaitForSeconds(.3f);
            
            if (currentSlot.currentPrefab != null)
                Destroy(currentSlot.currentPrefab);

            //animação de puxar

            currentSlot.index = _index;
            currentSlot.currentPrefab = Instantiate(newGun.itemPrefab, holder);
            currentSlot.GunHolder = currentSlot.currentPrefab.GetComponent<GunHolder>();

            currentSlot.currentPrefab.GetComponent<Rigidbody>().isKinematic = true;
            currentSlot.currentPrefab.GetComponent<Collider>().enabled = false;

            yield return new WaitForSeconds(newGun.timeToswitch);

            switchGun = false;
        }
        yield return null;
    }
    private void UsingItem()
    {
        if (currentSlot.GunHolder == null) return;

        currentSlot.GunHolder.UsingItem();
    }

}


[Serializable]
public class Gun 
{
    public GunSBJ gunInfo;
    public gunType gunType;

    [Space(10)]
    public int currentAmunition;
    public int maxAmunition;
    public int magazineAmunition;
}
[Serializable]
public class currentGun
{
    public GameObject currentPrefab;
    public GunHolder GunHolder;
    public int index;
}

public enum gunType { None, pistol, rifle }



