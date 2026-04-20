using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private FSMClass fsm;

    private LineOfSightBehaviour los;

    private GameObject player;

    private bool hasSeen;
    private SteeringBehaviours steering;

    [SerializeField] private float speed;
    [SerializeField] private float arriveSpeed;
    [SerializeField] private float rotationSpeed;

    private float defaultSpeed;

    private float forwardTo;

    private bool canChangeDirection;
    private bool changeRotation;

    private bool isChecking;

    private Vector3 actualRotation;
    private float rotationProgress;

    [SerializeField] private List<Transform> positionsWents;
    [SerializeField] private List<Vector3> rotationsDone;

    [SerializeField] private Transform positionToGo;

    [SerializeField] private List<Transform> initialPositionsToGo;
    [SerializeField] private List<Vector3> initialRotations;

    [SerializeField] private Transform nextPositionToGo;

    private int playerSector;

    private void Awake()
    {
        hasSeen = false;

        steering = new SteeringBehaviours();
        fsm = new FSMClass();
        los = GetComponent<LineOfSightBehaviour>();

        fsm.InitializeFSM();

        defaultSpeed = speed;

        canChangeDirection = false;
        changeRotation = false;
        rotationProgress = 0;
        isChecking = false;
    }

    private void OnEnable()
    {
        speed = defaultSpeed;
    }

    private void Start()
    {
        DefineInitialGo();

        StartCoroutine(GetPlayer());
    }

    private IEnumerator GetPlayer()
    {
        yield return new WaitForSeconds(0.3f);

        player = GameManager.manager._player;
    }

    private void Update()
    {
        if (player != null)
        {
            bool playerUsingCameras = player.GetComponent<PlayerBehaviour>()._isOnCameras;

            fsm.UpdateState(playerUsingCameras, hasSeen);

            ExecuteState();
        }
    }

    public void ExecuteState()
    {
        if (fsm._currentState is ArriveState)
        {
            ArriveMovement();
        }
        else if (fsm._currentState is EvadeState)
        {
            EvadeMovement();
        }
        else if (fsm._currentState is HideState)
        {
            Hide();
        }
        else if (fsm._currentState is DisapearState)
        {
            Disapear();
        }
    }

    public void ArriveMovement()
    {
        speed = defaultSpeed * steering.Arrive(transform, player.transform.position, 8f);
        Debug.Log(speed);
        Move();
    }

    public void EvadeMovement()
    {
        speed = defaultSpeed;
        Move();
    }

    public void Hide()
    {

    }

    public void Disapear()
    {

    }

    public virtual void Move()
    {
        if (changeRotation == false)
        {
            CheckPosition();
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else
        {
            changeRotation = ChangeToNewRotation();
        }
    }

    public void ReciveNextOnes(List<Transform> newDirectionToGo, List<Vector3> newRotation)
    {
        if (fsm._currentState is ArriveState)
        {
            NewDirection(newDirectionToGo, newRotation);
        }
        else if (fsm._currentState is EvadeState)
        {
            NewDirectionEvade(newDirectionToGo, newRotation);
        }
    }

    public virtual void NewDirection(List<Transform> newDirectionToGo, List<Vector3> newRotation)
    {
        if (canChangeDirection == true)
        {
            nextPositionToGo = newDirectionToGo[newDirectionToGo.Count - 1];
            actualRotation = newRotation[newRotation.Count - 1];

            canChangeDirection = false;
        }
        else
        {
            canChangeDirection = true;
        }
    }

    public virtual void NewDirectionEvade(List<Transform> newDirectionToGo, List<Vector3> newRotation)
    {
        if (canChangeDirection == true)
        {
            playerSector = player.GetComponent<PlayerBehaviour>()._sector;

            nextPositionToGo = newDirectionToGo[0];
            actualRotation = newRotation[0];

            if (newDirectionToGo.Count > 1)
            {
                switch (playerSector)
                {
                    case 0:
                        nextPositionToGo = newDirectionToGo[1];
                        actualRotation = newRotation[1];
                        break;

                    case 1:
                        nextPositionToGo = newDirectionToGo[0];
                        actualRotation = newRotation[0];
                        break;

                    default:

                        nextPositionToGo = newDirectionToGo[1];
                        actualRotation = newRotation[1];
                        if (newDirectionToGo.Count > 2)
                        {
                            nextPositionToGo = newDirectionToGo[2];
                            actualRotation = newRotation[2];
                        }
                        break;
                }
            }

            canChangeDirection = false;
        }
        else
        {
            canChangeDirection = true;
        }
    }

    public virtual void ChangeDirection()
    {
        isChecking = true;

        positionsWents.Add(positionToGo);
        rotationsDone.Add(actualRotation);
        positionToGo = nextPositionToGo;

        changeRotation = true;

        ConvertForwardToDirection(actualRotation);

    }

    public virtual bool ChangeToNewRotation() //El parametro es si tiene que rotar de forma negativa o positiva (falso == negativo, true == positivo)
    {
        if (transform.forward != actualRotation)
        {
            transform.forward = Vector3.Lerp(transform.forward, actualRotation, rotationProgress);


            rotationProgress += Mathf.Clamp(Time.deltaTime * rotationSpeed, 0, 1);
            return true;
        }

        rotationProgress = 0;
        transform.forward = actualRotation;
        isChecking = false;
        return false;
    }

    protected virtual void DefineInitialGo()
    {
        int directionToGo = Random.Range(0, initialPositionsToGo.Count);

        nextPositionToGo = initialPositionsToGo[directionToGo];
        positionToGo = nextPositionToGo;
        actualRotation = initialRotations[directionToGo];

        ChangeDirection();
    }

    protected virtual void ConvertForwardToDirection(Vector3 directionForward)
    {
        if (directionForward == new Vector3(0, 0, -1))
        {
            forwardTo = 0;
        }
        else if (directionForward == new Vector3(1, 0, 0))
        {
            forwardTo = 1;
        }
        else if (directionForward == new Vector3(0, 0, 1))
        {
            forwardTo = 2;
        }
        else if (directionForward == new Vector3(-1, 0, 0))
        {
            forwardTo = 3;
        }
    }

    protected void CheckPosition()
    {
        if (isChecking == false)
        {
            switch (forwardTo)
            {
                case 0:
                    if (transform.position.z <= positionToGo.position.z)
                    {
                        ChangeDirection();
                    }
                    break;

                case 1:
                    if (transform.position.x >= positionToGo.position.x)
                    {
                        ChangeDirection();
                    }
                    break;

                case 2:
                    if (transform.position.z >= positionToGo.position.z)
                    {
                        ChangeDirection();
                    }
                    break;

                case 3:
                    if (transform.position.x <= positionToGo.position.x)
                    {
                        ChangeDirection();
                    }
                    break;
            }
        }
    }
}
