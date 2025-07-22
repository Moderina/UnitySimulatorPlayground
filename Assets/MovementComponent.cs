using UnityEngine;
using UnityEngine.InputSystem;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] private float speed;
    public float mouseSensitivity = 100f;
    float xRotation = 0f;
    private bool InUI = false;
        
    Vector2 MoveVector;
    Vector2 MouseVector;
    
    [SerializeField] private Camera cam;
    [SerializeField] private CharacterController controller;
    [SerializeField] private InteractionController interactionController;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveVector = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        MouseVector = context.ReadValue<Vector2>();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (interactionController.IsUsingATool())
            {
                interactionController.DoPrimaryToolAction();
            }
            else
            {
                interactionController.StartInteractionWithObject();
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            if (interactionController.IsUsingATool())
            {
                interactionController.DoPrimaryToolAction(false);
            }
            else
            {
                interactionController.EndInteractionWithObject();
            }
        }
    }

    public void OnToolsMenu(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            InUI = true;
            GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().ToggleWheelMenu(true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            InUI = false;
            GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().ToggleWheelMenu();
        }
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            interactionController.ThrowObject(cam.transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        MoveView();
        interactionController.UpdateObjectUnderMouse();
    }

    private void MovePlayer()
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = MoveVector.x;
        moveDirection.z = MoveVector.y;
        controller.Move(transform.TransformDirection(moveDirection) * (speed * Time.deltaTime));
    }

    private void MoveView()
    {
        if (InUI) return;
        xRotation -= MouseVector.y * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * (MouseVector.x * mouseSensitivity * Time.deltaTime));
    }
}
