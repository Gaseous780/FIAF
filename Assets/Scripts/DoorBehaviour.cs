using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    private bool isOpenDoor = false;

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

        Debug.Log(isOpenDoor);
    }
}
