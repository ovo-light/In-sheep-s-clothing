using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShepherdMovement : MonoBehaviourPun
{
    // Start is called before the first frame update
    public CharacterController controller;
    public Transform playerBody;
    public Transform playerBodyUpDown;

    public float moveSpeed;
    public float runSpeed;
    public float jumpHight;
    public float gravity = - 9.18f;
    private Vector3 verticalVelocity;
    private bool isGround;
    private float speed;
    private bool isRun;

    public Camera playerCamera;
    public float mouseSensitivityX;
    public float mouseSensitivityY;
    private float xRotation = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;
        MouseLook();

        MovementAndJump();

        
    }


    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY;

        xRotation += mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        
        
        playerBodyUpDown.localRotation = Quaternion.Euler(0f, 0f, xRotation);
        playerBody.Rotate(Vector3.up * mouseX);

    }

    void MovementAndJump()
    {
        isGround = controller.isGrounded;
        isRun = Input.GetKey(KeyCode.LeftShift);

        if (isGround)
        {
            verticalVelocity.y = -2f;
        }

        speed = isRun ? runSpeed : moveSpeed;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 cameraRight = Camera.main.transform.right;
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        Vector3 moveDirection = (cameraRight * horizontalInput + cameraForward * verticalInput).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHight * -2f * gravity);
        }

        verticalVelocity.y += gravity * Time.deltaTime;

        Vector3 finalMovement = moveDirection * speed * Time.deltaTime + verticalVelocity * Time.deltaTime;
        controller.Move(finalMovement);


    }
}
