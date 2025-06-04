using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    [SerializeField] private Vector3 currentRotation;
    [SerializeField] private Vector3 targetRotation;

    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    [SerializeField] private float snappiness;
    [SerializeField] private float retunSpeed;

    private void Awake()
    {
        SetVaiables(Vector3.zero, 0, 0);
        GunHolder.OnGunFiring += RecoilFire;
    }

    private void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, retunSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
    public void RecoilFire()
    {
        targetRotation += new Vector3(-recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
    public void SetVaiables(Vector3 _recoil, float _snappiness, float _retunSpeed)
    {
        recoilX = _recoil.x;
        recoilY = _recoil.y;
        recoilZ = _recoil.z;

        snappiness = _snappiness;
        retunSpeed = _retunSpeed;
    }
    private void OnDestroy()
    {
        GunHolder.OnGunFiring -= RecoilFire;    
    }
}
