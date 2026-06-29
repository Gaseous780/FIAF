using System.Collections.Generic;
using UnityEngine;

public class ChairEnemy : EnemyBase
{
    //[SerializeField] private float speed = 10;

    private SteeringBehaviours steeringBehaviours;

    [SerializeField] private List<Vector3> positionsToGo;

    //private float rotationProgress;
    //[SerializeField] private float rotationSpeed;

    private BossTree bossTree;

    [SerializeField] private EnemyManager manager;

    private Node actualNode;

    [SerializeField]private NewDirectionBehaviour firstNode;

    public NewDirectionBehaviour _firstNode { get => firstNode; set => firstNode = value; }
    public Node _actualNode { get => actualNode; set => actualNode = value; }

    protected override void Awake()
    {
        steeringBehaviours = new SteeringBehaviours();
        LOS = GetComponent<LineOfSightBehaviour>();
        rotationProgress = 0;

        context = new EnemyContext() { _isOn = true, _LOS = LOS, _selfTransform = transform, _returnToOrigin = false, _decisionMoment = false, _isWander = true, _hasBreakDoor = false, _hasThinkMove = false };

        bossTree = new BossTree();
        bossTree.InitializeNodes();
    }

    void Start()
    {
        actualNode = firstNode._thisNode;
    }

    // Update is called once per frame
    protected override void Update()
    {
        bossTree.Evaluate(this, context);
    }

    public override void Move()
    {
        //Debug.Log(positionsToGo.Count);
        if (changeRotation == false)
        {
            Vector3 direction = steeringBehaviours.Seek(transform, positionsToGo[0]);
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, positionsToGo[0]) < 1)
            {
                rotationProgress = 0;
                actualRotation = transform.forward;
                nextRotation = steeringBehaviours.Seek(transform, positionsToGo[1]);
                changeRotation = true;
            }
        }
        else
        {
            changeRotation = ChangeToNewRotation();
        }
    }

    public override void SetPath()
    {
        positionsToGo = manager._pathCreator.SetPathTheta(actualNode);
        context._hasThinkMove = true;
    }

    public override bool ChangeToNewRotation()
    {
        if (rotationProgress >= 0.99f) 
        {
            positionsToGo.RemoveAt(0);
            changeRotation = false;
            return false; 
        }

        transform.forward = Vector3.Lerp(actualRotation, nextRotation, rotationProgress);
        rotationProgress += rotationSpeed * Time.deltaTime;
        return true;
    }

    public override void DestroyDoor()
    {
        Debug.Log("Destroy");
    }

    public override void SetMode()
    {
        Debug.Log("Set");
    }

    public override void ReturnToPositionOrigin()
    {
        Debug.Log("Return");
    }

    //private void Move()
    //{
    //    if (positionToGo.Count <= 0) { return; }

    //    Vector3 direction = steeringBehaviours.Seek(transform, positionToGo[0].position);
    //    transform.position += direction * speed * Time.deltaTime;

    //    if (Vector3.Distance(transform.position, positionToGo[0].position) > 4)
    //    {
    //        rotationProgress = 0;
    //        previousRotaion = transform.forward;
    //        toRotate = steeringBehaviours.Seek(transform, positionToGo[1].position);
    //    }
    //    else
    //    {
    //        Rotation();
    //    }

    //    if (Vector3.Distance(transform.position, positionToGo[0].position) < 1)
    //    {
    //        positionToGo.RemoveAt(0);
    //    }
    //}

    //private void Rotation()
    //{
    //    if (rotationProgress > 0.99f) { return; }

    //    transform.forward = Vector3.Lerp(previousRotaion, toRotate, rotationProgress);
    //    rotationProgress += rotationSpeed * Time.deltaTime;
    //}
}
