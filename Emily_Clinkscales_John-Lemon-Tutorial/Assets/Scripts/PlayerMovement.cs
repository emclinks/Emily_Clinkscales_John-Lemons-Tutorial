using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    public Transform playerBody;
    public float mouseSensitivity = 300f;
    float xRotation = 0f;

    public float turnSpeed = 20f;

    //Feature Additions
    public int health;
    int count;
    public TextMeshProUGUI healthText;
    public GameObject ectoText;
    public bool m_PlayerHasKey;
    public bool m_IsPlayerEctod;
    GameEnding gameEnding;

    // Start is called before the first frame update
    void Start()
    {
        //Feature starts
        health = 10;
        count = 7;
        SetHealthText();
        ectoText.SetActive(false);

        //Basekit starts
        m_Animator = GetComponent<Animator> ();
        m_Rigidbody = GetComponent<Rigidbody> ();
        m_AudioSource = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize ();

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool ("IsWalking", isWalking);

        if(isWalking)
        {
            if(!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }
        //FPS Mouselook
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        this.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }

    void OnAnimatorMove ()
    {
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation (m_Rotation);
    }
    //Taking damage > instant failure feature.
    public void TakeDamage ()
    {
        health = health - 1;
        SetHealthText();
    }

    void OnTriggerEnter (Collider other)
    {
        //Ectoplasmic coating trigger
        if(other.gameObject.CompareTag("Ectoplasm"))
        {
            m_IsPlayerEctod = true;
            Ectomode();
        }
        //Key to open door setup
        if(other.gameObject.CompareTag("Key"))
        {
            m_PlayerHasKey = true;
            other.gameObject.SetActive(false);
        }
    }
    
    void OnCollision (Collider other)
    {
        if(other.gameObject.CompareTag("Ghost"))
        {
            if(m_IsPlayerEctod == true)
            {
                Observer.GhostDeath();
                other.gameObject.SetActive(false);
                count = count - 1;
            }
            else
            {
                TakeDamage();
            }
        }
    }
    
    void SetHealthText()
    {
        healthText.text = "Health: " + health.ToString();
        //Setting loss conditional
        if (health <= 0)
        {
            gameEnding.PlayerDied();
        }
    }

    void CountCheck()
    {
        //Setting second win conditional
        if (count <= 0)
        {
            gameEnding.m_IsPlayerAtExit = true;
        }
    }

    public void Ectomode()
    {
        health = health + 100000;
        healthText.text = "Health: ERROR";
    }
}
