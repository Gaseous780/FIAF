using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CamerasControllers : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Camera[] camerasOn;

    private Camera activeCamera;

    private Camera mainCamera;

    private bool camerasOff;

    private UIController uiController;
    private GameObject player;

    public bool _camerasOff => camerasOff;

    private void Awake()
    {
        camerasOff = true;

        mainCamera = Camera.main;
        activeCamera = camerasOn[0];
    }

    private void Start()
    {
        uiController = GameManager.manager._uiController;
        player = GameManager.manager._player;
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
            uiController.ActivateUICamera();
            player.GetComponent <PlayerBehaviour>()._isOnCameras = true;
            player.GetComponent <PlayerInput>().enabled = false;
        }
        else
        {
            camerasOff = true;

            mainCamera.gameObject.SetActive(true);
            activeCamera.gameObject.SetActive(false);
            uiController.ActivateUIOffice();
            player.GetComponent <PlayerBehaviour>()._isOnCameras = false;
            player.GetComponent<PlayerInput>().enabled = true;
        }
    }

    public void BackToWolrd()
    {
        camerasOff = true;

        mainCamera.gameObject.SetActive(true);
        activeCamera.gameObject.SetActive(false);
        uiController.ActivateUIOffice();
        player.GetComponent<PlayerBehaviour>()._isOnCameras = false;
        player.GetComponent<PlayerInput>().enabled = true;
    }
}
