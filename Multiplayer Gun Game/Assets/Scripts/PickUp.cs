using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class PickUp : MonoBehaviour
{
    Gun gunScript;
    Rigidbody rb;
    public float Range = 4f;
    public float dropForwardForce, dropUpwardForce;

    public Transform mid;
    public Transform cameraTransform;
    public Rigidbody playerRb;

    public LayerMask PickUpLayer;

    public bool wEquipped;
    public static bool wSlotFull;

    private void Start()
    {
        if (!wEquipped)
        {
            wSlotFull = false;
        }
        if (wEquipped)
        {
            wSlotFull = true;
        }
    }
    private void Update()
    {
        if (wEquipped && Input.GetKeyDown(KeyCode.Q)) DropWeapon();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (wSlotFull != true)
            {
                if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, Range, PickUpLayer) && !wSlotFull)
                {
                    Gun gun = hitInfo.collider.gameObject.GetComponent<Gun>();
                    gun.playerRb = playerRb;
                    gun.cameraTransform = cameraTransform;
                    hitInfo.collider.gameObject.transform.parent = transform;
                    PickUpWeapon();
                }
            }
        }

    }

    private void PickUpWeapon()
    {
        wEquipped = true;
        wSlotFull = true;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        rb.isKinematic = true;

        gunScript.enabled = true;
    }
    private void DropWeapon()
    {
        wEquipped = false;
        wSlotFull = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        rb.velocity = GetComponent<Rigidbody>().velocity;

        rb.AddForce(Camera.main.transform.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(Camera.main.transform.up * dropUpwardForce, ForceMode.Impulse);
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

        gunScript.enabled = false;
    }

}
