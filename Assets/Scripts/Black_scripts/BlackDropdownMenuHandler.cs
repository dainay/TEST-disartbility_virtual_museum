using UnityEngine;

public class BlackDropdownMenuHandler : MonoBehaviour
{
    [SerializeField] private BlackRaycasterManager raycasterManager;
    [SerializeField] private Camera mainCamera;      // Основная камера игрока
    [SerializeField] private Camera dropdownCamera;  // Камера для выпадающего меню
    [SerializeField] private CanvasGroup[] dropdownCanvasGroups; // Массив CanvasGroup

    private bool isDropdownActive = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }

        if (isDropdownActive && Input.GetKeyDown(KeyCode.X))
        {
            SwitchToMainCamera();
        }
    }

    void HandleClick()
    {
        GameObject hitObject = raycasterManager.GetRaycastHit();

        if (hitObject != null && hitObject.CompareTag("DropPainter"))
        {
            Debug.Log("🎨 DropPainter clicked: " + hitObject.name);

            BlackDropdownMenuHandler handler = hitObject.GetComponent<BlackDropdownMenuHandler>();

            if (handler != null)
            {
                handler.SwitchToDropdownCamera();
            }
        }
    }

    public void SwitchToDropdownCamera()
    {
        isDropdownActive = true;

        mainCamera.gameObject.SetActive(false);
        dropdownCamera.gameObject.SetActive(true);

   
        EnableCursor(true);
    }

    public void SwitchToMainCamera()
    {
        isDropdownActive = false;

        mainCamera.gameObject.SetActive(true);
        dropdownCamera.gameObject.SetActive(false);

     

        EnableCursor(false);
    }

    void EnableCursor(bool enable)
    {
        Cursor.visible = enable;
        Cursor.lockState = enable ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
