using UnityEngine;

public class RecoilScript : MonoBehaviour
{
    private Quaternion targetRotation;
    public Transform playerCam;
    
    [Header ("Recoil and Spread")]
    public float recoilX;
    public float recoilY;
    public float recoilZ;

    public float aimRecoilX;
    public float aimRecoilY;
    public float aimRecoilZ;

    public float snappiness;
    public float returnSpeed;

    void Start()
    {

    }

    void Update()
    {
        transform.localRotation = Quaternion.Slerp(targetRotation, playerCam.rotation, returnSpeed);
        targetRotation = playerCam.rotation;
    }

    public void RecoilFire()
    {
        targetRotation = new Quaternion(playerCam.eulerAngles.x + recoilX,playerCam.eulerAngles.y + Random.Range(-recoilY, recoilY),playerCam.eulerAngles.z + Random.Range(-recoilZ, recoilZ), 1);
        Quaternion.Slerp(transform.localRotation, targetRotation, snappiness);
        Debug.Log(targetRotation);
    }
}
