using System.Collections;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject doorModelCore;

    [SerializeField] private float minPositionOnY;
    [SerializeField] private float maxPositionOnY;

    [SerializeField] private float speedOfDoors = 1f;

    private bool isOpenDoor = true;

    private bool closeDoor;

    private float progresDoorStatus;

    [SerializeField] private GameObject player;
    private EnergyBehaviour playerEnergy;

    private void Awake()
    {
        maxPositionOnY = doorModelCore.transform.position.y;

        closeDoor = false;
        progresDoorStatus = 0;
    }

    private void Start()
    {
        playerEnergy = player.GetComponent<PlayerBehaviour>()._energy;
    }

    public void InteractDoor()
    {
        if (playerEnergy._currentEnergy > 0)
        {
            if (isOpenDoor == true && playerEnergy._usesOfEnergy < 4)
            {
                isOpenDoor = false;
                playerEnergy.IncreaseUsesOfEnergy();
            }
            else if (isOpenDoor == false)
            {
                isOpenDoor = true;
                playerEnergy.DecreaseUsesOfEnergy();
            }

            closeDoor = true;
        }
    }

    public void MovementDoor()
    {
        progresDoorStatus += speedOfDoors * Time.deltaTime;

        if (isOpenDoor == true)
        {
            doorModelCore.transform.position = new Vector3(doorModelCore.transform.position.x, Mathf.Lerp(doorModelCore.transform.position.y, maxPositionOnY, progresDoorStatus), doorModelCore.transform.position.z);
        }
        else
        {
            doorModelCore.transform.position = new Vector3(doorModelCore.transform.position.x, Mathf.Lerp(doorModelCore.transform.position.y, minPositionOnY, progresDoorStatus), doorModelCore.transform.position.z);
        }
    }

    private void Update()
    {
        if (playerEnergy._currentEnergy < 1)
        {
            isOpenDoor = true;
            closeDoor = true;
        }

        if (closeDoor == true)
        {
            MovementDoor();

            if (progresDoorStatus >= 1)
            {
                closeDoor = false;
                progresDoorStatus = 0;
            }
        }
    }
}
