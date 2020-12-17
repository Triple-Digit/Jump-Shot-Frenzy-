using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{    
    [Header("Movement Variables")]
    [SerializeField] float m_moveSpeed = 5f;
    [SerializeField] float m_jumpForce = 3f;

    float m_input;
    Rigidbody2D m_body;

    [Header("Jump Variables")]
    [SerializeField] Transform m_groundCheckPosition;
    [SerializeField] float m_groundCheckRadius = 1f;
    [SerializeField] LayerMask m_groundLayers;
    bool m_grounded;

    PhotonView m_photonView;
    

    private void Awake()
    {
        m_photonView = GetComponent<PhotonView>();
        m_body = GetComponent<Rigidbody2D>();
    }


    private void Start()
    {
        if(!m_photonView.IsMine)
        {
            Destroy(m_body);
        }
    }
    private void Update()
    {
        if (!m_photonView.IsMine) return;
        m_grounded = Physics2D.OverlapCircle(m_groundCheckPosition.position, m_groundCheckRadius, m_groundLayers);
        MovePlayer();
        Jump();
    }

    void MovePlayer()
    {
        m_input = Input.GetAxisRaw("Horizontal");
        m_body.velocity = new Vector2(m_input * m_moveSpeed, m_body.velocity.y);
    }

    void Jump()
    {
        if(m_grounded && Input.GetKeyDown(KeyCode.Space))
        {
            m_body.velocity = Vector2.up * m_jumpForce;
        }
    }
}
