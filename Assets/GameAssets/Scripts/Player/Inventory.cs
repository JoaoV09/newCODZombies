using System;
using System.Collections;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    public ItemSlot[] itemSlot;
    public currentItem currentSlot;

    [Space(10)]
    [Header("Amuniton Slots")]
    public AmunitionSlots[] amunitionSlot;

    [Header("Collect")]
    [SerializeField] private float distToCollect;
    [SerializeField] private LayerMask colectMask;

    [Header("References")]
    [SerializeField] private Animator arms;
    public GameObject hitTarget;
    public Transform holder;
    [SerializeField] private Camera cam;
    private InputManager ip;

    [Header("Debug State")]
    public bool switchGun { private set; get; }

    private void Start()
    {
        ip = InputManager.instances;
        cam = Camera.main;
        arms = GlobalReferences.instances.arms;
        GunHolder.OnHit += HiMark;
    }

    private void Update()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                if (currentSlot.index != i)
                    StartCoroutine(EquipGun(i));
                
                break;
            }
        }

        UsingItem();
        Interact();

        if (Input.GetKeyDown(ip.drop) && !switchGun)
            DropItemEquiped();
    }

    private void Interact()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * distToCollect);

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, distToCollect, colectMask))
        {
            if (hit.collider.tag == "Interactable")
            {
                var interact = hit.collider.GetComponent<InteractMain>();

                if (interact != null)
                {
                    UIManager.Instance.SetInteractText(interact.textToInterac);
                }
            }
            else
            {
                UIManager.Instance.SetInteractText(null);
            }

            if (!Input.GetKeyDown(ip.collect)) return;

            if (hit.collider.tag == "Item")
            {
                var newGun = hit.collider.GetComponent<ItemHolder>();
                for (int i = 0; i < itemSlot.Length; i++)
                {
                    if (newGun.itemInfo.slotType == itemSlot[i].slotType)
                    {

                        if (itemSlot[i].itemInfo != null)
                        {
                            DropItem(i);

                        }

                        itemSlot[i].itemInfo = newGun.itemInfo;
                        itemSlot[i].item = Instantiate(itemSlot[i].itemInfo.itemPrefab, holder);



                        itemSlot[i].item.GetComponent<ItemHolder>().Infos(newGun.gameObject, itemSlot[i].item);


                        itemSlot[i].item.GetComponent<Rigidbody>().isKinematic = true;
                        itemSlot[i].item.GetComponent<Collider>().enabled = false;

                        itemSlot[i].item.gameObject.SetActive(itemSlot[i].equipe);

                        //if (itemSlot[i].itemInfo != null && currentSlot.index == i)
                        //EquipGun(i);

                        if (i == currentSlot.index)
                            StartCoroutine(EquipGun(i));

                        Destroy(newGun.gameObject);

                        break;
                    }
                }
            }
            else if (hit.collider.tag == "Interactable")
            {
                var interact = hit.collider.GetComponent<InteractMain>();

                if (interact != null)
                {
                    interact.Interact(gameObject);
                }

            }
        }
        else
        {
            UIManager.Instance.SetInteractText(null);
        }
    }
    private IEnumerator EquipGun(int _index)
    {
        var newGun = itemSlot[_index].itemInfo;

        if (newGun != null)
        {
            switchGun = true;

            //animação de guardar
            arms.CrossFade("DesEquipe", .2f);

            var _current = itemSlot[currentSlot.index];

            yield return new WaitForSeconds(_current.itemInfo != null ? _current.itemInfo.timeToSwitch : 1f);
            _current.equipe = false;

            if (_current.itemInfo != null)
                _current.item.SetActive(_current.equipe);

            itemSlot[_index].equipe = true;
            //Destroy(currentSlot.currentPrefab);

            arms.runtimeAnimatorController = newGun.controller;

            //animação de puxar

            currentSlot.index = _index;
            currentSlot.currentPrefab = itemSlot[_index].item;
            currentSlot.itemHolder = currentSlot.currentPrefab.GetComponent<ItemHolder>();


            currentSlot.currentPrefab.SetActive(itemSlot[_index].equipe); 

            yield return new WaitForSeconds(newGun.timeToSwitch);

            switchGun = false;
        }

        yield return null;
    }
    private void UsingItem()
    {
        if (currentSlot.itemHolder == null) return;
        
        if (!switchGun)
        {
            currentSlot.itemHolder.UsingItem(this);
        }
        currentSlot.itemHolder.SetHUD(this);
    }
    private void DropItem(int _index)
    {
        var item = itemSlot[_index];
        item.item.SetActive(true);

        item.item.transform.parent = null;
        item.item.GetComponent<Collider>().enabled = true;
        item.item.GetComponent<Rigidbody>().isKinematic = false;

        item.item = null;
        item.equipe = false;
        item.itemInfo = null;

        currentSlot.itemHolder = null;
        currentSlot.currentPrefab = null;

    }
    private void DropItemEquiped()
    {
        if (itemSlot[currentSlot.index == 0 ? 1 : 0].itemInfo == null) return; 

        var item = itemSlot[currentSlot.index];

        if (item == null) return;

        item.item.SetActive(item.equipe);

        item.item.transform.parent = null;
        item.item.GetComponent<Collider>().enabled = true;
        item.item.GetComponent<Rigidbody>().isKinematic = false;

        item.item = null;
        item.equipe = false;
        item.itemInfo = null;

        currentSlot.currentPrefab = null;
        currentSlot.itemHolder = null;

        StartCoroutine(EquipGun(currentSlot.index == 0 ? 1 : 0));
    }
    public void HiMark()
    {
        StartCoroutine(targetActive());
    }
    public IEnumerator targetActive()
    {
        hitTarget.SetActive(true);
        yield return new WaitForSeconds(.05f);
        hitTarget.SetActive(false);
    }
}