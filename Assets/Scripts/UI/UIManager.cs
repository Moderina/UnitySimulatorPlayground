using UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private WheelMenu wheelMenu;

    public WheelMenu GetWheelMenu() { return wheelMenu; }

    public void ToggleWheelMenu(bool show = false)
    {
        Cursor.visible = show;
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        if (!show) wheelMenu.ChangeTool();
        wheelMenu.gameObject.SetActive(show);
    }
}
