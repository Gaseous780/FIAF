using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    private UIController uiController;

    private GameObject player;

    public UIController _uiController => uiController;
    public GameObject _player => player;

    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        FindReferences();
    }

    public void FindReferences()
    {
        uiController = GameObject.FindAnyObjectByType<UIController>();
        player = GameObject.FindAnyObjectByType<PlayerController>().gameObject;
    }
}
