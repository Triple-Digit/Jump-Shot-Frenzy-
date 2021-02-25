using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhysics : MonoBehaviourPunCallbacks
{

    [SerializeField] float m_bulletSpeed = 5f;
    [SerializeField] float m_damageAmount = 1f;
    [SerializeField] Rigidbody2D m_rigidbody2D;
    PhotonView m_photonView;

    private void Awake()
    {
        m_photonView = GetComponent<PhotonView>();
        
    }
       

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DestroyBullet();
        if (!photonView.IsMine) return;
        PhotonView target = collision.gameObject.GetComponent<PhotonView>();
        if(target != null && (!target.IsMine || target.IsRoomView))
        {
            if(target.tag == "Player")
            {
                target.RPC("RPC_TakeDamage", RpcTarget.AllBuffered, m_damageAmount);
            }
            this.GetComponent<PhotonView>().RPC("DestroyBullet", RpcTarget.AllBuffered);
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.right * m_bulletSpeed * Time.deltaTime);
    }

    [PunRPC]
    public void DestroyBullet()
    {
        Destroy(this.gameObject);
    }



}
