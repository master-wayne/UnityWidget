using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum WidgetType
{
    MOVE_X,
    MOVE_Y,
    MOVE_Z,
    ROTATE_X,
    ROTATE_Y,
    ROTATE_Z
}
public enum WidgetClass
{
    MOVE_WIDGET,
    ROTATION_WIDGET
}
public class Widget : MonoBehaviour
{
    public WidgetClass m_widgetClass;

    [SerializeField]
    public WidgetType m_widgetType;

    [SerializeField]
    public Color m_defaultColor = Color.white;

    [SerializeField]
    public Color m_hoverColor = Color.white;

    [SerializeField]
    public Color m_selectColor = Color.yellow;

    public Vector3 m_upAxis;
    public Vector3 m_forwardAxis;

    private Material m_widgetMaterial;

    private void OnEnable()
    {
        Manipulator.SetWidgetVisibility += SetWidgetVisibility;
        
    }
    private void OnDisable()
    {
        Manipulator.SetWidgetVisibility -= SetWidgetVisibility;
    }

    private void SetWidgetVisibility(bool isVisible)
    {
        GetComponent<MeshRenderer>().enabled = isVisible;
    }

    void Start()
    {
        Init();
    }

    private void Init()
    {
        switch (m_widgetType)
        {
            case WidgetType.MOVE_X:
                m_forwardAxis = Vector3.right;
                m_upAxis = Vector3.zero;
                m_widgetClass = WidgetClass.MOVE_WIDGET;
                break;

            case WidgetType.MOVE_Y:
                m_forwardAxis = Vector3.up;
                m_upAxis = Vector3.zero;
                m_widgetClass = WidgetClass.MOVE_WIDGET;
                break;

            case WidgetType.MOVE_Z:
                m_forwardAxis = Vector3.forward;
                m_upAxis = Vector3.zero;
                m_widgetClass = WidgetClass.MOVE_WIDGET;
                break;

            case WidgetType.ROTATE_X:
            case WidgetType.ROTATE_Y:
            case WidgetType.ROTATE_Z:
                m_forwardAxis = Vector3.right;
                m_upAxis = transform.up;
                m_widgetClass = WidgetClass.ROTATION_WIDGET;
                break;
        }
        m_widgetMaterial = GetComponent<MeshRenderer>().material;
        SetColor(m_defaultColor);
    }
    void Update()
    {
        AlignRotationWidgetToCamera();
    }

    public void SetColor(Color color)
    {
        m_widgetMaterial.color = color;
    }

    private void AlignRotationWidgetToCamera()
    {
        if (m_widgetClass == WidgetClass.ROTATION_WIDGET)
        {
            Vector3 viewVectorProjected = Vector3.ProjectOnPlane(Camera.main.transform.forward, m_upAxis).normalized;
            if (viewVectorProjected != Vector3.zero)
            {
                Quaternion targetRotation;
                targetRotation = transform.parent.rotation * Quaternion.LookRotation(viewVectorProjected, m_upAxis) * Quaternion.Euler(0, 90, 0) ;
                //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 20);
                transform.rotation = targetRotation;
            }
        }
    }
}
