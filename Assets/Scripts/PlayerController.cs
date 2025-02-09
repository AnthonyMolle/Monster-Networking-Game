using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera playerVCAM;
    CinemachinePOV playerVCAMPOV;
    [SerializeField] Rigidbody rb;
    [SerializeField] MeshRenderer mr;
    [SerializeField] float mouseSensitivity = 300;
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float acceleration = 100;

    [SerializeField] GameObject pauseMenu;

    Transform camTransform;

    public bool controlsActive = true;

    CinemachineVirtualCamera outerCamera;
    public bool canInteract = false;

    //public TextAsset NPCDialogue;
    public NPC activeNPC;

    #region Input
    
    PlayerInput playerInput;

    Vector2 movementInput;
    public void OnMove(InputAction.CallbackContext context) {movementInput = context.ReadValue<Vector2>();}

    Vector2 lookInput;
    public void OnLook(InputAction.CallbackContext context) {lookInput = context.ReadValue<Vector2>();}

    bool interactPressed;
    public void OnInteract(InputAction.CallbackContext context) {interactPressed = context.action.triggered;}

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
        //dm = FindObjectOfType<DialogueManager>();
        // xRotation = gameObject.transform.rotation.eulerAngles.x;
        // yRotation = gameObject.transform.rotation.eulerAngles.y; // angles may be off
    }

    // Update is called once per frame
    void Update()
    {
        if (controlsActive)
        {
            if (interactPressed && canInteract)
            {
                DeactivatePlayer();
                
                if (activeNPC != null)
                {
                    activeNPC.SendDialogue();
                }
                else
                {
                    Debug.Log("npc not found, caninteract should be false but wasnt");
                }
            }

            transform.rotation = Quaternion.Euler(0, camTransform.rotation.eulerAngles.y, 0);
            //Debug.Log(lookInput);
        }
        else
        {
            
        }

        if (pausePressed)
        {
            pauseMenu.gameObject.SetActive(true);
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
                rb.velocity = new Vector3(cappedVelocity.x, 0f, cappedVelocity.z);
            }
        }
    }

    public void SetInteractable(CinemachineVirtualCamera cam)
    {
        outerCamera = cam;
        canInteract = true;
    }

    public void ResetInteractable()
    {
        outerCamera = null;
        canInteract = false;
    }

    public void DeactivatePlayer()
    {
        if (outerCamera != null)
        {
            outerCamera.gameObject.SetActive(true);
        }
        controlsActive = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        mr.enabled = false;

        //playerInput.SwitchCurrentActionMap("UI");

        playerVCAMPOV.m_HorizontalAxis.m_MaxSpeed = 0;
        playerVCAMPOV.m_VerticalAxis.m_MaxSpeed = 0;
    }
    public void ReactivatePlayer()
    {
        controlsActive = true;
        if (outerCamera != null)
        {
            outerCamera.gameObject.SetActive(false);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mr.enabled = true;

        //playerInput.SwitchCurrentActionMap("Player");

        playerVCAMPOV.m_HorizontalAxis.m_MaxSpeed = mouseSensitivity;
        playerVCAMPOV.m_VerticalAxis.m_MaxSpeed = mouseSensitivity;
    }

    [SerializeField] float reactivationDelay;

    public void ReactivatePlayerDelay()
    {
        StartCoroutine(PlayerActivationDelay());

        if (outerCamera != null)
        {
            outerCamera.gameObject.SetActive(false);
        }

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
