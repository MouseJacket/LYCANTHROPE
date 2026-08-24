using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //Super helpful it turns out, can check if all objects in an aggregate return a bool value in one line
//https://www.youtube.com/watch?v=38BtpQWEct4
public class WerewolfTransform : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject[] states;
    [SerializeField] private float increment;
    [SerializeField] private SkyTimer timer;
    [SerializeField] private int currentState;
    private int stateDelay;
    private MeshFilter _filter;
    
    void Start()
    {
        cam = Camera.main;
        increment = 1f/states.Length;
        _filter = GetComponent<MeshFilter>();
        timer = FindAnyObjectByType<SkyTimer>();
        foreach (GameObject state in states)
        {
            state.tag = "Untagged";
        }
        currentState = 0;
        UpdateState(currentState);
    }
    void Update()
    {
        currentState = (int)Math.Floor(timer.timePercent / increment);
        if (!IsVisible())
        {
            if (stateDelay != currentState)
            {
                Debug.Log($"State Change - {currentState}");
                UpdateState(currentState);
                stateDelay = currentState;
            }
        }
    }

    private void UpdateState(int current)
    {
        foreach (GameObject state in states)
        {
            if (state == states[current])
            {
                state.SetActive(true);
            }
            else
            {
                state.SetActive(false);
            }
        }
    }

    public void MakeInteractable()
    {
        gameObject.tag = "Selectable";
        foreach (GameObject state in states)
        {
            state.tag = "Selectable";
        }
    }
    private bool IsVisible()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        return planes.All(plane => plane.GetDistanceToPoint(transform.position) >= 0); //Linq.All()
    }
    
}
