using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected int speed;
    [SerializeField] protected int returningSpeed;
    [SerializeField] protected int mode;

    protected bool isWaiting;
    [SerializeField] protected float timeWait;

    protected SteeringBehaviours steering;

    protected LineOfSightBehaviour LOS;

    protected EnemyTree tree;
    protected EnemyContext context;

    protected List<Vector3> PositionsWent;

    [SerializeField] protected Transform initialPosition;
    [SerializeField] protected Transform positionToGo;
    [SerializeField] protected Transform nextPositionToGo;

    [SerializeField] protected Vector3 actualRotation;
    [SerializeField] protected Vector3 nextRotation;
    protected bool canChangeDirection;
    protected bool changeRotation;
    protected int forwardTo; //0 adelante, 1 derecha, 2 atras, 3 izquierda

    [SerializeField] protected List<Transform> initialPositionsToGo;
    [SerializeField] protected List<Vector3> initialRotations;

    [SerializeField] protected List<Transform> positionsWents; //Sirve para el regreso de los enemigos a su posición original
    [SerializeField] protected List<Vector3> rotationsDone;

    //Lo que probablemente se vaya
    [SerializeField] protected Transform player;
    [SerializeField] protected Rigidbody playerRB;

    [SerializeField] protected float rotationProgress;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected float returningRotationSpeed;
    protected Vector3 wanderDirection;
    protected float wanderTime;
    [SerializeField] protected float wanderChangeInterval;

    protected virtual void Awake()
    {
        canChangeDirection = false;
        changeRotation = false;
        rotationProgress = 0;

        DefineInitialGo();

        steering = new SteeringBehaviours();

        wanderDirection = transform.forward;

        LOS = GetComponent<LineOfSightBehaviour>();

        context = new EnemyContext { _isOn = true, _LOS = LOS, _selfTransform = transform, _returnToOrigin = false };

        tree = new EnemyTree();
        tree.InitializeNodes();
    }

    protected virtual void Update()
    {
        //Vector3 dir = Vector3.zero;
        //switch (mode)
        //{
        //    case 0://persue
        //        dir = steering.Seek(transform, player.position);
        //        break;
        //    case 1:
        //        dir = steering.Flee(transform, player.position);
        //        break;
        //    case 2:
        //        dir = steering.Arrive(transform, player.position, 5f);
        //        break;
        //    case 3:
        //        dir = steering.Pursue(transform, player, playerRB, 5f);
        //        break;
        //    case 4:
        //        dir = steering.Evade(transform, player, playerRB, 0.5f);
        //        break;
        //    case 5://patrullaje
        //        wanderTime -= Time.deltaTime;
        //        if (wanderTime <= 0f)
        //        {
        //            wanderDirection = steering.Wander(wanderDirection, 180f);
        //            wanderTime = wanderChangeInterval;
        //        }
        //        dir = wanderDirection;
        //        break;
        //}
        //if (LOS.IsOnFront(transform) == false)
        //{
        //Move(dir);
        //}

        tree.Evaluate(this, context);
    }

    public virtual void Move(/*Vector3 dir*/)
    {
        if (changeRotation == false)
        {
            CheckPosition();
            transform.position += transform.forward * speed * Time.deltaTime;
            //transform.position = Vector3.Lerp(transform.position, positionToGo.position, Time.deltaTime * speed);
        }
        else
        {
            changeRotation = ChangeToNewRotation();
        }

        //Debug.Log(transform.forward);
        //Debug.Log(transform.newRotation);


        //if (dir != Vector3.zero)
        //{
        //    transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * rotationSpeed);
        //}
    }

    public virtual void Iddle()
    {

    }

    public virtual void ReturnToOrigin() { }

    public virtual void StartWait()
    {
        if (isWaiting == false)
        {
            StartCoroutine(Wait());
        }
    }

    public virtual IEnumerator Wait()
    {
        isWaiting = true;
        yield return new WaitForSeconds(timeWait);
        isWaiting = false;
    }

    public virtual void NewDirection(Transform newDirectionToGo, Vector3 newRotation)
    {
        if (context._returnToOrigin == false)
        {
            if (canChangeDirection == true)
            {
                nextPositionToGo = newDirectionToGo;
                actualRotation = newRotation;

                canChangeDirection = false;
            }
            else
            {
                canChangeDirection = true;
            }
        }
    }

    public virtual void ChangeDirection()
    {
        Debug.Log("mEcAMBIO");
        positionsWents.Add(positionToGo);
        positionToGo = nextPositionToGo;

        rotationsDone.Add(transform.forward);
        changeRotation = true;

        ConvertForwardToDirection(actualRotation);

    }

    public virtual bool ChangeToNewRotation() //El parametro es si tiene que rotar de forma negativa o positiva (falso == negativo, true == positivo)
    {
        Debug.Log("mEROTO");
        if (transform.forward != actualRotation)
        {
            transform.forward = Vector3.Lerp(transform.forward, actualRotation, rotationProgress);
            rotationProgress = Time.deltaTime * rotationSpeed;
            return true;
        }

        rotationProgress = 0;
        transform.forward = actualRotation;
        return false;
    }

    protected virtual void DefineInitialGo()
    {
        int directionToGo = Random.Range(0, initialPositionsToGo.Count);

        nextPositionToGo = initialPositionsToGo[directionToGo];
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
        switch (forwardTo)
        {
            case 0:
                if (transform.position.z <= positionToGo.position.z)
                {
                    ChangeDirection();
                    Debug.Log("meca");
                }
                break;

            case 1:
                if (transform.position.x >= positionToGo.position.x)
                {
                    Debug.Log("meca");
                    ChangeDirection();
                }
                break;

            case 2:
                if (transform.position.z >= positionToGo.position.z)
                {
                    Debug.Log("meca");
                    ChangeDirection();
                }
                break;

            case 3:
                if (transform.position.x <= positionToGo.position.x)
                {
                    Debug.Log("meca");
                    ChangeDirection();
                }
                break;
        }
    }
}
