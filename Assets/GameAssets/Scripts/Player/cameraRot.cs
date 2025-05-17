using UnityEngine;

public class cameraRot : MonoBehaviour
{
    float rotX;
    float rotY;
    public Transform orient;
    public float sensi;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;    
    }

    void Update()
    {

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensi;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensi;

        rotY += mouseX;

        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, -90, 90);


        transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        orient.rotation = Quaternion.Euler(0, rotY, 0);
    }
}
