using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInputs;

    [SerializeField] private float distance;

    private void Awake()
    {
        playerInputs = GetComponent<PlayerInput>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveMouse (InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
        }
    }

    public void ClickAction (InputAction.CallbackContext context)
    {
        if (context.started)
        {
            RaycastHit raycast;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), distance));

            if (Physics.Raycast(ray, out raycast))
            {
                GameObject objectRaycast = raycast.collider.gameObject;

                if (objectRaycast.CompareTag("Door") == true)
                {
                    objectRaycast.GetComponent<DoorBehaviour>().InteractDoor();
                }
            }
        }
    }
}
