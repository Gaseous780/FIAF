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

    private void Awake()
    {
        maxPositionOnY = doorModelCore.transform.position.y;

        closeDoor = false;
        progresDoorStatus = 0;
    }

    public void InteractDoor()
    {
        if (isOpenDoor == true)
        {
            isOpenDoor = false;
        }
        else
        {
            isOpenDoor= true;
        }

        closeDoor = true;
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
