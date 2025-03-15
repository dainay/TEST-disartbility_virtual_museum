using UnityEngine;

public class BlackDropdownMenuHandler : MonoBehaviour
{
    [SerializeField] private BlackRaycasterManager raycasterManager;
    [SerializeField] private Camera mainCamera;      // Индивидуальная камера игрока
    [SerializeField] private Camera dropdownCamera;  // Индивидуальная вторая камера

    private bool isDropdownActive = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ЛКМ
        {
            HandleClick();
        }

        if (isDropdownActive && Input.GetKeyDown(KeyCode.X)) // Нажатие "X" для выхода
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

            // Получаем DropdownMenuHandler с этого объекта
            BlackDropdownMenuHandler handler = hitObject.GetComponent<BlackDropdownMenuHandler>();

            if (handler != null)
            {
                handler.SwitchToDropdownCamera(); // Переключаем именно его камеру
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
