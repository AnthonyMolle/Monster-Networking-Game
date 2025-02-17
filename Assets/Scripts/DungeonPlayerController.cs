using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class DungeonPlayerController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera playerVCAM;
    [SerializeField] CombatManager combatManager;
    [SerializeField] Transform spawnPoint;
    CinemachinePOV playerVCAMPOV;
    [SerializeField] Rigidbody rb;
    [SerializeField] MeshRenderer mr;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] float mouseSensitivity = 300;

    [SerializeField] float moveSpeed = 10;
    [SerializeField] float acceleration = 100;
    [SerializeField] float decelerationRate = 10f;

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

    [SerializeField] GameObject objectHolderHolder;
    [SerializeField] GameObject objectHolder;
    [SerializeField] GameObject weapon;
    [SerializeField] float maxRotation;

    [SerializeField] int maxHealth = 5;
    int currentHealth = 5;

    Transform camTransform;

    public bool controlsActive = true;

    public bool canInteract = false;

    #region Input

    PlayerInput playerInput;

    Vector2 movementInput;
    public void OnMove(InputAction.CallbackContext context) {movementInput = context.ReadValue<Vector2>();}

    Vector2 lookInput;
    public void OnLook(InputAction.CallbackContext context) {lookInput = context.ReadValue<Vector2>();}

    bool interactPressed;
    public void OnInteract(InputAction.CallbackContext context) {interactPressed = context.action.triggered;}

    bool jumpPressed;
    public void OnJump(InputAction.CallbackContext context) {jumpPressed = context.action.triggered;}

    bool actionPrimaryPressed;
    public void OnPrimary(InputAction.CallbackContext context) {actionPrimaryPressed = context.action.triggered;}

    bool actionSecondaryPressed;
    public void OnSecondary(InputAction.CallbackContext context) {actionSecondaryPressed = context.action.triggered;}

    bool pausePressed;
    public void OnPause(InputAction.CallbackContext context) {pausePressed = context.action.triggered;}

    #endregion

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
        playerInput = FindObjectOfType<PlayerInput>();

        // reset everything related to the player and the combat
        currentHealth = maxHealth;
        transform.position = spawnPoint.position;
    }

    void OnEnable()
    {
        currentHealth = maxHealth;
        transform.position = spawnPoint.position;
        weapon.SetActive(true);
    }

    void OnDisable()
    {
        weapon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isGrounded);

        if (controlsActive)
        {
            if (interactPressed && canInteract)
            {
                DeactivatePlayer();
                combatManager.Win();
            }

            if (jumpPressed && isGrounded)
            {
                jump = true;
            }

            transform.rotation = Quaternion.Euler(0, camTransform.rotation.eulerAngles.y, 0);

            if (movementInput.x > 0.1f)
            {
                playerVCAM.m_Lens.Dutch = Mathf.Lerp(playerVCAM.m_Lens.Dutch, -cameraTilt, cameraTiltSpeed * Time.deltaTime);
            }
            else if (movementInput.x < -0.1f)
            {
                playerVCAM.m_Lens.Dutch = Mathf.Lerp(playerVCAM.m_Lens.Dutch, cameraTilt, cameraTiltSpeed * Time.deltaTime);
            }
            else
            {
                playerVCAM.m_Lens.Dutch = Mathf.Lerp(playerVCAM.m_Lens.Dutch, 0, cameraTiltSpeed * Time.deltaTime);
            }

            Vector3 targetPosition = (-rb.velocity.normalized * rb.velocity.magnitude/moveSpeed)/2 + transform.position;
            objectHolderHolder.transform.position = Vector3.Lerp(objectHolderHolder.transform.position, targetPosition, 10f * Time.deltaTime);

            float targetRotationY = -lookInput.x * maxRotation;
            float targetRotationX = lookInput.y * maxRotation;

            Vector3 targetRotationEuler = new Vector3(targetRotationX, targetRotationY, 0);
            objectHolder.transform.localRotation = Quaternion.Lerp(objectHolder.transform.localRotation, Quaternion.Euler(targetRotationEuler), 10f * Time.deltaTime);
        }
        else
        {

        }

        if (pausePressed)
        {
            pauseMenu.SetActive(true);
        }
    }

    private void FixedUpdate() 
    {
        if (controlsActive)
        {
            rb.AddForce(((gameObject.transform.forward * movementInput.y) + (gameObject.transform.right * movementInput.x)).normalized * acceleration, ForceMode.Force);

            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 cappedVelocity = flatVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(cappedVelocity.x, rb.velocity.y, cappedVelocity.z);
            }

            if (!isGrounded)
            {
                if (rb.velocity.y < fallInitiationSpeed)
                {
                    if (-rb.velocity.y > maxFallSpeed)
                    {
                        rb.velocity = new Vector3(rb.velocity.x,  -maxFallSpeed, rb.velocity.z);
                    }
                    else
                    {
                        rb.AddForce(-transform.up * fallGravity, ForceMode.Force);
                    }
                }
            }
            else
            {
                if (movementInput.y < 0.1f && movementInput.x < 0.1f && rb.velocity.magnitude > 0)
                {
                    Vector3 decelerationDirection = -(new Vector3(rb.velocity.x, 0, rb.velocity.z)).normalized;
                    rb.AddForce(decelerationDirection * decelerationRate, ForceMode.Force);

                    if (rb.velocity.magnitude < 0.25f)
                    {
                        rb.velocity = new Vector3(0, rb.velocity.y, 0);
                    }
                }
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
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
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

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        combatManager.Lose();
    }

    Chest currentChest;

    public void SetInteractable(Chest chest)
    {
        canInteract = true;
        currentChest = chest;
    }

    public void ResetInteractable()
    {
        canInteract = false;
        currentChest = null;
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
