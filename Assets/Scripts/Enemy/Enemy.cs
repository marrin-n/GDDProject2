using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("How much health this enemy has")]
    private int m_MaxHealth; 

    [SerializeField]
    [Tooltip("How fast enemy can move")]
    protected int m_Speed; 
    #endregion 

    #region Private Variables
    private float p_curHealth; 
    #endregion 

    #region Cached Components 
    protected Rigidbody2D cc_Rb; 
    #endregion 

    #region Cached References
    #endregion 

    #region Initialization 
    private void Awake()
    {
        p_curHealth = m_MaxHealth; 

        cc_Rb = GetComponent<Rigidbody2D>();

        InitializeMoveStateVariables(); 
    }

    private void Start()
    {
        
    }
    #endregion 


    #region Main Updates
    private void FixedUpdate() 
    {   
        if (p_curHealth < 0) {
            Debug.Log("ENEMY DIE");
            Destroy(gameObject);
        }
        //ChangeMoveState
        ChangeMoveState(); 

        //Move 
        Move(); 
        /*
        Vector3 dir = cr_Player.position - transform.position; 
        dir.Normalize(); 
        cc_Rb.MovePosition(cc_Rb.position + dir * m_Speed * Time.fixedDeltaTime); 
        */
    }
    #endregion


    #region Move / Attack Editor Variables
    [SerializeField]
    [Tooltip("Script that can check if bees are in sight range")]
    protected DetectLayerInCollider SightRangeScript; 

    [SerializeField]
    [Tooltip("Script that can check if bees are in attack range")]
    protected DetectLayerInCollider AttackRangeScript; 
    
    #endregion 

    #region Move / Attack Private Variables
    private string p_MoveState; 
    private float p_EvadeDuration; 
    private float p_EvadeTimer; 

    private float p_AttackDuration; 
    private float p_AttackTimer; 
    #endregion 

    #region Move / Attack Common Methods

    protected void InitializeMoveStateVariables() {
        p_MoveState = "rest"; 
        p_EvadeDuration = 3f; 
        p_EvadeTimer = 0f; 

        p_AttackDuration = 1.5f; 
        p_AttackTimer = 0f; 
    }

    //Enemy detected bee in sight range
    protected bool InSightRange() {
        return SightRangeScript.TargetInRange(); 
    }

    //Enemy detected bee in attack range
    protected bool InAttackRange() {
        return AttackRangeScript.TargetInRange(); 
    }

    //Check if evade duration is over
    protected bool EvadeOver() {
        if (p_EvadeTimer == 0) {
            return true; 
        } else if (p_EvadeTimer < 0) {
            p_EvadeTimer = 0; 
            return true; 
        } else {
            p_EvadeTimer -= Time.fixedDeltaTime; 
            return false; 
        }
    }

    protected bool AttackOver() {
        if (p_AttackTimer == 0) {
            return true; 
        } else if (p_AttackTimer < 0) {
            p_AttackTimer = 0; 
            return true; 
        } else {
            p_AttackTimer -= Time.fixedDeltaTime; 
            return false; 
        }
    }

    //Changes the move state: resting -> chase -> attack -> evade 
    protected void ChangeMoveState() {
        if (AttackOver() && EvadeOver()) {
            if (p_MoveState.Equals("rest") && InSightRange()) {
                p_MoveState = "chase"; 
            } else if (p_MoveState.Equals("chase") && InAttackRange()) {
                p_MoveState = "attack"; 
                p_AttackTimer = p_AttackDuration; 
            } else if (p_MoveState.Equals("attack")) {  
                p_MoveState = "evade"; 
                p_EvadeTimer = p_EvadeDuration; 
                allowAttack = true; 
            } else if (p_MoveState.Equals("evade")) {
                p_MoveState = "chase"; 
            } else if (!InSightRange()) {
                p_MoveState = "rest"; 
            } 
        }
    }
    
    //Processes the move state - resting, chase, attacks, evade 
    protected void Move() {
        if (p_MoveState.Equals("rest")) {
            Rest(); 
        } else if (p_MoveState.Equals("chase")) {
            Chase(); 
        } else if (p_MoveState.Equals("attack")) {
            Attack(); 
        } else if (p_MoveState.Equals("evade")) {
            Evade(); 
        } else {
            Debug.Log("Move state with name " + p_MoveState + " cannot be found"); 
        }
    }
    #endregion 

    #region Move State Virtual Methods
    //Method to Override in Child: Enemy Rest Behavior
    protected virtual void Rest() {

    } 

    //Method to Override in Child: Enemy Chase Behavior
    protected virtual void Chase() {

    } 

    //Enemy Attack Behavior
    [SerializeField]
    [Tooltip("Attack's offset position relative to enemy")]
    private float attackOffset = 1f; 

    [SerializeField]
    [Tooltip("Gameobject that controls the attack hitbox behavior")]
    private GameObject attackHitbox; 

    [SerializeField]
    [Tooltip("Delay until enemy hitbox is activated")]
    private float hitboxDelay = 0.4f; 

    protected void CreateAttack(Vector2 dir) {
        Vector3 attackPos = new Vector2(transform.position.x, transform.position.y) + dir * attackOffset; 
        StartCoroutine(DelayedInstantiate(hitboxDelay, attackHitbox, attackPos, dir)); 
    } 

    protected IEnumerator DelayedInstantiate(float waitTime, GameObject GO, Vector3 pos, Vector2 dir) {
        yield return new WaitForSeconds(waitTime); 
        GameObject attackObject = Instantiate(GO, pos, Quaternion.identity); 
        //Inproper generalization, quick fix for spider attack
        ProjectileAttack projectileAttackScript = attackObject.GetComponent<ProjectileAttack>(); 
        if (projectileAttackScript != null) {
            projectileAttackScript.setProjectileDirection(dir); 
        }
    }

    protected bool allowAttack = true; 
    //Creates a large attack, targetting on a bee in attack range, with a clear indicator
    protected virtual void Attack() { 
        if (InAttackRange() && allowAttack) {
            allowAttack = false; 
            cc_Rb.velocity = new Vector2(0, 0); 
            Vector2 BeePos = AttackRangeScript.GetRandomTarget().transform.position; 
            Vector2 dir = BeePos - new Vector2(transform.position.x, transform.position.y); 
            dir.Normalize(); 

            CreateAttack(dir); 

            BeeFound = false; 
        } 
    }

    //True if a bee has been found to compute evade on
    protected bool BeeFound = false; 
    //Method to Override in Child: Enemy Evade Behavior
    protected virtual void Evade() {

    } 
    #endregion 

    #region Health Methods
    private void DecreaseHealth(float amt) {
        p_curHealth -= amt;
    }
    #endregion

    #region Collsion Methods
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.CompareTag("PlayerAttack")) {
            DecreaseHealth(col.gameObject.GetComponent<PlayerAttack>().damage);
            Debug.Log("enemy health " + p_curHealth.ToString());
            //DecreaseHealth(5);
        }
    }
    #endregion

}
