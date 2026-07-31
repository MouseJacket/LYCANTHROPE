using UnityEngine;
using UnityEngine.InputSystem;


public class InteractionPrepper : MonoBehaviour
{
    private BasicMovement movement;
    public GameObject highlightedObject;
    [SerializeField] private float selectDist;
    [SerializeField] private Material selectMat;
    [SerializeField] private Material nativeMat;
    [SerializeField] private Camera cam;
    [SerializeField] private Vector3 lookDir;
    
    void Start()
    {
        movement = GetComponent<BasicMovement>();
        cam = movement.playerCam;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        RaycastHit hit;
        lookDir = cam.gameObject.transform.forward;
        Debug.DrawRay(cam.transform.position, lookDir*selectDist);
        Physics.Raycast(cam.transform.position, lookDir, out hit, selectDist);
        if (Physics.Raycast(cam.transform.position, lookDir, out hit, selectDist) && 
            hit.collider.CompareTag("Selectable"))
        {
            if (hit.collider.gameObject != highlightedObject)
            {
                SelectObject(hit.collider.gameObject);
            }

            if (InputSystem.actions["Interact"].ReadValue<float>() > 0)
            {
                Debug.Log($"Interact - {highlightedObject.name}");
                
                Destroy(highlightedObject); //Replace with interaction method, associate JSON or MonoBehavior
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
