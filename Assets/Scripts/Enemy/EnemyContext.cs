using UnityEngine;

public class EnemyContext
{
    private Transform selfTransform;
    private Transform playerTransform;
    private bool isOn;
    private bool returnToOrigin;
    private LineOfSightBehaviour LOS;
    private Vector3 [] directionToGo;

    public Transform _selfTransform { get { return selfTransform; } set { selfTransform = value; } }
    public Transform _playerTransform { get {return playerTransform; } set { playerTransform = value; } }
    public bool _isOn { get { return isOn; } set { isOn = value; } }
    public bool _returnToOrigin { get { return returnToOrigin; } set { returnToOrigin = value; } }
    public LineOfSightBehaviour _LOS { get { return LOS; } set { LOS = value; } }
    public Vector3 [] _directionToGo { get { return directionToGo; } set { directionToGo = value; } }
}
