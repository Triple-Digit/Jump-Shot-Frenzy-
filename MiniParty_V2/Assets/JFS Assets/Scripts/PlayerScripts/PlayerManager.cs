using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] PhotonView m_photonView;
    GameObject m_controller;

    private void Awake()
    {
        m_photonView = GetComponent<PhotonView>();
        GameManager.AddActivePlayer(this);
    }

    private void Start()
    {
        if(m_photonView.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        Transform spawnPoint = SpawnManager.Instance.GetSpawnPoint();
        m_controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnPoint.position, spawnPoint.rotation, 0, new object[] { m_photonView.ViewID });
    }

    public void Die()
    {
        PhotonNetwork.Destroy(m_controller);
        GameManager.RemoveActivePlayer(this);
        CreateController();
    }

    


}
