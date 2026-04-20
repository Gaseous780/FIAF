using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IEnemyBasics
{
    [SerializeField] protected int speed;
    [SerializeField] protected int returningSpeed;

    protected bool isWaiting;
    [SerializeField] protected float timeWait;

    protected SteeringBehaviours steering;

    protected LineOfSightBehaviour LOS;

    protected EnemyTree tree;
    protected EnemyContext context;

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

    protected bool isChecking;
    protected bool nextPointIsReturn;

    protected virtual void Awake()
    {
        canChangeDirection = false;
        changeRotation = false;
        rotationProgress = 0;
        isChecking = false;
        nextPointIsReturn = false;

        steering = new SteeringBehaviours();

        wanderDirection = transform.forward;

        LOS = GetComponent<LineOfSightBehaviour>();

        context = new EnemyContext { _isOn = true, _LOS = LOS, _selfTransform = transform, _returnToOrigin = false };

        DefineInitialGo();

        tree = new EnemyTree();
        tree.InitializeNodes();

        this.enabled = false;
    }

    protected virtual void Update()
    {

        tree.Evaluate(this, context);
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

    public virtual void Idle()
    {

    }

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

        if (LOS.IsOnFront(context._selfTransform))
        {
            InitToReturn();
            context._returnToOrigin = true;

            yield return null;
        }

        if (context._isWander == true)
        {
            context._isWander = false;
        }
        else
        {
            context._isWander = true;
        }
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
        isChecking = true;

        if (nextPointIsReturn == true)
        {
            context._isOn = false;
            this.enabled = false;
            return;
        }

        if (context._returnToOrigin == false)
        {
            positionsWents.Add(positionToGo);
            rotationsDone.Add(actualRotation);
            positionToGo = nextPositionToGo;
        }
        else
        {
            positionToGo = nextPositionToGo;
            actualRotation = -rotationsDone[rotationsDone.Count - 1];
            positionsWents.RemoveAt(positionsWents.Count - 1);
            rotationsDone.RemoveAt(rotationsDone.Count - 1);

            if (positionsWents.Count > 0)
            {
                nextPositionToGo = positionsWents[positionsWents.Count - 1];
            }
            else
            {
                nextPointIsReturn = true;
                positionToGo = initialPosition;
            }
        }

        changeRotation = true;

        ConvertForwardToDirection(actualRotation);

    }

    public virtual bool ChangeToNewRotation() //El parametro es si tiene que rotar de forma negativa o positiva (falso == negativo, true == positivo)
    {
        if (transform.forward != actualRotation)
        {
            if (context._returnToOrigin == false)
            {
                transform.forward = Vector3.Lerp(transform.forward, actualRotation, rotationProgress);
            }
            else
            {
                transform.forward = Vector3.Lerp(transform.forward, actualRotation, rotationProgress);
            }

            rotationProgress += Mathf.Clamp (Time.deltaTime * rotationSpeed, 0, 1);
            return true;
        }

        rotationProgress = 0;
        transform.forward = actualRotation;
        isChecking = false;
        return false;
    }

    public virtual void DefineInitialGo()
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

    protected void InitToReturn()
    {
        nextPositionToGo = positionsWents[positionsWents.Count - 1];
        actualRotation = -rotationsDone[rotationsDone.Count - 1];

        changeRotation = true;

        ConvertForwardToDirection(actualRotation);
    }

    public virtual void ReturnToPositionOrigin()
    {
        if (changeRotation == false)
        {
            CheckPosition();
            transform.position += transform.forward * returningSpeed * Time.deltaTime;
            //transform.position = Vector3.Lerp(transform.position, positionToGo.position, Time.deltaTime * speed);
        }
        else
        {
            changeRotation = ChangeToNewRotation();
        }
    }

    public virtual void Wander()
    {

    }

    public virtual void MoveRandom1() { }

    public virtual void MoveRandom2() { }

    public virtual void MoveRandom3() { }
}
