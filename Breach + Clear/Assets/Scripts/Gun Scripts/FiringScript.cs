using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//remove .UI after reload animation
using UnityEngine.UI;

public class FiringScript : MonoBehaviour
{
    [Header("Stats")]
    public int damage;
    public float fireRate, range, reloadTime, timeBetweenShots, reloadWindow;
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

    // remove when guns animation is finished
    [Header("TEMP Reload Indicator")]
    public Image badgeIndicator;
    public Image badgeShadowOne;
    public Image badgeShadowTwo;
    private Color iNDColor;
    private Color startColor;
    private Color startShadowOne;
    private Color startShadowTwo;

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
        StartCoroutine(Reloading());
    }
    private void ReloadFinished()
    {
        Debug.Log("Finished Reloading");
        // remove all color stuff when animation for guns is finished
        badgeIndicator.color = startColor;
        badgeShadowOne.color = startShadowOne;
        badgeShadowTwo.color = startShadowTwo;
        bulletsLeft = magazineSize;
        reloading = false;
    }

    IEnumerator Reloading()
    {
        bool newMag = false;
        bool pullOne = false;
        bool magPulled = false;
        float exitTime = reloadWindow;
        float timer = 0;
    
        // remove all color stuff when animation for guns is finished
        startColor = badgeIndicator.color;
        startShadowOne = badgeShadowOne.color;
        startShadowTwo = badgeShadowTwo.color;

        iNDColor = new Color(1, 0.2f, 0, 1);
        badgeIndicator.color = iNDColor;
        badgeShadowOne.color = iNDColor;
        badgeShadowTwo.color = iNDColor;

        while (exitTime > timer)
        {

            if (!magPulled)
            {
                yield return new WaitForSeconds(reloadTime / 2);
                Debug.Log("Mag Pulled");
                iNDColor = new Color(1, 0.5f, 0, 1);
                magPulled = true;
                
            }

            if (magPulled)
            {
                if (Input.GetKeyDown("e"))
                {
                    //Insert new mag
                    yield return new WaitForSeconds(reloadTime / 2);
                    Debug.Log("New Mag In");
                    iNDColor = new Color(0.3f, 0.65f, 0, 1);
                    newMag = true;
                }

                if (Input.GetKeyDown("l") && newMag && !pullOne)
                {
                    // pull back bolt
                    Debug.Log("Pulled Back Bolt");
                    yield return new WaitForSeconds(0.01f);
                    pullOne = true;
                }

                if (Input.GetKeyDown("l") && pullOne)
                {
                    // let go of bolt
                    Debug.Log("Bolt Reset");
                    ReloadFinished();
                    yield break;
                }
            }

            badgeIndicator.color = iNDColor;
            badgeShadowOne.color = iNDColor;
            badgeShadowTwo.color = iNDColor;

            timer += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Reload Failed");
        reloading = false;
        yield return null;
    }
}

