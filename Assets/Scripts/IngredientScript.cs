using System;
using UnityEngine;

public class IngredientScript : MonoBehaviour
{
    [SerializeField] private NoteManager _noteManager;
    [SerializeField] private GameObject _ingredient;

    private void Start()
    {
        _noteManager = FindAnyObjectByType<NoteManager>();
        _ingredient = transform.parent.gameObject;
        if (!_noteManager.ingredients.ContainsKey(_ingredient.name))
        {
            _noteManager.ingredients.Add(_ingredient.name,false);
        }
        
        
    }
    public void Grab()
    {
        _noteManager.ingredients[_ingredient.name] = true;
        Debug.Log($"Got {_ingredient.name}");
        Destroy(_ingredient);
    }
}
