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
    [SerializeField] public Rigidbody rb;
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

    bool isLaunched = false;

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

    public bool actionPrimaryPressed;
    public void OnPrimary(InputAction.CallbackContext context) {actionPrimaryPressed = context.action.triggered;}

    public bool actionSecondaryPressed;
    public void OnSecondary(InputAction.CallbackContext context) {actionSecondaryPressed = context.action.triggered;}

    bool pausePressed;
    public void OnPause(InputAction.CallbackContext context) {pausePressed = context.action.triggered;}

    #endregion

    protected virtual void Awake() 
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    protected virtual void Start()
    {   
        camTransform = Camera.main.transform;

        UpdateSensitivity();

        rb = GetComponent<Rigidbody>();
        playerInput = FindObjectOfType<PlayerInput>();

        // reset everything related to the player and the combat
        currentHealth = maxHealth;
        transform.position = spawnPoint.position;
    }

    protected virtual void OnEnable()
    {
        currentHealth = maxHealth;
        transform.position = spawnPoint.position;
        weapon.SetActive(true);
        UpdateSensitivity();
    }

    public void UpdateSensitivity()
    {
        if (playerVCAMPOV == null)
        {
            playerVCAMPOV = playerVCAM.GetCinemachineComponent<CinemachinePOV>();
        }
        playerVCAMPOV.m_HorizontalAxis.m_MaxSpeed = PlayerPrefs.GetInt("Sensitivity") * 10;
        playerVCAMPOV.m_VerticalAxis.m_MaxSpeed = PlayerPrefs.GetInt("Sensitivity") * 10;
    }

    protected virtual void OnDisable()
    {
        weapon.SetActive(false);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
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

            Vector3 targetPosition = (-rb.velocity.normalized * rb.velocity.magnitude/moveSpeed)/2.5f + transform.position;
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

    float launchUnsetBuffer = 0.5f;
    protected virtual void FixedUpdate() 
    {
        Debug.Log(isGrounded);

        if (controlsActive)
        {
            //Debug.Log("running physics");
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
            else if (!isLaunched)
            {
                if (movementInput.y < 0.1f && movementInput.x < 0.1f && rb.velocity.magnitude > 0)
                {
                    Debug.Log("applying deceleration");
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
                if (isLaunched)
                {
                    launchUnsetBuffer -= Time.deltaTime;
                    if (launchUnsetBuffer <= 0)
                    {
                        UnsetLaunched();
                        launchUnsetBuffer = 0.5f;
                    }
                }

                if (!jumping && !isLaunched)
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
                else if (jumping || isLaunched)
                {
                    if (jumping)
                    {
                        jumping = false;
                    }

                    if (isLaunched)
                    {
                        UnsetLaunched();
                    }
                }
            }

            
        }
    }

    public virtual void SetLaunched()
    {
        isLaunched = true;
        isGrounded = false;
    }

    public virtual void UnsetLaunched()
    {
        isLaunched = false;
    }

    protected virtual void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    protected virtual void DoCoyoteTime()
    {
        StartCoroutine(CoyoteTimeCoroutine());
    }

    protected virtual IEnumerator CoyoteTimeCoroutine()
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

    protected virtual void Die()
    {
        combatManager.Lose();
    }

    Chest currentChest;

    public virtual void SetInteractable(Chest chest)
    {
        canInteract = true;
        currentChest = chest;
    }

    public virtual void ResetInteractable()
    {
        canInteract = false;
        currentChest = null;
    }

    public virtual void DeactivatePlayer()
    {
        controlsActive = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        mr.enabled = false;

        playerVCAMPOV.m_HorizontalAxis.m_MaxSpeed = 0;
        playerVCAMPOV.m_VerticalAxis.m_MaxSpeed = 0;

        rb.velocity = Vector3.zero;
    }
    public virtual void ReactivatePlayer()
    {
        controlsActive = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mr.enabled = true;

        playerVCAMPOV.m_HorizontalAxis.m_MaxSpeed = mouseSensitivity;
        playerVCAMPOV.m_VerticalAxis.m_MaxSpeed = mouseSensitivity;
    }

    [SerializeField] float reactivationDelay;

    public virtual void ReactivatePlayerDelay()
    {
        StartCoroutine(PlayerActivationDelay());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mr.enabled = true;
    }

    protected virtual IEnumerator PlayerActivationDelay()
    {
        yield return new WaitForSeconds(reactivationDelay);

        controlsActive = true;
        playerVCAMPOV.m_HorizontalAxis.m_MaxSpeed = mouseSensitivity;
        playerVCAMPOV.m_VerticalAxis.m_MaxSpeed = mouseSensitivity;
    }

    public Vector3 GetCameraPosition()
    {
        return Camera.main.transform.position;
    }

    public Vector3 GetCameraForwardVector()
    {
        return Camera.main.transform.forward;
    }

    public Transform GetHeldObjectTransform()
    {
        return weapon.transform;
    }
}
