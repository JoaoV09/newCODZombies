using System;
using System.Collections;
using UnityEngine;
public class SingleShot : GunHolder
{
    float T;

    public override void UsingItem(Inventory inventory)
    {

        if (input == null) return;

        if ((((GunSBJ)itemInfo).firingType == FiringType.automatic ? Input.GetKey(input.firing) : Input.GetKeyDown(input.firing)) && T < Time.time && currentAmunition > 0 && !reload)
            Firing(inventory);
        
        if (Input.GetKeyDown(input.reload) && currentAmunition < maxAmunition && !reload)
            StartCoroutine(Reload(inventory));

        Aim();
    }
    public override void Aim()
    {
        var veloc = GlobalReferences.instances.playMove.GetComponent<Rigidbody>().linearVelocity;
        veloc.y = 0;
        var _gun = ((GunSBJ)itemInfo);

            //OnGunAim.Invoke();
        
        aim = Input.GetKey(input.aim);
        GlobalReferences.instances.arms.SetBool("Aim", Input.GetKey(input.aim));
        cam.GetComponentInParent<CameraRecoil>().SetVaiables(Input.GetKey(input.aim) ? _gun.aimRecoil : _gun.recoil, _gun.snappiness, _gun.returnSpeed);

    }
    public override void Firing(Inventory inventory)
    {
        firing = true;

        currentAmunition = Mathf.Clamp(currentAmunition - 1, 0, maxAmunition);

        Instantiate(((GunSBJ)itemInfo).firingPartical, firingPoint);

        GlobalReferences.instances.arms.CrossFade(Input.GetKey(input.aim) ? "Aim_Firing" : "Firing", .05f);
        itemAnimator.CrossFade("Firing",  .05f);

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, ((GunSBJ)itemInfo).distToFiring, ((GunSBJ)itemInfo).maks))
        {

            for (int i = 0; i < ((GunSBJ)itemInfo).bulletHits.Length; i++)
            {
                var bulletsHit = ((GunSBJ)itemInfo).bulletHits[i];

                if (hit.collider.tag == bulletsHit.tag)
                {
                    if (bulletsHit.bulletHole)
                        Instantiate(bulletsHit.bulletHole, hit.point, Quaternion.LookRotation(hit.normal));

                    if (bulletsHit.particalCollider)
                        Instantiate(bulletsHit.particalCollider, hit.point, Quaternion.LookRotation(hit.normal));

                    break;
                }
            }

            if (hit.collider.tag == "Enemy")
            {
                var part = hit.collider.GetComponent<BodyPart>();
                part?.TakeDamager(((GunSBJ)itemInfo).damager);
                part?.SpawnPopups(hit.point, ((GunSBJ)itemInfo).damager, transform);
                OnHit.Invoke();

            }else if (hit.collider.tag == "Item")
            {
                hit.collider.GetComponent<Rigidbody>().AddForce(cam.transform.forward * forcerCollision, ForceMode.Impulse);
            }


        }

        OnGunFiring.Invoke();
        T = Time.time + ((GunSBJ)itemInfo).timeToFiring;
        firing = false;
    }

    public IEnumerator Reload(Inventory inventory)
    {
        reload = true;

        //OnGunReload.Invoke();
        int magazineAmunition = 0;

        for (int d = 0; d < inventory.amunitionSlot.Length; d++)
        {
            if (inventory.amunitionSlot[d].amunitionType == amunitionType)
            {
                magazineAmunition = d;
                break;
            }
        }

        GlobalReferences.instances.arms.CrossFade("Reload", .2f);
        itemAnimator.CrossFade("Reload", .2f);

        yield return new WaitForSeconds(((GunSBJ)itemInfo).timeToReload);

        var qnt = maxAmunition - currentAmunition;

        if (qnt > 0 && inventory.amunitionSlot[magazineAmunition].amunition >= qnt)
        {
            currentAmunition = maxAmunition;
            inventory.amunitionSlot[magazineAmunition].amunition -= qnt;
        }
        else if (magazineAmunition > 0)
        {
            currentAmunition = inventory.amunitionSlot[magazineAmunition].amunition;
            inventory.amunitionSlot[magazineAmunition].amunition = 0;
        }

        reload = false;
        yield return null;
    }

    public override void SetHUD(Inventory inventory)
    {

        for (int i =0; i < inventory.amunitionSlot.Length; i++)
        {
            if (inventory.amunitionSlot[i].amunitionType == amunitionType)
            {
                UIManager.Instance.SetHUD(itemInfo.itemSprite, $"{currentAmunition} / {inventory.amunitionSlot[i].amunition}");
                break;
            }
        }

    }

    public override void Infos(GameObject oldObj, GameObject NewObj)
    {
        var oldGun = oldObj.GetComponent<GunHolder>();
        var newGun = NewObj.GetComponent<GunHolder>();

        newGun.currentAmunition = oldGun.currentAmunition;
    }
}
