using UnityEngine;

public class LoseCondition : MonoBehaviour
{
    [SerializeField] private ConditionsManager conditionsManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag ("Enemy") == true || other.CompareTag ("Ghost") == true)
        {
            conditionsManager.ActivateLose(other.gameObject);
        }
    }
}
