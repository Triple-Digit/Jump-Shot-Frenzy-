using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    SpawnPoint[] m_spawnPoints;

    
    public static SpawnManager Instance;

    private void Awake()
    {
        #region Singleton Pattern
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
        #endregion

        m_spawnPoints = GetComponentsInChildren<SpawnPoint>();
    }

    public Transform GetSpawnPoint()
    {
        return m_spawnPoints[Random.Range(0, m_spawnPoints.Length)].transform;
    }




}
