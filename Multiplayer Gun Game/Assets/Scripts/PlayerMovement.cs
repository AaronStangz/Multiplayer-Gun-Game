using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using Steamworks;

public class PlayerMovement : NetworkBehaviour
{

    [Header("Spawning")]
    public float SpawnX = -5;
    public float Spawnz = 5;

    public float mouseSensitivity = 100f;
    float xRotation = 0f;
    public float Speed = 0.1f;

    public GameObject PlayerModel;
    public GameObject Cam;

    private void Start()
    {
        PlayerModel.SetActive(false);
        Cam.SetActive(false);
    }
    private void Update()
    {


        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (PlayerModel.activeSelf == false)
            {
                SpawnPosition();
                PlayerModel.SetActive(true);
                Cam.SetActive(isOwned);
            }

            if (isOwned)
            {
                Movement();
            }
        }
    }

    public void Movement()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, 0.0f, zDirection);

        transform.position += moveDirection * Speed;
    }
    public void SpawnPosition()
    {
        transform.position = new Vector3(Random.Range(SpawnX, Spawnz), 0.8f, Random.Range(-15,7));
    }
}
