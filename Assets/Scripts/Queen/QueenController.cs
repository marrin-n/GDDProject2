using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenController : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("How fast the queen follows the mouse. ")]
    private float m_Speed;

    [SerializeField]
    [Tooltip("Amount of health that the queen starts with")]
    private int m_MaxHealth;

    [SerializeField]
    [Tooltip("The spawning object this queen is linked to.")]
    private BeeSpawner m_Spawner;

    [SerializeField]
    [Tooltip("Prefab for attack colliders")]
    private GameObject m_AttackCollider;

    [SerializeField]
    [Tooltip("Distance awawy from queen that attack spawns.")]
    private float m_AttackRange;

    [SerializeField]
    [Tooltip("Duration of Attack Cooldown")]
    private float m_CooldownDuration;
    #endregion

    #region Cached Components
    private Rigidbody2D cc_Rb;
    #endregion

    #region Private Variables
    private Vector2 p_Velocity;
    private float p_CurHealth;
    private Vector2 mousePosition;
    private float AttackCoolDown;
    private GameObject AttackInstance;
    private bool AttackInProgress;
    Vector2 dir;
    #endregion

    #region Intialization
    private void Awake() {
        p_Velocity = Vector2.zero;
        cc_Rb = GetComponent<Rigidbody2D>();
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        AttackInProgress = false;
        AttackCoolDown = 0;

        p_CurHealth = m_MaxHealth;
    }
    #endregion

    #region Main Updates
    private void Update() {
        //MOVEMENT

        //Update Mouse position
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Calculate direction based on mouse
        dir = mousePosition - new Vector2(transform.position.x, transform.position.y);
        dir.Normalize();
        cc_Rb.velocity = dir * m_Speed;

        //Check for attack trigger
        if (Input.GetKeyDown(KeyCode.Space)) {
            Attack();
        }

        //Check for attack CoolDown
        if (AttackCoolDown < 0) {
            EndAttack();
        } else if (AttackCoolDown > 0 & AttackInProgress) {
            AttackCoolDown -= Time.deltaTime;
        }
        
    }
    #endregion

    #region Collision Methods
    private void OnTriggerEnter2D(Collider2D other) {
        // Spawns new bee when colliding with flower
        if (other.CompareTag("Flower")) {
            m_Spawner.SpawnBee(transform.position);
            Destroy(other.gameObject);
        }
    }
    #endregion

    #region Attack Methods
    private void Attack() {
        Debug.Log("Attack Started");
        if (!AttackInProgress) {
            AttackInProgress = true;
            AttackCoolDown = m_CooldownDuration;
            //Calculates position based on the range given and teh direction the queen is headed 
            AttackInstance = Instantiate(m_AttackCollider, new Vector2(transform.position.x, transform.position.y) + dir * m_AttackRange, Quaternion.identity);
            m_Spawner.SetAllFollows(AttackInstance);
        } else {
            Debug.Log("Cooldown not over, attack already in progress");
        }
        
    }
    private void EndAttack() {
        AttackInProgress = false;
        AttackCoolDown = 0;
        m_Spawner.SetAllFollows(gameObject);
        Destroy(AttackInstance);
    }
    #endregion
}
