using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;
using static UnityEngine.GraphicsBuffer;
using Mirror;
using Steamworks;

public class Gun: MonoBehaviour
{
    [Header("Bullet")]
    public GameObject bullet;

    [Header("Bullet Force")]
    public float shootForce, upwardForce;

    [Header("Gun Stats")]
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    [Header("Recoil")]
    public Rigidbody playerRb;
    public float recoilForce;
    public float recoilOnCam;

    [Header("Bools")]
    bool shooting, readyToShoot, reloading;
    PickUp pickUpScript;
    public bool wEquipped;

    [Header("Ref")]
    public Transform attackPoint;
    public Transform cameraTransform;
    public Transform Mid;
    public Rigidbody GunRb;

    [Header("CamShake")]
    public AnimationCurve curve;
    public float shakeDuration = 1f;

    [Header("Graphics")]
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;
    public TextMeshProUGUI rToReload;
    public GameObject rToreloadText;


    //bug fixing :D
    public bool allowInvoke = true;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        if (PickUp.wSlotFull == true)
        {
            gameObject.transform.position = Mid.transform.position;
        }

        if (playerRb != null)
        {
            MyInput();
            if (ammunitionDisplay != null)
                ammunitionDisplay.SetText("Ammo " + bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        }
        
        if(bulletsLeft == 0 && !reloading)
        {
            rToreloadText.SetActive(true);
        }
        if(reloading)
        {
            rToreloadText.SetActive(false);
        }
       
    }
    private void MyInput()
    {
        if(PickUp.wSlotFull == true)
        {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            Shoot();
        }

        }

    }

    private void Shoot()
    {
        readyToShoot = false;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your current view
        RaycastHit hit;


        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //Just a point far away from the player

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); //Just add spread to last direction

        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity); //store instantiated bullet in currentBullet
        currentBullet.transform.forward = directionWithSpread.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(Camera.main.transform.up * upwardForce, ForceMode.Impulse);

        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;

            playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
            StartCoroutine(CamShake());
           // Camera.main.transform.rotation = Quaternion.Euler(recoilOnCam, 0, 0);
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime); //Invoke ReloadFinished function with your reloadTime as delay
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    IEnumerator CamShake()
    {
        Vector3 startPosition = cameraTransform.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float streangth = curve.Evaluate(elapsedTime / shakeDuration);
            transform.position = startPosition + Random.insideUnitSphere * streangth;
            yield return null;
        }
    }
}
