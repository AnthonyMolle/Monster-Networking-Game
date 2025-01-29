using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;

public class DungeonPlayerController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera playerVCAM;
    CinemachinePOV playerVCAMPOV;
    [SerializeField] Rigidbody rb;
    [SerializeField] MeshRenderer mr;
    [SerializeField] float mouseSensitivity = 300;

    [SerializeField] float moveSpeed = 10;
    [SerializeField] float acceleration = 100;

    [SerializeField] float jumpForce = 5f;

    [SerializeField] float fallInitiationSpeed = 0.5f;
    [SerializeField] float fallGravity = 5f;
    [SerializeField] float maxFallSpeed = 10f;

    [SerializeField] float coyoteTime = 0.25f;

    [SerializeField] float groundCheckLength = 1f;
    [SerializeField] LayerMask groundLayers;
    bool isGrounded;
    bool jump;
    bool jumping;

    [SerializeField] float cameraTilt = 5f;
    [SerializeField] float cameraTiltSpeed = 5f;

    Transform camTransform;

    float xRotation = 0f;
    float yRotation = 0f;
    float verticalMovement;
    float horizontalMovement;

    public bool controlsActive = true;

    public bool canInteract = false;

    private void Awake() 
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        camTransform = Camera.main.transform;

        playerVCAMPOV = playerVCAM.GetCinemachineComponent<CinemachinePOV>();
        playerVCAMPOV.m_HorizontalAxis.m_MaxSpeed = mouseSensitivity;
        playerVCAMPOV.m_VerticalAxis.m_MaxSpeed = mouseSensitivity;

        rb = GetComponent<Rigidbody>();
        //dm = FindObjectOfType<DialogueManager>();
        // xRotation = gameObject.transform.rotation.eulerAngles.x;
        // yRotation = gameObject.transform.rotation.eulerAngles.y; // angles may be off
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isGrounded);

        if (controlsActive)
        {
            if (Input.GetKeyDown(KeyCode.E) && canInteract)
            {
                DeactivatePlayer(); // might not need this
                // i think this will almost exclusively be used for opening chests
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                jump = true;
            }

            transform.rotation = Quaternion.Euler(0, camTransform.rotation.eulerAngles.y, 0);

            horizontalMovement = Input.GetAxisRaw("Horizontal");
            verticalMovement = Input.GetAxisRaw("Vertical");

            if (horizontalMovement > 0.1f)
            {
                playerVCAM.m_Lens.Dutch = Mathf.Lerp(playerVCAM.m_Lens.Dutch, -cameraTilt, cameraTiltSpeed * Time.deltaTime);
            }
            else if (horizontalMovement < -0.1f)
            {
                playerVCAM.m_Lens.Dutch = Mathf.Lerp(playerVCAM.m_Lens.Dutch, cameraTilt, cameraTiltSpeed * Time.deltaTime);
            }
            else
            {
                playerVCAM.m_Lens.Dutch = Mathf.Lerp(playerVCAM.m_Lens.Dutch, 0, cameraTiltSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //outerCamera.gameObject.GetComponentInParent<DialogueManager>().SendInput();
            }
        }
    }

    private void FixedUpdate() 
    {
        if (controlsActive)
        {
            rb.AddForce(((gameObject.transform.forward * verticalMovement) + (gameObject.transform.right * horizontalMovement)).normalized * acceleration, ForceMode.Force);

            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 cappedVelocity = flatVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(cappedVelocity.x, rb.velocity.y, cappedVelocity.z);
            }

            if (jump)
            {
                isGrounded = false;
                jump = false;
                jumping = true;
                Jump();
            }

            RaycastHit hit; 
            if (Physics.Raycast(transform.position, -transform.up, out hit, groundCheckLength, groundLayers))
            {
                if (!jumping)
                {
                    isGrounded = true;
                }
            }
            else
            {
                if (isGrounded)
                {
                    DoCoyoteTime();
                }
                else if (jumping)
                {
                    jumping = false;
                }
            }
        }
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Force);
    }

    private void DoCoyoteTime()
    {
        StartCoroutine(CoyoteTimeCoroutine());
    }

    private IEnumerator CoyoteTimeCoroutine()
    {
        yield return new WaitForSeconds(coyoteTime);
        isGrounded = false;
    }

    public void SetInteractable(CinemachineVirtualCamera cam)
    {
        canInteract = true;
    }

    public void ResetInteractable()
    {
        canInteract = false;
    }

    public void DeactivatePlayer()
    {
        controlsActive = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        mr.enabled = false;

        playerVCAMPOV.m_HorizontalAxis.m_MaxSpeed = 0;
        playerVCAMPOV.m_VerticalAxis.m_MaxSpeed = 0;
    }
    public void ReactivatePlayer()
    {
        controlsActive = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mr.enabled = true;

        playerVCAMPOV.m_HorizontalAxis.m_MaxSpeed = mouseSensitivity;
        playerVCAMPOV.m_VerticalAxis.m_MaxSpeed = mouseSensitivity;
    }

    [SerializeField] float reactivationDelay;

    public void ReactivatePlayerDelay()
    {
        StartCoroutine(PlayerActivationDelay());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mr.enabled = true;
    }

    private IEnumerator PlayerActivationDelay()
    {
        yield return new WaitForSeconds(reactivationDelay);

        controlsActive = true;
        playerVCAMPOV.m_HorizontalAxis.m_MaxSpeed = mouseSensitivity;
        playerVCAMPOV.m_VerticalAxis.m_MaxSpeed = mouseSensitivity;
    }
}
