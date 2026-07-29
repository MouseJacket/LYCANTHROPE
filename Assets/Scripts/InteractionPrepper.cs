using UnityEngine;
using UnityEngine.InputSystem;


public class InteractionPrepper : MonoBehaviour
{
    private BasicMovement movement;
    public GameObject highlightedObject;
    [SerializeField] private float selectDist;
    [SerializeField] private Material selectMat;
    [SerializeField] private Material nativeMat;
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector3 lookDir;
    
    void Start()
    {
        movement = GetComponent<BasicMovement>();
        _camera = movement.playerCam;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        RaycastHit hit;
        lookDir = _camera.gameObject.transform.forward;
        Debug.DrawRay(_camera.transform.position, lookDir*selectDist);
        Physics.Raycast(_camera.transform.position, lookDir, out hit, selectDist);
        if (Physics.Raycast(_camera.transform.position, lookDir, out hit, selectDist) && 
            hit.collider.CompareTag("Selectable"))
        {
            Debug.Log($"Hit - {hit.collider.gameObject}");
            if (hit.collider.gameObject != highlightedObject)
            {
                SelectObject(hit.collider.gameObject);
            }

            if (InputSystem.actions["Interact"].ReadValue<float>() > 0)
            {
                Debug.Log($"Interact - {highlightedObject.name}");
                GameObject.Destroy(highlightedObject);
                highlightedObject = null;
            }
        }
        else
        {
            DeselectObject();
        }
    }

    void SelectObject(GameObject newHighlight)
    {
        if (highlightedObject != null)
        {
            DeselectObject();
        }

        highlightedObject = newHighlight;
        nativeMat = highlightedObject.GetComponent<MeshRenderer>().material;
        highlightedObject.GetComponent<MeshRenderer>().material = selectMat;
    }

    void DeselectObject()
    {
        if (highlightedObject != null)
        {
            highlightedObject.GetComponent<MeshRenderer>().material = nativeMat;
            highlightedObject = null;
        }
    }
}
