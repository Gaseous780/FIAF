using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daemon : EnemyBase
{
    private EnemyTreeWeight treeWeight;

    [SerializeField] private float speedRandom1;
    [SerializeField] private float speedRandom2;
    [SerializeField] private float speedRandom3;

    [SerializeField] private int wanderSpeed;

    private bool takingADecition;

    private (List<Transform> newDirectionToGo, List<Vector3> newRotation) temporalReferences;

    protected override void Awake()
    {
        canChangeDirection = false;
        changeRotation = false;
        rotationProgress = 0;
        isChecking = false;
        nextPointIsReturn = false;

        takingADecition = false;

        treeWeight = new EnemyTreeWeight();
        treeWeight.InitTree();

        wanderDirection = transform.forward;

        LOS = GetComponent<LineOfSightBehaviour>();

        context = new EnemyContext { _isOn = true, _LOS = LOS, _selfTransform = transform, _returnToOrigin = false, _decisionMoment = false, _isWander = true};

        DefineInitialGo();

        this.enabled = false;
    }

    protected override void Update()
    {
        treeWeight.Evaluate(this, context);
    }

    public void NewDirection(List <Transform> newDirectionToGo, List <Vector3> newRotation)
    {
        if (context._returnToOrigin == false)
        {
            if (canChangeDirection == true)
            {
                temporalReferences = (newDirectionToGo, newRotation);
                context._decisionMoment = true;
            }
            else
            {
                 canChangeDirection = true;
            }
        }
    }

    public override void MoveRandom1()
    {
        if (takingADecition == true)
        {
            takingADecition = false;

            nextPositionToGo = temporalReferences.newDirectionToGo[0];
            actualRotation = temporalReferences.newRotation[0];

            canChangeDirection = false;

            speed = (int)speedRandom1;

            context._decisionMoment = false;
            takingADecition = false;
        }
    }

    public override void MoveRandom2()
    {
        if (takingADecition == true)
        {
            takingADecition = false;

            if (temporalReferences.newDirectionToGo.Count > 1)
            {
                nextPositionToGo = temporalReferences.newDirectionToGo[1];
                actualRotation = temporalReferences.newRotation[1];
            }
            else
            {
                nextPositionToGo = temporalReferences.newDirectionToGo[0];
                actualRotation = temporalReferences.newRotation[0];
            }
            canChangeDirection = false;

            speed = (int)speedRandom2;

            context._decisionMoment = false;
            takingADecition = false;
        }
    }

    public override void MoveRandom3()
    {
        if (takingADecition == true)
        {
            takingADecition = false;

            if (temporalReferences.newDirectionToGo.Count > 2)
            {
                nextPositionToGo = temporalReferences.newDirectionToGo[2];
                actualRotation = temporalReferences.newRotation[2];
            }
            else if (temporalReferences.newDirectionToGo.Count > 1)
            {
                nextPositionToGo = temporalReferences.newDirectionToGo[1];
                actualRotation = temporalReferences.newRotation[1];
            }
            else
            {
                nextPositionToGo = temporalReferences.newDirectionToGo[0];
                actualRotation = temporalReferences.newRotation[0];
            }

            canChangeDirection = false;

            speed = (int)speedRandom3;

            context._decisionMoment = false;
            takingADecition = false;
        }
    }

    public override void DefineInitialGo()
    {
        base.DefineInitialGo();

        context._isWander = true;
        ConvertForwardToDirection(initialRotations[0]);
    }

    public override void Wander()
    {
        if (changeRotation == false)
        {
            NextWander();
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else
        {
            changeRotation = ChangeToNewRotation();
        }
    }

    private void NextWander()
    {
        if (forwardTo == 1)
        {
            if (transform.position.x >= positionToGo.position.x)
            {
                positionToGo = temporalReferences.newDirectionToGo[0];
                actualRotation = temporalReferences.newRotation[0];

                ConvertForwardToDirection(actualRotation);
                changeRotation = true;
            }
        }
        else
        {
            if (transform.position.x <= positionToGo.position.x)
            {
                positionToGo = temporalReferences.newDirectionToGo[1];
                actualRotation = temporalReferences.newRotation[1];

                ConvertForwardToDirection(actualRotation);
                changeRotation = true;
            }
        }
    }

    public override IEnumerator Wait()
    {
        isWaiting = true;
        yield return new WaitForSeconds(timeWait);

        if (LOS.IsOnFront(context._selfTransform))
        {
            InitToReturn();
            context._returnToOrigin = true;

            yield return null;
        }
        else
        {
            context._isWander = false;
        }

        isWaiting = false;
    }
}
