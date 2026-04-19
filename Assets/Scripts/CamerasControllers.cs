using UnityEngine;
using UnityEngine.EventSystems;

public class CamerasControllers : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Camera[] camerasOn;

    private Camera activeCamera;

    private Camera mainCamera;

    private bool camerasOff;

    private void Awake()
    {
        camerasOff = true;

        mainCamera = Camera.main;
        activeCamera = camerasOn[0];
    }

    public void SwitchToCamera(int cameraTo)
    {
        activeCamera.gameObject.SetActive(false);
        camerasOn[cameraTo].gameObject.SetActive(true);
        activeCamera = camerasOn[cameraTo];
    }

    public void OnPointerEnter(PointerEventData eventData) 
    {
        if (camerasOff == true) 
        { 
            camerasOff = false;

            activeCamera.gameObject.SetActive(true);
            mainCamera.gameObject.SetActive(false);
            GameManager.manager._uiController.ActivateUICamera();
            GameManager.manager._player.SetActive(false);
        }
        else
        {
            camerasOff = true;

            mainCamera.gameObject.SetActive(true);
            activeCamera.gameObject.SetActive(false);
            GameManager.manager._uiController.ActivateUIOffice();
            GameManager.manager._player.SetActive(true);
        }
    }
}
