using System.Collections;
using UnityEngine;

public class ConditionsManager : MonoBehaviour
{
    [SerializeField] GameObject UILose;

    [SerializeField] private float timeToShow;

    [SerializeField] private EnemyManager enemyManager;

    [SerializeField] private CamerasControllers controller;

    public void ActivateLose(GameObject enemy)
    {
        if (controller._camerasOff == false)
        {
            controller.BackToWolrd();
        }
        StartCoroutine(EnableUI());
    }

    private IEnumerator EnableUI()
    {
        yield return new WaitForSeconds(timeToShow);

        UILose.SetActive(true);
    }

    public void Reset()
    {
        GameManager.manager._sceneaController.ResetScene();
    }
}
