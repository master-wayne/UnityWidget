using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    private Vector3 m_origin;
    private bool m_isDragging = false;

    void Start()
    {
        
    }
    void OnMouseDown()
    {
        if (!m_isDragging)
        {
            m_origin = Input.mousePosition;
            m_isDragging = true;
        }
    }

    void OnMouseDrag()
    {
        Vector3 currentMousePos = Input.mousePosition;
        Vector3 displacement = currentMousePos - m_origin;
        float dot = Vector3.Dot(Vector3.right, displacement.normalized);
        Debug.Log(dot);
        
    }

    void OnMouseUp()
    {
        m_isDragging = false;
    }

    void Update()
    {

        
    }
}
