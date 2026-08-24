using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Cure : MonoBehaviour
{
    public string code;
    [SerializeField] private string codeAttempt;
    [SerializeField] private GameObject resume;
    [SerializeField] private UIIngredientScript[] ingredients;
    [SerializeField] private GameObject syringeInspect;
    [SerializeField] private GameObject syringeHold;
    [SerializeField] private GameObject revolverHold;
    private UIIngredientScript[] _uiIngredients;
    private NoteManager _noteMan;
    private BasicMovement _movement;
    private InteractionPrepper _interact;
    private WerewolfTransform _werewolf;
    //private Camera _cam;
    
    
    void Start()
    {
        ingredients = FindObjectsByType<UIIngredientScript>();
        //_cam = Camera.main;
        _uiIngredients = FindObjectsByType<UIIngredientScript>();
        _movement = FindAnyObjectByType<BasicMovement>();
        _noteMan = FindAnyObjectByType<NoteManager>();
        _werewolf = FindAnyObjectByType<WerewolfTransform>();
        _interact = FindAnyObjectByType<InteractionPrepper>();

        
        resume.GetComponent<Button>().onClick.AddListener(MenuDown);
        MenuDown();
    }

    public void MenuUp()
    {
        _movement.MenuInteraction(true);
        resume.SetActive(true);
        syringeInspect.SetActive(true);
        foreach (UIIngredientScript ingredient in ingredients)
        {
            GameObject obj = ingredient.gameObject;
            GameObject parObj = obj.transform.parent.gameObject;
            parObj.SetActive(_noteMan.ingredients[obj.name]);
        }
    }

    public void MenuDown()
    {
        Debug.Log("MenuClose");
        Reset();
        _movement.MenuInteraction(false);
        resume.SetActive(false);
        codeAttempt = "";
        syringeInspect.SetActive(false);
    }

    private void CodeCorrect()
    {
        MenuDown();
        _interact.hasSyringe = true;
        syringeHold.SetActive(true);
        if (_interact.hasGun)
        {
            _interact.hasGun = false;
            revolverHold.SetActive(false);
        }
        _werewolf.MakeInteractable();
        Destroy(gameObject);
    }

    private void Reset()
    {
        Debug.Log($"Incorrect code - {codeAttempt}");
        codeAttempt = "";
         
        foreach (UIIngredientScript ingredient in _uiIngredients)
        {
            ingredient.gameObject.SetActive(true);
            ingredient.Reset();
        }
    }
    public void EnterIngredient(GameObject ingredient)
    {
        codeAttempt = string.Concat(codeAttempt,ingredient.name[0]);
        ingredient.SetActive(false);
        if (codeAttempt.Length >= 4)
        {
            if (codeAttempt == code)
            {
                CodeCorrect();
            }
            else
            {
                Reset();
            }
        }
        Debug.Log($"Button Push - {ingredient.name[0]}");
    }
}
