using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewDirectionBehaviour : MonoBehaviour
{
    [SerializeField] private List <Transform> newPosition;
    [SerializeField] private List <Vector3> newDirection;

    [SerializeField] private List<Node> neightbourdsToTransfer;

    private Node thisNode;

    public Node _thisNode => thisNode;

    private void Start()
    {
        thisNode = new Node(neightbourdsToTransfer);
    }

    private void OnTriggerEnter(Collider other)
    {
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
