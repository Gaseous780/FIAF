using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewDirectionBehaviour : MonoBehaviour
{
    [SerializeField] private List <Transform> newPosition;
    [SerializeField] private List <Vector3> newDirection;

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
