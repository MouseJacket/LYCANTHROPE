using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class KeypadScript : MonoBehaviour
{
    private BasicMovement _movement;
    public string code;
    public bool interacting;
    [SerializeField] private string codeAttempt;
    [SerializeField] private GameObject resume;
    [SerializeField] private Button[] buttons;
    void Start()
    {
        _movement = FindAnyObjectByType<BasicMovement>();
        for (int i = 0; i < 10; i++)
        {
            int buttonNum = i;
            buttons[i].onClick.AddListener(() => ButtonPush(buttonNum));// => lambda function
        }
        resume.GetComponent<Button>().onClick.AddListener(KeypadDown);
        code = FindAnyObjectByType<NoteManager>().code.ToString();
        KeypadDown();
    }

    // void Update()
    // {
    //     if (interacting)
    //     {
    //         if (InputSystem.actions["Interact"].ReadValue<float>() > 0)
    //         {
    //             KeypadDown(); //Need to add additional tick coroutine
    //         }
    //     }
    // }

    public void KeypadUp()
    {
        _movement.MenuInteraction(true);
        resume.SetActive(true);
        interacting = true;
    }

    public void KeypadDown()
    {
        _movement.MenuInteraction(false);
        resume.SetActive(false);
        codeAttempt = "";
        interacting = false;
    }

    private void CodeCorrect()
    {
        KeypadDown();
        Destroy(gameObject);
    }

    private void CodeIncorrect()
    {
        Debug.Log("Incorrect code");
        codeAttempt = "";
    }

    void ButtonPush(int value)
    {
        codeAttempt = string.Concat(codeAttempt,value.ToString());
        if (codeAttempt.Length >= 4)
        {
            if (codeAttempt == code)
            {
                CodeCorrect();
            }
            else
            {
                CodeIncorrect();
            }
        }
        Debug.Log($"Button Push - {value}");
        
    }
}
