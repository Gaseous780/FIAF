using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private bool isOnCameras;
    private int sector; //0 = Esta viendo en el sector A, 1 = Esta viendo en el sector B. -1 = Cuando no esta en ninguno de los sectores

    public bool _isOnCameras { get { return isOnCameras; } set { isOnCameras = value; } }
    public int _sector { get { return sector; } set { sector = value; } }

    private void Awake()
    {
        isOnCameras = false;

        sector = -1;
    }
}
