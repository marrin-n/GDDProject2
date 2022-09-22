using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("How many bees this flower should spawn.")]
    private int m_NumBees;
    public int NumBees {
        get {
            return m_NumBees;
        }
    }
    #endregion 

}