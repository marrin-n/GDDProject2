using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine;

public class DetectLayerInCollider : MonoBehaviour
{
    private string targetTag; 
    private List<GameObject> targetList;

    #region Initialization 
    void Awake()
    {
        targetList = new List<GameObject>(); 
        targetTag = "Bee"; 
    }
    #endregion

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == targetTag) {
			targetList.Add(coll.gameObject); 
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag == targetTag) {
			targetList.Remove(coll.gameObject); 
		}
    }

    public bool TargetInRange() {
        if (targetList.Any()) {
            return true; 
        } else {
            return false; 
        }
    }

    public GameObject GetRandomTarget() {
        if (TargetInRange()) {
            return targetList[Random.Range(0, targetList.Count)];
        } 
        return null; 
    }
}
