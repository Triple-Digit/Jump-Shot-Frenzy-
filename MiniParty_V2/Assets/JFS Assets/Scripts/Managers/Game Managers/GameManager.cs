using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<PlayerManager> m_activePlayers = new List<PlayerManager>();

    [Header("Game State atrributes")]
    public bool m_matchActive = false;
    public bool m_endMatch = false;
    float m_timer = 0.0f;


    private void Awake()
    {
        SingletonPattern();
        m_timer = 3f;
        StartMatch();

    }

    void SingletonPattern()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (instance)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    private void FixedUpdate()
    {
        //Timer();
       
        
    }

    public static void AddActivePlayer(PlayerManager player)
    {
        instance.m_activePlayers.Add(player);
    }

    public static void RemoveActivePlayer(PlayerManager player)
    {
        instance.m_activePlayers.Remove(player);
    }

    void StartMatch()
    {
        //Start the match usisng a countdown and then start a timer that counts down for 5mins or less
        m_matchActive = true;
    }

    void EndMatch()
    {
        //End round
        m_endMatch = true;
    }

    void CheckMatchStatus()
    {
        if(m_activePlayers.Count == 1 || m_timer <= 0)
        {
            EndMatch();
        }
    }

    int Timer(float t)
    {
        if(t > 0)
        {
            t -= Time.deltaTime;            
        }

        int seconds = Convert.ToInt32(t % 60);
        return seconds;
    }
}
