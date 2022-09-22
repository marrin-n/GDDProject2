using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerController : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("How fast the bee follows the queen. ")]
    private float m_Speed;

    [SerializeField]
    [Tooltip("Amount of health that the bee starts with")]
    private int m_MaxHealth;

    [SerializeField]
    [Tooltip("The spawning object this bee is linked to.")]
    private BeeSpawner m_Spawner;
    
    #endregion

    #region Cached Components
    private Rigidbody2D cc_Rb;
    #endregion

    #region Private Variables
    private Vector2 p_Velocity;
    private float p_CurHealth;
    private Vector2 origin; // The point that the bee follows (enemy during attack, queen otherwise)
    private GameObject followObject;
    private bool IsLive;
    #endregion

    #region Intialization
    private void Awake() {
        p_Velocity = Vector2.zero;
        cc_Rb = GetComponent<Rigidbody2D>();
        origin = new Vector2(0,0); //Orgin Set to 0 before queen is assigned by BeeSpawner
        IsLive = true;

        p_CurHealth = m_MaxHealth;
    }
    #endregion

    #region Main Updates
    private void FixedUpdate() {
        if (IsLive) {
            //MOVEMENT
            if (followObject != null) { // Uses queen as origin if it has been assigned, else uses origin
                origin = followObject.transform.position;
            }
            //Calculate direction based on mouse
            Vector2 dir = origin - new Vector2(transform.position.x, transform.position.y);
            dir.Normalize();
            //Set velocity (setting velocity instead of setting positions allows for dynamic collisions)
            cc_Rb.velocity = dir * m_Speed;
        }
    }

    private void Update() {
        if (p_CurHealth <= 0) {
            IsLive = false;
            //m_Spawner.DespawnBee(); 
             // having issues with this ?? not sure why, its to remove bee from spawner list but game seems to work without doing that
            Destroy(gameObject);
        }
    }
    #endregion

    #region Bee Control Function
    public void SetFollow(GameObject follow) { //used by Bee Spawner
        followObject = follow;
    }
    #endregion

    #region Health Methods
    private void DecreaseHealth(float amt) {
        p_CurHealth -= amt;
    }
    #endregion

    #region Collsion Methods
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.CompareTag("EnemyAttack")) {
            DecreaseHealth(1);
        }
    }
    #endregion
    
}
