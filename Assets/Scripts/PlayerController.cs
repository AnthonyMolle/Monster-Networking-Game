using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera playerVCAM;
    [SerializeField] Rigidbody rb;
    [SerializeField] MeshRenderer mr;
    [SerializeField] float mouseSensitivity = 300;
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float acceleration = 100;

    DialogueManager dm;

    Transform camTransform;

    float xRotation = 0f;
    float yRotation = 0f;
    float verticalMovement;
    float horizontalMovement;

    public bool controlsActive = true;

    CinemachineVirtualCamera outerCamera;
    public bool canInteract = false;

    public TextAsset NPCDialogue;
    public NPC ActiveNPC;

    private void Awake() 
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        camTransform = Camera.main.transform;

        playerVCAM = GameObject.FindWithTag("PlayerCam").GetComponent<CinemachineVirtualCamera>();
        playerVCAM.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = mouseSensitivity;
        playerVCAM.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = mouseSensitivity;

        rb = GetComponent<Rigidbody>();
        dm = FindObjectOfType<DialogueManager>();
        // xRotation = gameObject.transform.rotation.eulerAngles.x;
        // yRotation = gameObject.transform.rotation.eulerAngles.y; // angles may be off
    }

    // Update is called once per frame
    void Update()
    {
        if (controlsActive)
        {
            if (Input.GetKeyDown(KeyCode.E) && canInteract)
            {
                outerCamera.gameObject.SetActive(true);
                controlsActive = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                mr.enabled = false;
                
                if (NPCDialogue != null)
                {
                    dm.InitializeDialogue(NPCDialogue, ActiveNPC);
                }
                else
                {
                    Debug.Log("dialogue not found");
                }
            }

            transform.rotation = Quaternion.Euler(0, camTransform.rotation.eulerAngles.y, 0);

            horizontalMovement = Input.GetAxisRaw("Horizontal");
            verticalMovement = Input.GetAxisRaw("Vertical");
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

    public void ReactivatePlayer()
    {
        controlsActive = true;
        outerCamera.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mr.enabled = true;
    }
}
