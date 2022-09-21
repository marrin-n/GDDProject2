using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSpawner : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("The player that the bee follows.")]
    private GameObject m_Player;

    [SerializeField]
    [Tooltip("The bee prefab to spawn.")]
    private GameObject m_Bee;

    [SerializeField]
    [Tooltip("Spawn point offset from queen, X direction.")]
    private float m_SpawnXOffset;

    [SerializeField]
    [Tooltip("Spawn point offset from queen, Y dirrection.")]
    private float m_SpawnYOffset;

    #endregion

    #region Private Variables
    private int NumSpawnedBees;
    private List<WorkerController> Bees = new List<WorkerController>(); // All bee controllers are stored in a script in order to control origin
    #endregion

    #region Initialization
    private void Awake() {
        NumSpawnedBees = 0;
    }
    #endregion

    #region Spawn Methods
    public void SpawnBee(Vector3 QueenPosition) {
        Bees.Add(Instantiate(m_Bee, QueenPosition + new Vector3(m_SpawnXOffset, m_SpawnYOffset, 0), Quaternion.identity).GetComponent<WorkerController>());
        Bees[Bees.Count-1].SetFollow(m_Player);    
        NumSpawnedBees++;
        Debug.Log("Bee spanwed. Total bees spawned: " + NumSpawnedBees);
    }

    public void SetAllFollows(GameObject follow){ // used to switch the follow point between queen and attack collider
        foreach (WorkerController bee in Bees)
        {
            bee.SetFollow(follow);
        }
    }

    public void DespawnBee() {
        NumSpawnedBees--;
    }
    #endregion
}
