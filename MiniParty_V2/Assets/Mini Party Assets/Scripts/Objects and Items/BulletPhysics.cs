using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhysics : MonoBehaviour
{

    [SerializeField] float m_bulletSpeed = 5f;
    [SerializeField] Rigidbody2D m_rigidbody2D;
    

    void Start()
    {
        m_rigidbody2D.velocity = transform.right * m_bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DestroyBullet();
    }

    [PunRPC]
    public void DestroyBullet()
    {
        Destroy(this.gameObject);
    }



}
