using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(LineRenderer))]
public class Test1 : MonoBehaviour
{
    //private LineRenderer m_lineRenderer;

    [SerializeField]
    public LayerMask m_layerMask;

    private bool m_isDragging = false;

    private Vector3 m_mouseDragStartPos;
    private Quaternion m_initRotation;

    void Start()
    {
       /*  m_lineRenderer = GetComponent<LineRenderer>();
        m_lineRenderer.startWidth = 0.025f;
        m_lineRenderer.endWidth = 0.025f;
        m_lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        m_lineRenderer.startColor = Color.red;
        m_lineRenderer.endColor = Color.red;
        m_lineRenderer.positionCount = 3; */
    }

    void Update()
    {

    }

    /*     void OnMouseDown()
        {
            if (!m_isDragging)
            {
                m_lineRenderer.positionCount = 3;
                //Debug.Log("Begin drag.");

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_layerMask))
                {
                    m_beginDragPos = hit.point;
                    Debug.Log("beginDragPoint " + m_beginDragPos);
                }
                m_isDragging = true;
            }
        } */
    void OnMouseDown()
    {
        if (!m_isDragging)
        {
            //m_lineRenderer.positionCount = 3;
            //Debug.Log("Begin drag.");

            m_mouseDragStartPos = Input.mousePosition;

            //Vector3 beginDragPos   = Input.mousePosition;
            //beginDragPos.z         = Camera.main.WorldToScreenPoint(transform.position).z;
            //beginDragPos           = Camera.main.ScreenToWorldPoint(beginDragPos);
            //m_beginDragVector      = beginDragPos - transform.position;

            m_initRotation = transform.rotation;

            m_isDragging = true;
        }
    }
    void OnMouseDrag()
    {
        //Debug.Log("Dragging");

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
        //Quaternion newRotation = m_initRotation * Quaternion.AngleAxis(angle, transform.InverseTransformDirection(transform.up));
        Quaternion newRotation = m_initRotation * Quaternion.Euler(0, angle, 0);
        //Quaternion newRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Lerp(m_initRotation, newRotation, rotFactor);

        //Debug.Log(dot);

        /*         Vector3 mouseVector = (currentDragPos - m_beginDragPos).normalized;
                                //float rotationFactor = mouseVector.magnitude;
                                Vector3.di

                                if (mouseVector.magnitude > 0)
                                {
                                    float dot = Mathf.Clamp01(Vector3.Dot(mouseVector, transform.up));
                                    float factor = 1 - dot;
                                    //float dot_normalized = (dot + 1) / 2;
                                    //float factor = Mathf.Lerp(1, -1, dot_normalized);

                                    Quaternion newRotation = m_initRotation * Quaternion.AngleAxis(factor * mouseVector.si, transform.up);

                                    transform.rotation = Quaternion.Lerp(m_initRotation, m_initRotation * newRotation, mouseVector.magnitude);

                                    //transform.Rotate(transform.up, 2 * factor);
                                } */
        //m_beginDragPos = currentDragPos;

        //m_lineRenderer.SetPositions(new Vector3[] {transform.position, m_beginDragPos, currentDragPoint});
    }
    /*     void OnMouseDrag()
            {
                //Debug.Log("Dragging");

                Vector3 currentMousePos = Input.mousePosition;
                currentMousePos.z = Camera.main.WorldToScreenPoint(m_beginDragPos).z;

                Vector3 v = Camera.main.ScreenToWorldPoint(currentMousePos) - transform.position;

                Vector3 planeNormal = transform.up;
                Vector3 vProjected = Vector3.ProjectOnPlane(v, planeNormal);

                Ray ray = new Ray(transform.position, vProjected.normalized);
                RaycastHit hit;
                Vector3 currentDragPoint = m_beginDragPos;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_layerMask))
                {
                    currentDragPoint = hit.point;
                    Debug.Log("currentDragPoint " + currentDragPoint);
                }
                m_lineRenderer.SetPositions(new Vector3[] {transform.position, m_beginDragPos, currentDragPoint});
            } */

    /*     void OnMouseDrag()
            {
                Debug.Log("Dragging");

                Vector3 currentMousePos = Input.mousePosition;
                currentMousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;

                Vector3 v = Camera.main.ScreenToWorldPoint(currentMousePos) - transform.position;

                Vector3 planeNormal = transform.up;
                Vector3 vProjected = Vector3.ProjectOnPlane(v, planeNormal);

                Ray ray = new Ray(transform.position, vProjected.normalized);
                RaycastHit hit;
                Vector3 currentDragPoint = m_beginDragPos;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_layerMask))
                {
                    currentDragPoint = hit.point;
                    Debug.Log("currentDragPoint " + currentDragPoint);
                }
                m_lineRenderer.SetPositions(new Vector3[] {transform.position, m_beginDragPos, currentDragPoint});
            } */

    void OnMouseUp()
    {
        if (m_isDragging)
        {
            //m_lineRenderer.positionCount = 0;
            m_isDragging = false;
        }
    }
}
