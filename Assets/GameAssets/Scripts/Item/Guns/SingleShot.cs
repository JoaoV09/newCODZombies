using System.Collections;
using UnityEngine;

public class SingleShot : GunHolder
{
    float T;
    public override void UsingItem(Inventory inventory)
    {
        if ((((GunSBJ)itemInfo).firingType == FiringType.automatic ? Input.GetKey(input.firing) : Input.GetKeyDown(input.firing)) && T < Time.time && inventory.gunSlot[inventory.currentSlot.index].currentAmunition > 0 && !reload)
            Firing(inventory);

        if (Input.GetKeyDown(input.reload) && inventory.gunSlot[inventory.currentSlot.index].currentAmunition < maxAmunition)
            StartCoroutine(Reload(inventory));

        Aim();
    }
    public override void Aim()
    {
        var veloc = GlobalReferences.instances.playMove.GetComponent<Rigidbody>().linearVelocity;
        veloc.y = 0;

        GlobalReferences.instances.arms.SetBool("Aim", veloc.magnitude > 3.5 ? false : Input.GetKey(input.aim));
    }

    public override void Firing(Inventory inventory)
    {
        inventory.gunSlot[inventory.currentSlot.index].currentAmunition = Mathf.Clamp(inventory.gunSlot[inventory.currentSlot.index].currentAmunition - 1, 0, maxAmunition);

        Instantiate(((GunSBJ)itemInfo).firingPartical, firingPoint);

        GlobalReferences.instances.arms.CrossFade(Input.GetKey(input.aim) ? "Aim_Firing" : "Firing", .1f);
        gunAnimator.CrossFade("Firing", .1f);

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
                //var enemy = hit.collider.GetComponent<StateParty>();
                //enemy.takeDamager((GunSBJ)itemInfo).damager);
            }else if (hit.collider.tag == "Gun")
            {
                hit.collider.GetComponent<Rigidbody>().AddForce(cam.transform.forward * 10, ForceMode.Impulse);
            }


        }
            T = Time.time + ((GunSBJ)itemInfo).TimeToFiring;
    }
    public IEnumerator Reload(Inventory inventory)
    {
        reload = true;
        GlobalReferences.instances.arms.CrossFade("Reload", .2f);
        gunAnimator.CrossFade("Reload", .2f);

        yield return new WaitForSeconds(((GunSBJ)itemInfo).timeToReload);

        var current = inventory.gunSlot[inventory.currentSlot.index];
        var qnt = maxAmunition - current.currentAmunition;

        if (qnt > 0 && current.magazineAmunition >= qnt)
        {
            current.currentAmunition = current.maxAmunition;
            current.magazineAmunition -= qnt;
        }
        else if (current.magazineAmunition > 0)
        {
            current.currentAmunition = current.magazineAmunition;
            current.magazineAmunition = 0;
        }

        reload = false;
        yield return null;
    }
}
