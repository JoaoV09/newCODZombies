using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    public Gun[] gunSlot;
    public currentGun currentSlot;

    [Header("Collect")]
    [SerializeField] private float distToCollect;
    [SerializeField] private LayerMask colectMask;


    [Header("UI Slots")]
    [SerializeField] private Image gunSprite;
    [SerializeField] private TextMeshProUGUI gunText;

    [Header("References")]
    [SerializeField] private Animator arms;
    [SerializeField] private Transform holder;
    [SerializeField] private Camera cam;
    private InputManager ip;

    [Header("Debug State")]
    public bool switchGun { private set; get; }

    private void Start()
    {
        ip = InputManager.instances;
        cam = Camera.main;
        arms = GlobalReferences.instances.arms;
        SetIU();
    }

    private void Update()
    {
        for (int i = 0; i < gunSlot.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                StartCoroutine(EquipGun(i));
                break;
            }
        }

        UsingItem();
        Collect();
        SetIU();
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
    private IEnumerator EquipGun(int _index)
    {
        if (currentSlot.index != _index)
        {
            var newGun = gunSlot[_index].gunInfo;

            if (newGun != null)
            {
                switchGun = true;

                //animação de guardar
                arms.CrossFade("DesEquipe", .2f);

                var _current = gunSlot[currentSlot.index].gunInfo;

                yield return new WaitForSeconds(_current != null ? _current.timeToSwitch : 1f);

                if (currentSlot.currentPrefab != null)
                    Destroy(currentSlot.currentPrefab);

                arms.runtimeAnimatorController = newGun.controller;

                //animação de puxar

                currentSlot.index = _index;
                currentSlot.currentPrefab = Instantiate(newGun.itemPrefab, holder);
                currentSlot.GunHolder = currentSlot.currentPrefab.GetComponent<GunHolder>();

                currentSlot.currentPrefab.GetComponent<Rigidbody>().isKinematic = true;
                currentSlot.currentPrefab.GetComponent<Collider>().enabled = false;


                var _recoil = cam.GetComponentInParent<CameraRecoil>();

                _recoil.SetVaiables(_current.recoil, _current.snappiness, _current.returnSpeed);

                yield return new WaitForSeconds(newGun.timeToSwitch);

                switchGun = false;
            }
        }
        yield return null;
    }
    private void UsingItem()
    {
        if (currentSlot.GunHolder == null || switchGun) return;

        currentSlot.GunHolder.UsingItem(this);
    }

    private void SetIU()
    {
        if (!currentSlot.GunHolder) return;

        gunSprite.sprite = currentSlot.GunHolder.itemInfo.itemSprite;
        gunText.text = $"{gunSlot[currentSlot.index].currentAmunition} / {gunSlot[currentSlot.index].magazineAmunition}";
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

[Serializable]
public class Projects 
{
    public ProjectHolder project;
    public int amount;
}


