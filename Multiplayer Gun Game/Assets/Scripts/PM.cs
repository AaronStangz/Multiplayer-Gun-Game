using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using Steamworks;
using UnityEngine.EventSystems;
using System.Threading;


    [RequireComponent(typeof(Rigidbody))]
public class PM : NetworkBehaviour
{
    [Header("Spawning")]
    public float SpawnX = -5;
    public float Spawnz = 5;

    [Header("Spawn")]
    public GameObject PlayerModel;
    public GameObject CamHolder;

    [Header("MoveMent")]
    public float WalkSpeed = 10f;
    public float SideSpeed = 8f;
    public float SprintSpeed = 15f;
    public float rotateSpeed = 100f;
    public float cameraSensitivity = 2.0f;
    public float cameraDistance = 5.0f;
    public Transform cameraTransform;
    public Transform Camera;
    public float jumpForce = 500f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private Vector3 inputDirection;
    private float cameraRotationX = 0f;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PlayerModel.SetActive(false);
        CamHolder.SetActive(false);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (PlayerModel.activeSelf == false)
            {
                SpawnPosition();
                PlayerModel.SetActive(true);
                CamHolder.SetActive(isOwned);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (isOwned)
        {
            // Get input direction from the player
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

            // Rotate the camera
            float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity;
            cameraRotationX -= mouseY;
            cameraRotationX = Mathf.Clamp(cameraRotationX, -90f, 90f);
            Camera.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
            cameraTransform.Rotate(Vector3.up * mouseX);

            // Check if the player is grounded
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
        }

        if (isOwned && Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Jump
            rb.AddForce(Vector3.up * jumpForce);
        }
    }

    private void FixedUpdate()
    {
        if (isOwned)
        {
            // Move the player
            Vector3 moveDirection = cameraTransform.TransformDirection(Vector3.forward);
            Vector3 moveDirectionL = cameraTransform.TransformDirection(Vector3.left);
            Vector3 moveDirectionR = cameraTransform.TransformDirection(Vector3.right);
            float speed = Input.GetKey(KeyCode.LeftShift) ? SprintSpeed : WalkSpeed;
            if (isOwned && Input.GetKey(KeyCode.W) && isGrounded)
            {
                rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
            }
            if (isOwned && Input.GetKey(KeyCode.A) && isGrounded)
            {
                rb.MovePosition(rb.position + moveDirectionL * SideSpeed * Time.fixedDeltaTime);
            }
            if (isOwned && Input.GetKey(KeyCode.D) && isGrounded)
            {
                rb.MovePosition(rb.position + moveDirectionR * SideSpeed * Time.fixedDeltaTime);
            }
            if (isOwned && Input.GetKey(KeyCode.S) && isGrounded)
            {
                rb.MovePosition(rb.position + moveDirectionR * SideSpeed * Time.fixedDeltaTime * 2f);
            }
        }
    }
    public void SpawnPosition()
    {
        transform.position = new Vector3(Random.Range(SpawnX, Spawnz), 0.8f, Random.Range(-15, 7));
    }
}
