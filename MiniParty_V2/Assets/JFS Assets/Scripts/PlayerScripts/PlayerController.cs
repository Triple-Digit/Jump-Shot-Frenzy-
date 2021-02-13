using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [Header("Player Holder")]
    [SerializeField] GameObject m_playerBody;

    [Header("Movement Variables")]
    [SerializeField] float m_moveSpeed = 5f;
    [SerializeField] float m_jumpForce = 3f;
    bool m_facingRight = true;
    float m_direction = 1f;

    float m_input;
    Rigidbody2D m_body;

    [Header("Jump Variables")]
    [SerializeField] Transform m_groundCheckPosition;
    [SerializeField] float m_groundCheckRadius = 1f;
    [SerializeField] LayerMask m_groundLayers;
    bool m_grounded;

    [Header("Front Flip Variables")]
    [SerializeField] bool m_flipping = false;
    [SerializeField] float m_rotationAngle = 0f;
    [SerializeField] float m_rotationSpeed = 1f;
    [SerializeField] float m_resetOrientationSpeed = 4f;

    [Header("Combat")]
    [SerializeField] Transform m_shootingPoint;
    [SerializeField] GameObject m_bulletPrefab;
    [SerializeField] float m_fireRate = 0f;
    [SerializeField] int m_roundsPerClip = 1;
    int m_roundsRemaining;
    float m_currentHealth;
    [SerializeField] float m_maxHealth = 1f;


    
    PhotonView m_photonView;
    bool m_OfflineMode;
    PlayerManager m_playerManager;
    

    private void Awake()
    {
        m_photonView = GetComponent<PhotonView>();
        m_body = GetComponent<Rigidbody2D>();
        m_playerManager = PhotonView.Find((int)m_photonView.InstantiationData[0]).GetComponent<PlayerManager>();
        m_currentHealth = m_maxHealth;
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
        FrontFlip();
        Shoot();
    }

    void MovePlayer()
    {
        m_input = Input.GetAxisRaw("Horizontal");
        m_body.velocity = new Vector2(m_input * m_moveSpeed, m_body.velocity.y);
        if (m_input > 0 && !m_facingRight) ChangeFacingDirection();
        else if (m_input < 0 && m_facingRight) ChangeFacingDirection();
    }

    void Jump()
    {
        if(m_grounded && Input.GetKeyDown(KeyCode.Space))
        {
            m_body.velocity = Vector2.up * m_jumpForce;
        }
    }

    void FrontFlip()
    {
        m_flipping = !m_grounded;
        m_playerBody.gameObject.transform.rotation = Quaternion.Euler(0, 0, m_rotationAngle);
        
        if (m_flipping)
        {
            m_rotationAngle -= Time.deltaTime * 360 * m_rotationSpeed;
        }

        else
        {
            if (m_rotationAngle < -1)
            {
                if (m_rotationAngle > -100)
                {
                    m_rotationAngle += Time.deltaTime * 360 * m_resetOrientationSpeed;
                }
                if (m_rotationAngle < -90)
                {
                    m_rotationAngle -= Time.deltaTime * 360 * m_resetOrientationSpeed;
                }
            }
            else
            {
                m_rotationAngle = 0;
            }
        }

        if (m_rotationAngle <= -360f)
        {
            m_rotationAngle = 0;
        }
    }

    void ChangeFacingDirection()
    {
        if(m_grounded)
        {
            m_facingRight = !m_facingRight;
            m_direction *= -1f;
        }
    }

    void Shoot()
    {
        if(!m_grounded && Input.GetKeyDown(KeyCode.Mouse0))
        {            
            GameObject bullet = PhotonNetwork.Instantiate(m_bulletPrefab.name, m_shootingPoint.position, m_playerBody.gameObject.transform.rotation);
        }        
    }

    private void OnDrawGizmosSelected()
    {
        if (m_groundCheckPosition == null) return;
        Gizmos.DrawWireSphere(m_groundCheckPosition.position, m_groundCheckRadius);
    }

    public void Die()
    {
        m_photonView.RPC("RPC_Die", RpcTarget.All);
    }

    [PunRPC]
    void RPC_Die()
    {
        if (!m_photonView) return;

    }

    [PunRPC]
    public void RPC_TakeDamage(float amount)
    {
        if (!m_photonView) return;
        TakeDamage(amount);

    }

    void TakeDamage(float amount)
    {
        if(m_photonView.IsMine)
        {
            m_currentHealth -= amount;
        }
    }

    void CheckHealth()
    {
        if(photonView.IsMine && m_currentHealth <=0)
        {
            m_photonView.RPC("Die", RpcTarget.AllBuffered);
        }
    }

}
