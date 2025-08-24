using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    public delegate void ObjectSelected(Transform obj);
    public delegate void ObjectUnSelected();

    public event ObjectSelected OnObjectSelected;
    public event ObjectUnSelected OnObjectUnSelected;

    void Update()
    {
        HandleObjectSelection();
    }

    void HandleObjectSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                OnObjectSelected?.Invoke(hit.transform);
            }
            else
            {
                OnObjectUnSelected?.Invoke();
            }
        }
    }
}
