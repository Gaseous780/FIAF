using UnityEngine;

public class LineOfSightBehaviour : MonoBehaviour
{
    [SerializeField] private int distance = 33;
    [SerializeField] private float angle = 90;

    [SerializeField] private LayerMask doorsLayers;

    private Transform player;

    public bool IsRange(Transform self, Transform target)
    {
        return Vector3.Distance(self.position, target.position) < distance;
    }

    public bool IsAngle(Transform self, Transform target)
    {
        Vector3 dir = target.position - self.position;

        return Vector3.Angle(self.forward, dir) < angle / 2;
    }

    public bool IsObstacle(Transform self, Transform target)
    {
        Vector3 dir = target.position - self.position;

        return Physics.Raycast(self.position, dir.normalized, dir.magnitude, doorsLayers);
    }

    public bool IsOnFront (Transform self)
    {
        return Physics.Raycast(self.position, self.forward, self.forward.magnitude, doorsLayers);
    }
}
