using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairEnemy : EnemyBase
{
    //[SerializeField] private float speed = 10;

    private SteeringBehaviours steeringBehaviours;

    [SerializeField] private List<Node> positionsToGo;

    //private float rotationProgress;
    //[SerializeField] private float rotationSpeed;

    private BossTree bossTree;

    [SerializeField] private EnemyManager manager;

    private Node actualNode;

    [SerializeField]private NewDirectionBehaviour firstNode;

    [SerializeField] private float timeToReactivate = 5f;

    [SerializeField] private GameObject[] models; 

    private bool processing;

    public NewDirectionBehaviour _firstNode { get => firstNode; set => firstNode = value; }
    public Node _actualNode { get => actualNode; set => actualNode = value; }

    protected override void Awake()
    {
        steeringBehaviours = new SteeringBehaviours();
        LOS = GetComponent<LineOfSightBehaviour>();
        rotationProgress = 0;

        processing = false;

        context = new EnemyContext() { _isOn = true, _LOS = LOS, _selfTransform = transform, _returnToOrigin = false, _decisionMoment = false, _isWander = true, _hasBreakDoor = false, _hasThinkMove = false, _mustReturn = false };

        bossTree = new BossTree();
        bossTree.InitializeNodes();

        models[0].SetActive(true);
        models[1].SetActive(false);
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
        if (positionsToGo.Count <= 0) { return; }

        if (changeRotation == false)
        {
            Vector3 direction = steeringBehaviours.Seek(transform, positionsToGo[0]._position);
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, positionsToGo[0]._position) < 1)
            {
                if (positionsToGo.Count >= 2)
                {
                    rotationProgress = 0;
                    actualRotation = transform.forward;
                    nextRotation = steeringBehaviours.Seek(transform, positionsToGo[1]._position);
                    changeRotation = true;
                }
            }
        }
        else
        {
            changeRotation = ChangeToNewRotation();
        }
    }

    public override void SetPath()
    {
        positionsToGo = manager._pathCreator.SetPathTheta(actualNode, firstNode._thisNode);
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
        if (processing == false)
        {
            processing = true;

            positionsToGo[1]._gameObjectToDisable.SetActive(false);

            StartCoroutine(ReActivateGameObject(positionsToGo[1]._gameObjectToDisable));

            SetPath();

            context._hasBreakDoor = true;
        }
    }

    private IEnumerator ReActivateGameObject (GameObject gameObject)
    {
        yield return new WaitForSeconds (timeToReactivate);

        gameObject.SetActive (true);
    }

    public override void SetMode()
    {
        Debug.Log("Pase");

        models[0].SetActive(false);
        models[1].SetActive(true);

        speed *= 2;
        rotationSpeed *= 2;

        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, transform.forward.magnitude, LayerMask.GetMask("Water"));
        hit.collider.gameObject.SetActive(false);

        context._hasBreakDoor = false;
        StartCoroutine(ReActivateGameObject(hit.collider.gameObject));
        processing = false;
        context._returnToOrigin = true;
    }

    public override void ReturnToPositionOrigin()
    {
        if (positionsToGo.Count <= 0) 
        {
            context._isOn = false;
            speed /= 2;
            rotationSpeed /= 2;
            context._hasThinkMove = false;
            context._mustReturn = false;

            StartCoroutine(ReActivate());

            return;
        }
        Move();
    }

    public override void SetReturnToPositionOrigin()
    {
        positionsToGo = manager._pathCreator.SetPathAStar(actualNode);

        context._mustReturn = true;
        processing = false;
        context._hasBreakDoor = false;
        context._returnToOrigin = false;

        models[0].SetActive(true);
        models[1].SetActive(false);
    }

    private IEnumerator ReActivate()
    {
        yield return new WaitForSeconds(30);

        context._isOn = true;
    }

    public override void Idle()
    {
        
    }
}
