using System.Collections;
using UnityEngine;

public class MultShot : GunHolder
{
    float T;
    public int bulletsSpawn = 1;
    public Vector2 bulletRanger;
    public Vector2 bulletRangerAim;

    public Vector2 currentRanger;
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
        var _gun = ((GunSBJ)itemInfo);

        //OnGunAim.Invoke();

        aim = Input.GetKey(input.aim);
        currentRanger = Input.GetKey(input.aim) ? bulletRangerAim : bulletRanger;
        GlobalReferences.instances.arms.SetBool("Aim", Input.GetKey(input.aim));
        cam.GetComponentInParent<CameraRecoil>().SetVaiables(Input.GetKey(input.aim) ? _gun.aimRecoil : _gun.recoil, _gun.snappiness, _gun.returnSpeed);

    }

    public override void Firing(Inventory inventory)
    {
        firing = true;
        inventory.gunSlot[inventory.currentSlot.index].currentAmunition = Mathf.Clamp(inventory.gunSlot[inventory.currentSlot.index].currentAmunition - 1, 0, maxAmunition);

        Instantiate(((GunSBJ)itemInfo).firingPartical, firingPoint);

        GlobalReferences.instances.arms.CrossFade(Input.GetKey(input.aim) ? "Aim_Firing" : "Firing", .05f);
        gunAnimator.CrossFade("Firing", .05f);

        OnGunFiring.Invoke();

        for (int i = 0; i < bulletsSpawn; i++)
        {
            var bulletDir = new Vector3(cam.transform.forward.x + Random.Range(-currentRanger.x, currentRanger.x), cam.transform.forward.y + Random.Range(-currentRanger.y, currentRanger.y), cam.transform.forward.z);

            if (Physics.Raycast(cam.transform.position, bulletDir, out RaycastHit hit, ((GunSBJ)itemInfo).distToFiring, ((GunSBJ)itemInfo).maks))
            {
                for (int x = 0; x < ((GunSBJ)itemInfo).bulletHits.Length; x++)
                {
                    var bulletsHit = ((GunSBJ)itemInfo).bulletHits[x];

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
                }
                else if (hit.collider.tag == "Gun")
                {
                    hit.collider.GetComponent<Rigidbody>().AddForce(cam.transform.forward * forcerCollision, ForceMode.Impulse);
                }


            }
        }
        T = Time.time + ((GunSBJ)itemInfo).timeToFiring;
        firing = false;
    }

    public IEnumerator Reload(Inventory inventory)
    {
        reload = true;

        //OnGunReload.Invoke();

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
