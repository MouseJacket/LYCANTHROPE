using System;
using UnityEngine;

public class SyringeScript : MonoBehaviour
{
    private NoteManager _noteMan;
    private BasicMovement _movement;
    private GameObject[] _ingredients;
    private GameObject[] _inventoryIngredients;
    private bool interacting;
    public string code;
    [SerializeField] private string codeAttempt;
    [SerializeField] private GameObject resume;

    private void Start()
    {
        code = "1234";
        _noteMan = FindAnyObjectByType<NoteManager>();
        _movement = FindAnyObjectByType<BasicMovement>();
    }

    public void ToggleInteraction()
    {
        resume.SetActive(true);
        _movement.MenuInteraction(true);
        resume.SetActive(true);
        interacting = true;
        
    }

    
}
