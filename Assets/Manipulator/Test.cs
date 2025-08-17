using UnityEngine;

// Enum to let user choose which local axis faces the camera
public enum FacingAxis { X, Y, Z }

public class Test : MonoBehaviour
{
    public FacingAxis facingAxis = FacingAxis.Z; // Default to Z-axis (forward)
    public FacingAxis upAxis = FacingAxis.Y; // Default up axis for rotation

    private Camera mainCamera;

    void Start()
    {
        // Get reference to the main camera
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera == null)
        {
            return;
        }

        // Get the direction from the semicircle to the camera
        Vector3 directionToCamera = mainCamera.transform.position - transform.position;

        // Get the up axis vector based on user selection
        Vector3 upVector;
        if (upAxis == FacingAxis.X)
        {
            upVector = transform.right;
        }
        else if (upAxis == FacingAxis.Y)
        {
            upVector = transform.up;
        }
        else
        {
            upVector = transform.forward;
        }

        // Project the direction onto the plane perpendicular to the up axis
        Vector3 projectedDirection = Vector3.ProjectOnPlane(mainCamera.transform.forward, upVector).normalized;

        // Rotate the semicircle so the selected axis faces the camera
        if (projectedDirection != Vector3.zero)
        {
            Quaternion targetRotation;
            if (facingAxis == FacingAxis.X)
            {
                targetRotation = Quaternion.LookRotation(projectedDirection, upVector) * Quaternion.Euler(0, 90, 0);
            }
            else if (facingAxis == FacingAxis.Y)
            {
                targetRotation = Quaternion.LookRotation(projectedDirection, upVector) * Quaternion.Euler(-90, 0, 0);
            }
            else
            {
                targetRotation = Quaternion.LookRotation(projectedDirection, upVector);
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3);
            //transform.rotation = targetRotation;
        }
    }
}