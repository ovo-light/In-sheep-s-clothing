using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShepherdMovement : MonoBehaviourPun
{
    // Start is called before the first frame update
    public CharacterController controller;

    public float moveSpeed;
    public float jumpHight;
    public float gravity = - 9.18f;
    private Vector3 verticalVelocity;
    private bool isGround;

    public Camera playerCamera;
    public float mouseSensitivityX;
    public float mouseSensitivityY;
    private float xRotation = 0f;

    void Start()
    {
        // 锁定鼠标到屏幕中心（游戏时隐藏鼠标，按Esc解锁）
        Cursor.lockState = CursorLockMode.Locked;
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

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void MovementAndJump()
    {
        isGround = controller.isGrounded;

        if (isGround)
        {
            verticalVelocity.y = -2f;
        }

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

        Vector3 finalMovement = moveDirection * moveSpeed * Time.deltaTime + verticalVelocity * Time.deltaTime;
        controller.Move(finalMovement);


    }
}
