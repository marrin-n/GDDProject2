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
    /*
    [SerializeField]
    [Tooltip("The player that the bee follows.")]
    private QueenController player;
    */
    #endregion

    #region Cached Components
    private Rigidbody2D cc_Rb;
    #endregion

    #region Private Variables
    private Vector2 p_Velocity;
    private float p_CurHealth;
    private Vector2 origin; // The point that the bee follows (enemy during attack, queen otherwise)
    private GameObject followObject;
    #endregion

    #region Intialization
    private void Awake() {
        p_Velocity = Vector2.zero;
        cc_Rb = GetComponent<Rigidbody2D>();
        origin = new Vector2(0,0); //Orgin Set to 0 before queen is assigned by BeeSpawner

        p_CurHealth = m_MaxHealth;
    }
    #endregion

    #region Main Updates
    private void FixedUpdate() {
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
    #endregion

    #region Bee Control Function
    public void SetFollow(GameObject follow) { //used by Bee Spawner
        followObject = follow;
    }
    #endregion

    
}
