using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using TMPro;

public class PickUp : MonoBehaviour
{

    Gun gunScript;
    Rigidbody rb;
    [Header("Stats")]
    public float Range = 4f;
    public float dropForwardForce, dropUpwardForce;

    [Header("Ref")]
    public Transform mid;
    public Transform cameraTransform;
    public Rigidbody playerRb;

    [Header("Text")]
    public TextMeshProUGUI ammunitionDisplay;
    public GameObject ammoText;
    public TextMeshProUGUI rToReload;
    public GameObject rToreloadText;

    [SerializeField]
    public LayerMask PickUpLayer;

    public bool wEquipped;
    public static bool wSlotFull;


    private void Start()
    {
        ammoText.SetActive(false);

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
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (wSlotFull != true)
            {
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, Range, PickUpLayer) && !wSlotFull)
                {
                    Gun gun = hitInfo.collider.gameObject.GetComponent<Gun>();
                    hitInfo.collider.gameObject.transform.parent = transform;
                    gun.playerRb = playerRb;
                    gun.ammunitionDisplay = ammunitionDisplay;
                    gun.rToReload = rToReload;
                    gun.rToreloadText = rToreloadText;
                    gun.GunRb.transform.localRotation = Quaternion.identity;
                    gun.GunRb.detectCollisions = false;
                    gun.Mid = mid;
                    gun.cameraTransform = cameraTransform;
                    PickUpWeapon();

                    if (wEquipped = true && Input.GetKeyDown(KeyCode.Q))
                    {
                        ammoText.SetActive(false);
                        wEquipped = false;
                        wSlotFull = false;

                        gun.transform.SetParent(null);
                    }
                }
            }
        }
    }

    private void PickUpWeapon()
    {
        ammoText.SetActive(true);
        wEquipped = true;
        wSlotFull = true;

        transform.localRotation = Quaternion.Euler(0, -1, 0);
    }
}
