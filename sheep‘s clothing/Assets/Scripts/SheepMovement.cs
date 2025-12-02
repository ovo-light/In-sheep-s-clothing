using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SheepMovement : MonoBehaviourPun
{
    public CharacterController controller;
    public float moveSpeed;
    public float runSpeed;
    public float jumpHight;
    public float gravity = -9.18f;
    private Vector3 verticalVelocity;
    private float speed;
    private bool isGround;
    private bool isRun;


    public Camera thirdPersonCamera;
    public float mouseSensitivityX;
    public float mouseSensitivityY;
    private float cameraYRotation = 0f;
    private float cameraXRotation = 15f;

    public float cameraDistance;
    public float cameraHeight;

    public float scrollSensitivity; // 滚轮灵敏度（值越大缩放越快）
    public float minCameraDistance;   // 最小距离（避免太近穿模）
    public float maxCameraDistance;   // 最大距离（避免太远看不见）


    void Start()
    {
        
        
    }

    void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;
        
        CameraLook();

        MovementAndJump();
        
        UpdateCameraPosition();

        HandleScrollZoom();
    }


    void CameraLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY;

        cameraYRotation += mouseX;

        cameraXRotation -= mouseY;
        cameraXRotation = Mathf.Clamp(cameraXRotation, -60f, 30f);
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

        Vector3 cameraRight = Vector3.ProjectOnPlane(thirdPersonCamera.transform.right, Vector3.up).normalized;
        Vector3 cameraForward = Vector3.ProjectOnPlane(thirdPersonCamera.transform.forward, Vector3.up).normalized;

        Vector3 moveDirection = (cameraRight * horizontalInput + cameraForward * verticalInput).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHight * -2f * gravity);
        }

        verticalVelocity.y += gravity * Time.deltaTime;

        Vector3 finalMovement = moveDirection * speed * Time.deltaTime + verticalVelocity * Time.deltaTime;
        controller.Move(finalMovement);

        if (moveDirection.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }

    }

    void UpdateCameraPosition()
    {
        if (thirdPersonCamera == null) return;

        // 计算摄像机相对于羊的位置（基于旋转角度和距离）
        Vector3 offset = new Vector3(0, cameraHeight, -cameraDistance);
        // 绕羊的Y轴旋转水平角度
        Quaternion yRotation = Quaternion.Euler(0, cameraYRotation, 0);
        offset = yRotation * offset;
        // 绕羊的X轴旋转垂直角度（让摄像机上下俯仰）
        Quaternion xRotation = Quaternion.AngleAxis(cameraXRotation, yRotation * Vector3.right);
        offset = xRotation * offset;

        // 设置摄像机位置（羊的位置 + 计算出的偏移）
        thirdPersonCamera.transform.position = transform.position + offset;
        // 摄像机始终看向羊的位置（略微向上，避免看脚）
        thirdPersonCamera.transform.LookAt(transform.position + Vector3.up * 0.5f);
    }

    void HandleScrollZoom()
    {
        // 获取滚轮输入（正值向前滚=拉近，负值向后滚=拉远）
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollInput) < 0.01f) return; // 忽略微小输入

        // 调整距离（scrollInput为正→减小距离=拉近；为负→增大距离=拉远）
        cameraDistance -= scrollInput * scrollSensitivity;

        // 限制距离在最小和最大范围内
        cameraDistance = Mathf.Clamp(cameraDistance, minCameraDistance, maxCameraDistance);
    }
}
