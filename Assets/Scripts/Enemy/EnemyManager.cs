using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;

    [SerializeField] private float maxTimeToActivate;
    [SerializeField] private float minTimeToActivate;

    void Start()
    {
        StartCoroutine(TimeToInitEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TimeToInitEnemy()
    {
        float timeToEnable = Random.Range(minTimeToActivate, maxTimeToActivate);
        yield return new WaitForSeconds(timeToEnable);

        EnableOneEnemy();
    }

    private void EnableOneEnemy()
    {
        int enemyToActivate = Random.Range(0, enemies.Length);

        if (enemies[enemyToActivate].GetComponent<EnemyBase>() != null && enemies[enemyToActivate].GetComponent<EnemyBase>().enabled == false)
        {
            enemies[enemyToActivate].GetComponent<EnemyBase>().enabled = true;
            IEnemyBasics enemy = enemies[enemyToActivate].GetComponent<EnemyBase>() as IEnemyBasics;

            enemy.DefineInitialGo();
        }
        else if (enemies[enemyToActivate].GetComponent<Ghost>() != null && enemies[enemyToActivate].GetComponent<Ghost>().enabled == false)
        {
            enemies[enemyToActivate].GetComponent<Ghost>().enabled = true;
            IEnemyBasics enemy = enemies[enemyToActivate].GetComponent<Ghost>() as IEnemyBasics;

            enemy.DefineInitialGo();
        }

        StartCoroutine(TimeToInitEnemy());
    }

    public void DisableAllEnemies() 
    { 
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<EnemyBase>() != null)
            {
                enemy.GetComponent<EnemyBase>().StopAllCoroutines();
                enemy.GetComponent<EnemyBase>().enabled = false;
            }
            else if (enemy.GetComponent<Ghost>() != null)
            {
                enemy.GetComponent<Ghost>().StopAllCoroutines();
                enemy.GetComponent<Ghost>().enabled = false;
            }
        }
    }
}
