using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public GameObject RandomEnemy => enemyList.Count==0 ? null : enemyList[Random.Range(0, enemyList.Count)];
    public int WaveNumber=>waveNumber;

    public float TimeBetweenWaves => timeBetweenWaves;
    [SerializeField]bool spawnEnemy = true;
    [SerializeField]GameObject waveUI;
    [SerializeField]GameObject[] enemyPrefabs;
    [SerializeField] float timeBetweenSpawns = 1f;
    [SerializeField]int minEnemyAmount=4;
    [SerializeField] int maxEnemyAmount = 10;
    [SerializeField] float timeBetweenWaves = 1f;
    int waveNumber = 1;
    int enemyAmount;
    List<GameObject> enemyList;
    WaitForSeconds waitTimeBetweenSpawns;
    WaitForSeconds waitTimeBetweenWaves;
    WaitUntil waitUntilNoEnemy;

    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>();
        waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
        waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
        waitUntilNoEnemy = new WaitUntil(()=>enemyList.Count==0);
    }
    //bool NoEnemy()
    //{
    //    return enemyList.Count == 0;
    //}
    IEnumerator Start()
    {
        while (spawnEnemy)
        {
            yield return waitUntilNoEnemy;
            waveUI.SetActive(true);
            yield return waitTimeBetweenWaves;
            waveUI.SetActive(false);
            yield return StartCoroutine(nameof(RandomSpawnCoroutine));
        }
        
    }
    IEnumerator RandomSpawnCoroutine()
    {
        enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber/3, maxEnemyAmount);
        for(int i = 0; i < enemyAmount; i++)
        {
            //var enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            enemyList.Add( PoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));//�����ɵ��˲���ӵ��б���

            yield return waitTimeBetweenSpawns;
        }
        waveNumber++;
    }
    public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);

}
