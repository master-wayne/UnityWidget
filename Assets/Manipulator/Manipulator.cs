using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Manipulator : MonoBehaviour
{
    [SerializeField]
    public LayerMask m_manipulatorLayer;

    [SerializeField]
    public SelectionController m_selectionController;

    private bool m_isVisible = false;
    private Transform m_activeHandle;

    private Color m_HandleColor;

    [HideInInspector]
    public static bool m_isDragging = false;

    private Widget m_activeWidget;

    [SerializeField]
    public float m_manipulatorScaleFactor = 0.2f;

    [SerializeField]
    public float m_moveSpeed = 0.2f;

    [SerializeField]
    public float m_rotateSpeed = 5f;

    private Vector3 m_mouseDragStartPos;
    private Vector3 m_targetStartPos;
    private Quaternion m_targetStartRot;

    private delegate void WidgetEnter(Widget activeWidget);
    private delegate void WidgetExit(Widget activeWidget);
    private delegate void WidgetStartDrag(Widget activeWidget, Vector3 mousePos);
    private delegate void WidgetDrag(Widget activeWidget, Vector3 mousePos);
    private delegate void WidgetEndDrag(Widget activeWidget, Vector3 mousePos);
    public delegate void WidgetVisibility(bool isVisible);

    private event WidgetEnter OnWidgetEnter;
    private event WidgetExit OnWidgetExit;
    private event WidgetStartDrag OnWidgetStartDrag;
    private event WidgetDrag OnWidgetDrag;
    private event WidgetDrag OnWidgetEndDrag;
    public static event WidgetVisibility SetWidgetVisibility;

    private void OnEnable()
    {
        OnWidgetEnter          += Enter;
        OnWidgetExit           += Exit;
        OnWidgetStartDrag      += StartDrag;
        OnWidgetDrag           += Drag;
        OnWidgetEndDrag        += EndDrag;
    }

    private void OnDisable()
    {
        OnWidgetEnter          -= Enter;
        OnWidgetExit           -= Exit;
        OnWidgetStartDrag      -= StartDrag;
        OnWidgetDrag           -= Drag;
        OnWidgetEndDrag        -= EndDrag; 
    }

    // Start is called before the first frame update
    void Start()
    {
        SetWidgetVisibility.Invoke(false);
    }

    private void Enter(Widget widget)
    {
        widget.SetColor(widget.m_hoverColor);
    }

    private void Exit(Widget widget)
    {
        widget.SetColor(widget.m_defaultColor);
    }

    private void StartDrag(Widget widget, Vector3 mousePos)
    {
        widget.SetColor(widget.m_selectColor);
        m_mouseDragStartPos = mousePos;
        m_targetStartPos = m_selectionController.m_target.position;
        m_targetStartRot = m_selectionController.m_target.rotation;
    }
    private void Drag(Widget widget, Vector3 mousePos)
    {
        if (widget.m_widgetClass == WidgetClass.MOVE_WIDGET)
        {
            ManipulatorMove(widget, mousePos);
        }
        if (widget.m_widgetClass == WidgetClass.ROTATION_WIDGET)
        {
            ManipulatorRotate2(widget, mousePos);
        }
    }
    private void EndDrag(Widget widget, Vector3 mousePos)
    {
        widget.SetColor(widget.m_defaultColor);
    }

    private void ManipulatorMove(Widget widget, Vector3 mousePos)
    {
        Vector3 mouseDelta = mousePos - m_mouseDragStartPos;
        Vector3 localAxis = transform.TransformDirection(widget.m_forwardAxis).normalized;
        Camera cam = Camera.main;
        Vector3 targetPos = m_selectionController.m_target.position;

        Vector3 p1 = cam.ScreenToWorldPoint(new Vector3(mouseDelta.x, mouseDelta.y, cam.WorldToScreenPoint(targetPos).z));
        Vector3 p2 = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.WorldToScreenPoint(targetPos).z));
        Vector3 worldMouseDelta = p1 - p2;
        float movement = Vector3.Dot(worldMouseDelta, localAxis) * m_moveSpeed;
         m_selectionController.m_target.position = m_targetStartPos + localAxis * movement;
    
    }
    private void ManipulatorRotate(Widget widget, Vector3 mousePos)
    {
        Vector3 mouseDragStartPos_WS, beginDragVector;

        m_mouseDragStartPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        mouseDragStartPos_WS = Camera.main.ScreenToWorldPoint(m_mouseDragStartPos);
        beginDragVector = mouseDragStartPos_WS - transform.position;

        Vector3 currentDragPos_WS = Input.mousePosition;
        currentDragPos_WS.z = Camera.main.WorldToScreenPoint(transform.position).z;
        currentDragPos_WS = Camera.main.ScreenToWorldPoint(currentDragPos_WS);
        Vector3 currentDragVector = currentDragPos_WS - transform.position;

        float rotFactor = (Input.mousePosition - m_mouseDragStartPos).magnitude;

        float angle = Vector3.SignedAngle(beginDragVector, currentDragVector, transform.up);

        quaternion rot = Quaternion.identity;
        switch (widget.m_widgetType)
        {
            case WidgetType.ROTATE_X:
                rot = Quaternion.Euler(angle, 0, 0);
                break;
            case WidgetType.ROTATE_Y:
                rot = Quaternion.Euler(0, angle, 0);
                break;
            case WidgetType.ROTATE_Z:
                rot = Quaternion.Euler(0, 0, -angle);
            break;
        }
        Quaternion newRotation = m_targetStartRot * rot;
        m_selectionController.m_target.rotation = Quaternion.Lerp(m_targetStartRot, newRotation, rotFactor);
    }

    private void ManipulatorRotate2(Widget widget, Vector3 mousePos)
    {
        Vector3 mouseDragStartPos_WS;

        m_mouseDragStartPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        mouseDragStartPos_WS = Camera.main.ScreenToWorldPoint(m_mouseDragStartPos);

        Vector3 currentDragPos_WS = Input.mousePosition;
        currentDragPos_WS.z = Camera.main.WorldToScreenPoint(transform.position).z;
        currentDragPos_WS = Camera.main.ScreenToWorldPoint(currentDragPos_WS);

        Vector3 mouseDisplacement_WS = currentDragPos_WS - mouseDragStartPos_WS;
        Vector3 rotationAxis_WS = Vector3.zero;

        float rotFactor = -(Input.mousePosition - m_mouseDragStartPos).magnitude;

        quaternion rot = Quaternion.identity;
        float dot;
        switch (widget.m_widgetType)
        {
            case WidgetType.ROTATE_X:
                rotationAxis_WS = transform.TransformDirection(Vector3.right);
                dot = Vector3.Dot(mouseDisplacement_WS.normalized, rotationAxis_WS.normalized);
                rot = Quaternion.Euler(rotFactor * m_rotateSpeed * (1 - dot), 0, 0);
                break;
            case WidgetType.ROTATE_Y:
                rotationAxis_WS = transform.TransformDirection(Vector3.up);
                dot = Vector3.Dot(mouseDisplacement_WS.normalized, rotationAxis_WS.normalized);
                rot = Quaternion.Euler(0, rotFactor * m_rotateSpeed * (1 - dot), 0);
                break;
            case WidgetType.ROTATE_Z:
                rotationAxis_WS = transform.TransformDirection(Vector3.forward);
                dot = Vector3.Dot(mouseDisplacement_WS.normalized, rotationAxis_WS.normalized);
                rot = Quaternion.Euler(0, 0, rotFactor * m_rotateSpeed * (1 - dot));
                break;
        }

        Quaternion newRotation = m_targetStartRot * rot;
        m_selectionController.m_target.rotation = newRotation;
    }

    void Update()
    {
        SetVisibility();

        if (m_isVisible)
        {
            UpdateManipulatorTransform();
            CheckForMouseInteraction();
        }
    }
    private void CheckForMouseInteraction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_manipulatorLayer))
        {
            Widget widget = hit.collider.GetComponent<Widget>();

                if (!m_isDragging)
                {
                    if (m_activeWidget == null)
                    {
                        m_activeWidget = widget;
                        OnWidgetEnter?.Invoke(m_activeWidget);
                    }
                    else if (Input.GetMouseButtonDown(0))
                    {
                        m_isDragging = true;
                        OnWidgetStartDrag?.Invoke(m_activeWidget, Input.mousePosition);
                    }
                }
                else
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        m_isDragging = false;
                        OnWidgetEndDrag?.Invoke(m_activeWidget, Input.mousePosition);
                    }
                    else
                    {
                        m_isDragging = true;
                        OnWidgetDrag?.Invoke(m_activeWidget, Input.mousePosition);
                    }
                }
        }
        else
        {
            if (!m_isDragging)
            {
                if (m_activeWidget != null)
                {
                    OnWidgetExit?.Invoke(m_activeWidget);
                    m_activeWidget = null;
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    m_isDragging = false;
                    OnWidgetEndDrag?.Invoke(m_activeWidget, Input.mousePosition);
                    m_activeWidget = null;
                }
                else
                {
                    m_isDragging = true;
                    OnWidgetDrag?.Invoke(m_activeWidget, Input.mousePosition);
                }
            }
        }
    }

    private void UpdateManipulatorTransform()
    {
        transform.position = m_selectionController.m_target.position;
        transform.rotation = m_selectionController.m_target.rotation;

        // scale manipulator with distance
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
        transform.localScale = Vector3.one * distance * m_manipulatorScaleFactor;
    }

    private void SetVisibility()
    {
        bool isVisible = false;
        if (m_selectionController.m_target != null)
        {
            isVisible = true;
        }
        else
        {
            isVisible = false;
        }
        if (m_isVisible != isVisible)
        {
            m_isVisible = isVisible;
            SetWidgetVisibility.Invoke(m_isVisible);
        }
    }
}
