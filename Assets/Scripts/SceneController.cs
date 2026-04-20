using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private GameManager manager;

    private void Start()
    {
        manager = GetComponentInParent<GameManager>();

        SceneManager.sceneLoaded += manager.FindReferences;
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
