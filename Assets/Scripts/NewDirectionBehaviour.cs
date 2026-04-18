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
            Debug.Log("Bondiola");
        }
    }
}
