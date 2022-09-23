using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy
{
    private float restSpeed = 1.5f; 

    private float walkTimer = 0f; 
    
    //direction that the spider is facing
    private int dir = 1; 

    //determiens if spider should walk or stop for few random seconds
    private bool walking = true;
    
    private void walkBackAndForth(float speed, bool stopEnabled) {
        if (walkTimer == 0) {
            float duration = Random.Range(1f, 3f); 
            walkTimer += duration; 
            if ((!stopEnabled) || walking) {
                dir *= -1; 
            } 
        } else if (walkTimer < 0) {
            walkTimer = 0; 
            walking = !walking; 
        } else {
            cc_Rb.velocity = new Vector2(speed * dir, 0); 
            walkTimer -= Time.fixedDeltaTime; 
        }
    }

    private void walkStop(float speed, bool stopEnabled) {
        if (!stopEnabled) {
            walkBackAndForth(speed, stopEnabled); 
        } else if (stopEnabled) {
            if (walking) {
                walkBackAndForth(speed, stopEnabled); 
            } else {
                walkBackAndForth(0, stopEnabled); 
            }
        }
    }

    //Move slowly back and forth 
    protected override void Rest() {
        walkStop(restSpeed, true); 
    } 

    private float activeSpeed = 3f; 
    //After detecting the player, moves closer until within a certain range 
    protected override void Chase() {
        if (InSightRange()) {
            float duration = Random.Range(1f, 3f); 
            Vector2 BeePos = SightRangeScript.GetRandomTarget().transform.position; 
            Vector2 dir = new Vector2(BeePos.x - transform.position.x, 0); 
            dir.Normalize(); 

            cc_Rb.velocity = dir * activeSpeed; 
        } else {
            //Debug.Log("No bee is in chase range"); 
        }
    } 

    //Moves quickly back and forth
    protected override void Evade() {
        walkStop(activeSpeed, false); 
    } 

    protected override void Attack() {
        if (InAttackRange() && allowAttack) {
            allowAttack = false; 
            cc_Animator.SetTrigger("Attack"); 
            cc_Rb.velocity = new Vector2(0, 0); 
            Vector2 BeePos = AttackRangeScript.GetRandomTarget().transform.position; 
            Vector2 dir = BeePos - new Vector2(transform.position.x, transform.position.y); 
            dir.Normalize(); 

            CreateAttack(dir); 

            BeeFound = false; 
        } 
    }
}
