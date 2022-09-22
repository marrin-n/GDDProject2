using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Start is called before the first frame update
    #region Private Variables
    private float p_damage;
    public float damage {
        get {
            return p_damage;
        }
    }
    #endregion
    void Start()
    {
        p_damage = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Bee in attack zone");
        if (other.CompareTag("Bee")) {
            p_damage += 1;
        }
        else if (other.CompareTag("Queen")) {
            p_damage += 5;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Bee")) {
            p_damage -= 1;
        }
         else if (other.CompareTag("Queen")) {
            p_damage -= 5;
        }
    }

}
