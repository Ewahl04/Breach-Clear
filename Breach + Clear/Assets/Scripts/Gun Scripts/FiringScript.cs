using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringScript : MonoBehaviour
{
    [Header("Stats")]
    public int damage;
    public float fireRate, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    [Header("Recoil and Spread")]
    public float spread;

    public float recoilX;
    public float recoilY;

    public float aimRecoilX;
    public float aimRecoilY;

    public float snappiness;
    public float returnSpeed;

    //bools 
    bool shooting, readyToShoot, reloading;

    [Header("References")]
    public Camera fpsCam;
    public RaycastHit rayHit;
    public PlayerCam Recoil_Script;

    [Header("Graphics")]
    public CameraShake camShake;
    public float camShakeMagnitude, camShakeDuration;
    
    [Header("Muzzle Flash and Impacts")]
    public ParticleSystem muzzleFlash;
    public GameObject impactConcrete, impactDirt, impactBrick, impactFoliage, impactGlass, impactMetal, impactPlaster, impactRock, impactWater, impactWood, impactEnemy;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip shotSound;
    public float shotVolume = 1f;


    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;

        
    }
    private void Update()
    {
        MyInput();
    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetMouseButton(0);
        else shooting = Input.GetMouseButtonDown(0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
            
            //Audio
            audioSource.PlayOneShot(shotSound, shotVolume);
        }
    }
    private void Shoot()
    {
        readyToShoot = false;

        StartCoroutine(Recoil_Script.RecoilFire(recoilX, recoilY, aimRecoilX, aimRecoilY, snappiness, returnSpeed));

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range))
        {
            Damageable target = rayHit.transform.GetComponent<Damageable>();
            Transform targetObject;
            targetObject = rayHit.collider.transform;
 
            if (target != null)
            {
                target.TakesDamage(damage);
            }
            else
            {
                
            }

            if (rayHit.transform.CompareTag("isConcrete"))
            {
                Instantiate(impactConcrete, rayHit.point, Quaternion.LookRotation(rayHit.normal), targetObject);
            }
            if (rayHit.transform.CompareTag("isDirt"))
            {
                Instantiate(impactDirt, rayHit.point, Quaternion.LookRotation(rayHit.normal), targetObject);
            }
            if (rayHit.transform.CompareTag("isBrick"))
            {
                Instantiate(impactBrick, rayHit.point, Quaternion.LookRotation(rayHit.normal), targetObject);
            }
            if (rayHit.transform.CompareTag("isFoliage"))
            {
                Instantiate(impactFoliage, rayHit.point, Quaternion.LookRotation(rayHit.normal), targetObject);
            }
            if (rayHit.transform.CompareTag("isGlass"))
            {
                Instantiate(impactGlass, rayHit.point, Quaternion.LookRotation(rayHit.normal), targetObject);
            }
            if (rayHit.transform.CompareTag("isMetal"))
            {
                Instantiate(impactMetal, rayHit.point, Quaternion.LookRotation(rayHit.normal), targetObject);
            }
            if (rayHit.transform.CompareTag("isPlaster"))
            {
                Instantiate(impactPlaster, rayHit.point, Quaternion.LookRotation(rayHit.normal), targetObject);
            }
            if (rayHit.transform.CompareTag("isRock"))
            {
                Instantiate(impactRock, rayHit.point, Quaternion.LookRotation(rayHit.normal), targetObject);
            }
            if (rayHit.transform.CompareTag("isWater"))
            {
                Instantiate(impactWater, rayHit.point, Quaternion.LookRotation(rayHit.normal), targetObject);
            }
            if (rayHit.transform.CompareTag("isWood"))
            {
                Instantiate(impactWood, rayHit.point, Quaternion.LookRotation(rayHit.normal), targetObject);
            }
            if (rayHit.transform.CompareTag("isEnemy"))
            {
                Instantiate(impactEnemy, rayHit.point, Quaternion.LookRotation(rayHit.normal), targetObject);
            }
        }

        //Graphics
        muzzleFlash.Play();

        bulletsLeft--;
        bulletsShot--;

        //Shake Cam
        StartCoroutine(camShake.Shake(camShakeDuration, camShakeMagnitude));

        

        Invoke("ResetShot", 1 / fireRate);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        Debug.Log("Reloading");
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        Debug.Log("Finished Reloading");
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
