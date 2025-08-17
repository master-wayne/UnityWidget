using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    public Transform m_target;

    void Update()
    {
        HandleObjectSelection();
    }

    void HandleObjectSelection()
    {
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.LeftAlt) && !Manipulator.m_isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                SelectObject(hit.transform.gameObject);
            }
            else
            {
                DeselectObject();
            }
        }
    }

    void SelectObject(GameObject obj)
    {
        m_target = obj.transform;
    }

    void DeselectObject()
    {
         m_target = null;
    }
}
