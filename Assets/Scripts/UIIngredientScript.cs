using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIIngredientScript : MonoBehaviour,IEndDragHandler,IDragHandler
{
    private RectTransform _rect;
    private Cure _cure;
    private Canvas _canvas;
    [SerializeField] private RawImage syringeDrop;
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        _canvas = FindAnyObjectByType<Canvas>();
        raycaster = FindAnyObjectByType<GraphicRaycaster>();
        eventSystem = FindAnyObjectByType<EventSystem>();
        _cure = FindAnyObjectByType<Cure>();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        // Check if we hit any UI elements
        foreach (RaycastResult result in results)
        {
            // Check if the hit object has the component/tag/layer you want
            if (result.gameObject == syringeDrop.gameObject)
            {
                _cure.EnterIngredient(gameObject);
                return; // Exit after successful drop
            }
        }
        Reset();
    }

    public void Reset()
    {
        transform.localPosition = new Vector3(0, 0, 0);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.GetComponent<RectTransform>(), Input.mousePosition, 
            Camera.main, out mousePos);
    
        if (_rect.parent != _canvas.transform)
        {
            mousePos = _rect.parent.GetComponent<RectTransform>().InverseTransformPoint(
                _canvas.transform.TransformPoint(mousePos));
        }
        _rect.anchoredPosition = mousePos;
    }
}