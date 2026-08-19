using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class InteractionPrepper : MonoBehaviour
{
    private BasicMovement movement;
    public GameObject highlightedObject;
    public bool hasGun = false;
    [SerializeField] private float selectDist;
    [SerializeField] private Material selectMat;
    [SerializeField] private Material nativeMat;
    [SerializeField] private Camera cam;
    [SerializeField] private Vector3 lookDir;
    [SerializeField] private Image winScreen;
    [SerializeField] private TextMeshProUGUI readText;
    
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
                if (highlightedObject.GetComponent<NoteInteractionScript>())
                {
                    highlightedObject.GetComponent<NoteInteractionScript>().ReadNote();
                }
                else if (highlightedObject.GetComponent<KeypadScript>())
                {
                    highlightedObject.GetComponent<KeypadScript>().KeypadUp();
                }
                else if (highlightedObject.GetComponent<RevolverGrab>())
                {
                    highlightedObject.GetComponent<RevolverGrab>().GetGun();
                }
                else if (highlightedObject.GetComponent<WerewolfTransform>())
                {
                    if (hasGun)
                    {
                        RevolverEnding();
                    }
                }
                else
                {
                    Debug.Log($"Interact - {highlightedObject.name}");
                    Destroy(highlightedObject); //Replace with interaction method, associate JSON or MonoBehavior
                    highlightedObject = null;
                }
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

    void RevolverEnding()
    {
        winScreen.gameObject.SetActive(true);
        readText.text = "You Killed Lucas Morgan";
        DeselectObject();
        Time.timeScale = 0;
    }
}
