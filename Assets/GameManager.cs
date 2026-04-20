using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    private UIController uiController;

    private GameObject player;

    private SceneController sceneController;

    public UIController _uiController => uiController;
    public GameObject _player => player;
    public SceneController _sceneaController => sceneController;

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
        sceneController = GetComponentInChildren<SceneController>();

        FindReferences();
    }

    public void FindReferences()
    {
        uiController = GameObject.FindAnyObjectByType<UIController>();
        player = GameObject.FindAnyObjectByType<PlayerController>().gameObject;
    }

    public void FindReferences(Scene scene, LoadSceneMode mode)
    {
        uiController = GameObject.FindAnyObjectByType<UIController>();
        player = GameObject.FindAnyObjectByType<PlayerController>().gameObject;
    }
}
