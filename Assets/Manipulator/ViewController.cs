using UnityEngine;

public class ViewController : MonoBehaviour
{
    // Camera control settings
    public float zoomSpeed = 5f;
    public float panSpeed = 0.5f;
    public float rotateSpeed = 100f;

    public SelectionController m_selectionController;
    private Vector3 panOrigin;

    // Camera reference
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleCameraControls();
    }

    void HandleCameraControls()
    {
        Vector3 origin = Vector3.zero;
        if (m_selectionController.m_target != null)
        {
            origin = m_selectionController.m_target.position;
        }

        // Zoom with mouse scroll
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Vector3 zoomVector = (origin - Camera.main.transform.position).normalized;
            mainCamera.transform.Translate(Vector3.forward * scroll * zoomSpeed, Space.Self);
        }

        // Pan with Space + Drag
        if (Input.GetKey(KeyCode.Space))
        {
            if (Input.GetMouseButtonDown(0))
            {
                panOrigin = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                Vector3 delta = Input.mousePosition - panOrigin;
                mainCamera.transform.Translate(-delta.x * panSpeed * Time.deltaTime, -delta.y * panSpeed * Time.deltaTime, 0, Space.World);
                panOrigin = Input.mousePosition;
            }
        }

        // Rotate with Alt + Drag
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

            mainCamera.transform.RotateAround(origin, Vector3.up, mouseX);
            mainCamera.transform.RotateAround(origin, mainCamera.transform.right, -mouseY);

            //mainCamera.transform.RotateAround(mainCamera.transform.position, Vector3.up, mouseX);
            //mainCamera.transform.RotateAround(mainCamera.transform.position, mainCamera.transform.right, -mouseY);
        }
    }
}