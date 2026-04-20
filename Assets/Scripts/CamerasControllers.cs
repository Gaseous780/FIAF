using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CamerasControllers : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Camera[] camerasOn;

    private Camera activeCamera;

    private Camera mainCamera;

    private bool camerasOff;

    [SerializeField]private UIController uiController;
    [SerializeField]private GameObject player;
    private EnergyBehaviour energyBehaviour;

    public bool _camerasOff => camerasOff;

    private void Awake()
    {
        camerasOff = true;

        mainCamera = Camera.main;
        activeCamera = camerasOn[0];
    }

    private void Start()
    {
        energyBehaviour = player.GetComponent<PlayerBehaviour>()._energy;
    }

    public void SwitchToCamera(int cameraTo)
    {
        activeCamera.gameObject.SetActive(false);
        camerasOn[cameraTo].gameObject.SetActive(true);
        activeCamera = camerasOn[cameraTo];
    }

    private void Update()
    {
        if(energyBehaviour._currentEnergy < 1)
        {
            BackToWolrd();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (energyBehaviour._currentEnergy > 0)
        {
            if (camerasOff == true)
            {
                camerasOff = false;

                activeCamera.gameObject.SetActive(true);
                mainCamera.gameObject.SetActive(false);
                uiController.ActivateUICamera();
                player.GetComponent<PlayerBehaviour>()._isOnCameras = true;
                energyBehaviour.IncreaseUsesOfEnergy();
                player.GetComponent<PlayerInput>().enabled = false;
            }
            else
            {
                camerasOff = true;

                mainCamera.gameObject.SetActive(true);
                activeCamera.gameObject.SetActive(false);
                uiController.ActivateUIOffice();
                player.GetComponent<PlayerBehaviour>()._isOnCameras = false;
                energyBehaviour.DecreaseUsesOfEnergy();
                player.GetComponent<PlayerInput>().enabled = true;
            }
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
