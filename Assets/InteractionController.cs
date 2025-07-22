using Tools;
using UI;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform pickUpPoint;
    [SerializeField] private Transform toolHandle;
    [SerializeField] private Material OutlineMaterial;
    [SerializeField] private ComputeShader outline;
    [SerializeField] private RenderTexture texture;
    private GameObject CurrentHoveredObject;
    private GameObject CurrentInteractedObject;
    private Vector3 lastObjectPosition;

    private int currentTool = -1;
    void Start()
    {
        WheelMenu.OnToolChanged += ChangeTool;
        
        texture = new RenderTexture(256, 256, 0);
        texture.enableRandomWrite = true;
        texture.Create();

        int kernel = outline.FindKernel("CSMain");
        outline.SetTexture(kernel, "Result", texture);
        outline.Dispatch(kernel, 32, 32, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentInteractedObject)
            lastObjectPosition = CurrentInteractedObject.transform.position;
    }

    public bool IsUsingATool()
    {
        return currentTool != -1;
    }

    public GameObject GetLookedAtObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, 1 << 6))
        {

            hit.transform.gameObject.GetComponent<PickUp>().ToggleOutlineMaterial(OutlineMaterial);
            return hit.transform.gameObject;
        }
        
        return null;
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
        CurrentInteractedObject.GetComponent<PickUp>().OnPutDown(lastObjectPosition);
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
