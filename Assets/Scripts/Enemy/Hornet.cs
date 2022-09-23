using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hornet : Enemy
{

    //Flies in random circular motion
    protected override void Rest() {
        cc_Rb.velocity = new Vector2(0, 0); 
    } 

    //After detecting a bee in sight range, charge at the bee 
    protected override void Chase() {
        if (InSightRange()) {
            Vector2 BeePos = SightRangeScript.GetRandomTarget().transform.position; 
            Vector2 dir = BeePos - new Vector2(transform.position.x, transform.position.y); 
            dir.Normalize(); 

            cc_Rb.velocity = dir * m_Speed; 
        } else {
            //Debug.Log("No bee is in chase range"); 
        }
    } 


    //Circles the player 
    private GameObject TargetBee; 
    private float circleRange = 2.5f;

    protected override void Evade() {
        if (InSightRange()) {
            if (!BeeFound) {
                BeeFound = true; 
                TargetBee = SightRangeScript.GetRandomTarget(); 
            }
            if (TargetBee != null) {
                Vector2 BeePos = TargetBee.transform.position; 
                Vector2 dir = BeePos - new Vector2(transform.position.x, transform.position.y); 
                float distToBee = dir.magnitude; 
                dir.Normalize(); 

                if (Mathf.Abs(distToBee - circleRange) < 0.5) { //Enemy can start circling the player
                    cc_Rb.velocity = new Vector2(-dir.y, dir.x) * m_Speed; 
                } else if (distToBee - circleRange < 0) {
                    dir *= -1; 
                    cc_Rb.velocity = dir * m_Speed; 
                } else if (distToBee - circleRange > 0) {
                    cc_Rb.velocity = dir * m_Speed; 
                } else {
                    //Debug.Log("Evade condition not caught properly"); 
                }
            }
        }
    } 
}
