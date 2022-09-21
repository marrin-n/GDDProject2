using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("Speed of Camera.")]
    private float m_CameraSpeed;
    
    [SerializeField]
    [Tooltip("Edge size (how close the mouse needs to be to the edge to move the camera).")]
    private float m_EdgeSize;

    [SerializeField]
    [Tooltip("Max X position of screen (end of map)")]
    private float m_MaxX;

    [SerializeField]
    [Tooltip("Max X position of screen (end of map)")]
    private float m_MinX;
    #endregion

    #region Private Variables
    private float MouseX;
    private Vector3 NewPos;
    private float screenXpos;
    #endregion

    #region Intialization
    private void Awake() {
        transform.position = new Vector3(0, 0, -10);

    }
    #endregion

    #region Main Updates
    private void LateUpdate() {
        MouseX = Input.mousePosition.x;
        if (MouseX > Screen.width - m_EdgeSize) {
            NewPos = transform.position + new Vector3(m_CameraSpeed, 0, 0) * Time.deltaTime;
        } else if (MouseX < m_EdgeSize) {
            NewPos = transform.position - new Vector3(m_CameraSpeed, 0, 0) * Time.deltaTime;
        }

        if (NewPos.x > m_MinX & NewPos.x < m_MaxX) {
            transform.position = NewPos;
        }
    }
    #endregion
}
