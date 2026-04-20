using System.Collections;
using TMPro;
using UnityEngine;

public class ConditionsManager : MonoBehaviour
{
    [SerializeField] GameObject UILose;
    [SerializeField] GameObject UIWin;

    [SerializeField] private float timeToShow;

    [SerializeField] private EnemyManager enemyManager;

    [SerializeField] private CamerasControllers controller;

    [SerializeField] private GameObject player;

    [SerializeField] private TextMeshProUGUI text;

    private int time;
    private int timeToWin;

    private void Awake()
    {
        time = -1;
        timeToWin = 6;
    }

    private void Start()
    {
        StartCoroutine(RunTime());
    }

    private void Update()
    {
        if (time == -1)
        {
            text.text = "23:00";
        }
        else
        {
            text.text = "0" + time.ToString() + ":00";
        }
    }

    public void ActivateLose(GameObject enemy)
    {
        if (controller._camerasOff == false)
        {
            controller.BackToWolrd();
        }
        StartCoroutine(EnableUI());
    }

    public void ActivateWinCondition()
    {
        enemyManager.DisableAllEnemies();
        player.GetComponent<PlayerController>().enabled = false;
        UIWin  .SetActive(true);
    }

    private IEnumerator RunTime()
    {
        yield return new WaitForSeconds(60f);

        time++;

        if (time >= timeToWin)
        {
            ActivateWinCondition();
        }

        StartCoroutine(RunTime());
    }

    private IEnumerator EnableUI()
    {
        yield return new WaitForSeconds(timeToShow);

        enemyManager.DisableAllEnemies();
        UILose.SetActive(true);
    }

    public void Reset()
    {
        GameManager.manager._sceneaController.ResetScene();
    }
}
