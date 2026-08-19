using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class NoteManager : MonoBehaviour
{
    public NoteInteractionScript[] notes;
    public GameObject[] spots;
    public int code;
    public GameObject[] codeOrder;
    [SerializeField] private GameObject orderNote;
        
    void Start()
    {
        notes = FindObjectsByType<NoteInteractionScript>();
        spots = ReShuffle(GameObject.FindGameObjectsWithTag("NoteSpot"));
        codeOrder = ReShuffle(codeOrder);
        code = Random.Range(1000, 10000);
        Debug.Log($"Code - {code}");
        ApplyCode(code);
        for (int i = 0; i < notes.Length; i++)
        {
            MatchPos(notes[i].gameObject, spots[i].transform);
        }
        foreach (GameObject spot in spots)
        {
            Destroy(spot);
        }
        FormatOrderNote();
        FindAnyObjectByType<KeypadScript>().code = code.ToString();
    }
    
    GameObject[] ReShuffle(GameObject[] array) //https://discussions.unity.com/t/randomize-array-in-c/443241/5
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < array.Length; t++ )
        {
            GameObject tmp = array[t];
            int r = Random.Range(t, array.Length);
            array[t] = array[r];
            array[r] = tmp;
        }
        return array;
    }

    void ApplyCode(int code)
    {
        for (int dig = 0; dig < codeOrder.Length; dig++)
        {
            NoteInteractionScript noteScript = 
                codeOrder[dig].GetComponent<NoteInteractionScript>();
            string num = Digit(code, 3-dig).ToString();
            noteScript.noteNum = num;
            Debug.Log($"Note {dig} ({codeOrder[dig]}) - {num}");
            noteScript.Resync();
        }
    }
    int Digit(int code, int digit)
    {
        return (code/(int)Math.Pow(10,digit)) % 10;
    }
    

    void MatchPos(GameObject obj, Transform target)
    {
        obj.transform.position = target.position;
        obj.transform.rotation = target.rotation;
    }

    void FormatOrderNote()
    {
        NoteInteractionScript noteScript = orderNote.GetComponent<NoteInteractionScript>();
        string noteText = noteScript.noteText;
        for (int i = 0; i < 4; i++)
        {
            noteText = noteText.Replace($"[Color{i}]", FormatColorText(
                codeOrder[i].GetComponent<NoteInteractionScript>()));
        }
        orderNote.GetComponent<NoteInteractionScript>().noteText = noteText;
        Debug.Log("Order Note Formatted");
    }

    string FormatColorText(NoteInteractionScript note)
    {
        string textColor = UnityEngine.ColorUtility.ToHtmlStringRGB(
            note.GetComponent<NoteInteractionScript>().textColor);
        return $"<b><color=#{textColor}>{note.name.ToLower()}</color></b>";
    }
}
