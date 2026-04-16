using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected int speed;
    [SerializeField] protected int mode;

    protected SteeringBehaviours steering;

    protected LineOfSightBehaviour LOS;

    protected EnemyTree tree;
    protected EnemyContext context;

    protected List<Vector3> PositionsWent;

    //Lo que probablemente se vaya
    [SerializeField] protected Transform player;
    [SerializeField] protected Rigidbody playerRB;
    [SerializeField] protected float rotationSpeed;
    protected Vector3 wanderDirection;
    protected float wanderTime;
    [SerializeField] protected float wanderChangeInterval;

    protected virtual void Awake()
    {
        steering = new SteeringBehaviours();

        wanderDirection = transform.forward;

        LOS = GetComponent<LineOfSightBehaviour>();

        context = new EnemyContext { _isOn = true, _LOS = LOS, _selfTransform = transform, _returnToOrigin = false};

        tree = GetComponent<EnemyTree>();
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
        transform.position += transform.forward * speed * Time.deltaTime;

        //if (dir != Vector3.zero)
        //{
        //    transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * rotationSpeed);
        //}
    }

    public virtual void Iddle ()
    {

    }

    public virtual void ReturnToOrigin() { }

    public virtual IEnumerator Wait() {
        Debug.Log("waiting");
        yield return null; }


}
