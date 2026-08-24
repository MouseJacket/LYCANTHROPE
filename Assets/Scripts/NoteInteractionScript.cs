using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NoteInteractionScript : MonoBehaviour
{
    [TextArea] public string noteText;
    public string noteNum;
    public Color noteColor;
    public bool reading;
    public Color textColor;
    [SerializeField] private TextMeshProUGUI readText;
    [SerializeField] private RawImage noteBackground;
    private InteractionPrepper _interaction;
    private bool _inputReleased;
    
    private void Start()
    {
        noteColor = GetComponent<MeshRenderer>().material.color;
        _interaction = FindAnyObjectByType<BasicMovement>().gameObject.GetComponent<InteractionPrepper>();
    }

    public void Resync()
    {
        string hexColor = UnityEngine.ColorUtility.ToHtmlStringRGB(textColor);
        noteText = noteText.Replace("[VAR]", $"<b><color=#{hexColor}>{noteNum}</color></b>");
        DropNote();
    }

    private void Update()
    {
        if (reading)
        {
            float inputValue = InputSystem.actions["Interact"].ReadValue<float>();
            if (inputValue == 0)
                _inputReleased = true;
            if (_inputReleased && inputValue > 0 ||
                _interaction.highlightedObject != gameObject)
            {
                DropNote();
            }
        }
    }

    public void ReadNote()
    {
        Debug.Log($"Note - {gameObject.name}");
        noteBackground.enabled = true;
        noteBackground.color = noteColor;
        readText.text = noteText;
        reading = true;
        _inputReleased = false;
    }

    public void DropNote()
    {
        readText.text = "";
        noteBackground.enabled = false;
        reading = false;
    }
}
