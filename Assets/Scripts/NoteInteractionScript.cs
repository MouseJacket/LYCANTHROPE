using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NoteInteractionScript : MonoBehaviour
{
    public string noteText;
    public string noteNum;
    public Color noteColor;
    public bool reading;
    [SerializeField] private Color textColor;
    [SerializeField] private TextMeshProUGUI readText;
    [SerializeField] private RawImage noteBackground;
    private BasicMovement _movement;
    private void Start()
    {
        noteColor = GetComponent<MeshRenderer>().material.color;
        _movement = FindAnyObjectByType<BasicMovement>().gameObject.GetComponent<BasicMovement>();
        string hexColor = UnityEngine.ColorUtility.ToHtmlStringRGB(textColor);
        Debug.Log($"{gameObject.name} text color - {hexColor}");
        noteText = noteText.Replace("[VAR]", $"<color=#{hexColor}>{noteNum}</color>");
        DropNote();
    }

    private void Update()
    {
        if (reading)
        {
            if (_movement.lookInput.x != 0 || _movement.lookInput.y != 0 ||
                _movement.moveInput.x != 0 || _movement.moveInput.y != 0 ||
                InputSystem.actions["Interact"].ReadValue<float>() > 0)
            {
                DropNote();
            }
        }
    }

    public void ReadNote()
    {
        noteBackground.enabled = true;
        noteBackground.color = noteColor;
        readText.text = noteText;
        reading = true;
    }

    public void DropNote()
    {
        readText.text = "";
        noteBackground.enabled = false;
        reading = false;
    }
}
