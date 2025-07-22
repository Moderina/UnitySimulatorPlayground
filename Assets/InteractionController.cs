using Tools;
using UI;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private Transform pickUpPoint;
    [SerializeField] private Transform toolHandle;
    [SerializeField] private Material OutlineMaterial;
    private GameObject CurrentHoveredObject;
    private GameObject CurrentInteractedObject;

    private int currentTool = -1;
    void Start()
    {
        WheelMenu.OnToolChanged += ChangeTool;
    }

    public bool IsUsingATool()
    {
        return currentTool != -1;
    }

    public void UpdateObjectUnderMouse()
    {
        if(CurrentInteractedObject) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, 576))
        {
            if (hit.transform.gameObject == CurrentHoveredObject) return;
            if (CurrentHoveredObject)
            {
                CurrentHoveredObject.GetComponent<PickUp>().ToggleOutlineMaterial();
            }
            CurrentHoveredObject = hit.transform.gameObject;
            CurrentHoveredObject.GetComponent<PickUp>().ToggleOutlineMaterial(OutlineMaterial);
        }
        else
        {
            if (!CurrentHoveredObject) return;
            CurrentHoveredObject.GetComponent<PickUp>().ToggleOutlineMaterial();
            CurrentHoveredObject = null;
        }
    }

    public void StartInteractionWithObject()
    {
        if (CurrentHoveredObject) CurrentInteractedObject = CurrentHoveredObject;
        CurrentInteractedObject.GetComponent<PickUp>().OnPickedUp(pickUpPoint);
    }

    public void EndInteractionWithObject()
    {
        if (CurrentInteractedObject == null) return;
        CurrentInteractedObject.GetComponent<PickUp>().OnPutDown();
        CurrentInteractedObject = null;
    }

    public void ThrowObject(Vector3 direction)
    {
        if (CurrentInteractedObject == null) return;
        CurrentInteractedObject.GetComponent<PickUp>().OnThrown(direction);
        EndInteractionWithObject();
    }

    private void ChangeTool(int toolIndex)
    {
        if (currentTool != -1) toolHandle.GetChild(currentTool).gameObject.SetActive(false);
        if (toolIndex != -1) toolHandle.GetChild(toolIndex).gameObject.SetActive(true);
        currentTool = toolIndex;
    }

    public void DoPrimaryToolAction(bool started = true)
    {
        toolHandle.GetChild(currentTool).gameObject.GetComponent<ITool>().PrimaryAction(started);
    }
}
