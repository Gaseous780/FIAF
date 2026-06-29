using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewDirectionBehaviour : MonoBehaviour
{
    [SerializeField] private List <Transform> newPosition;
    [SerializeField] private List <Vector3> newDirection;

    [SerializeField] private NewDirectionBehaviour[] neightBourds; 
    private List<Node> neightbourdsToTransfer;

    private Node thisNode;
    [SerializeField] private bool isShortcut;

    public Node _thisNode => thisNode;

    private void Awake()
    {
        thisNode = new Node(transform.position,gameObject.name, isShortcut);
        neightbourdsToTransfer = new List<Node>();
    }

    private void Start()
    {
        foreach (NewDirectionBehaviour neightbour in neightBourds)
        {
            neightbourdsToTransfer.Add(neightbour._thisNode);
        }

        thisNode._neightbourds = neightbourdsToTransfer;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.GetComponent<ChairEnemy>() != null)
        {
            other.gameObject.GetComponent<ChairEnemy>()._actualNode = thisNode;
        }

        if (newPosition.Count <= 0) { return; }

        if (other.CompareTag("Enemy") == true)
        {
            other.GetComponent<EnemyBase>().NewDirection(newPosition[0], newDirection[0]);
        }
        else if (other.CompareTag ("Ghost") == true)
        {
            other.GetComponent<Ghost>().ReciveNextOnes(newPosition, newDirection);
        }
        else if (other.CompareTag("Daemon") == true)
        {
            other.GetComponent<Daemon>().NewDirection(newPosition, newDirection);
        }
    }
}
