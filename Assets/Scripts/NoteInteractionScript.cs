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
    private InteractionPrepper _interaction;
    private void Start()
    {
        readText = GameObject.Find("NoteText").GetComponent<TextMeshProUGUI>();
        noteBackground = GameObject.Find("NoteBackground").GetComponent<RawImage>();
        noteColor = GetComponent<MeshRenderer>().material.color;
        _interaction = FindAnyObjectByType<BasicMovement>().gameObject.GetComponent<InteractionPrepper>();
        string hexColor = UnityEngine.ColorUtility.ToHtmlStringRGB(textColor);
        Debug.Log($"{gameObject.name} text color - {hexColor}");
        noteText = noteText.Replace("[VAR]", $"<color=#{hexColor}>{noteNum}</color>");
        DropNote();
    }

    private void Update()
    {
        if (reading)
        {
            if (_interaction.highlightedObject!= gameObject||
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
