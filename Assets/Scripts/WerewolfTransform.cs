using System;
using UnityEngine;
using System.Linq; //Super helpful it turns out, can check if all objects in an aggregate return a bool value in one line
//https://www.youtube.com/watch?v=38BtpQWEct4
public class WerewolfTransform : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Mesh[] states;
    [SerializeField] private float increment;
    [SerializeField] private SkyTimer timer;
    [SerializeField] private int currentState;
    private MeshFilter _filter;
    
    void Start()
    {
        cam = Camera.main;
        increment = 1f/states.Length;
        _filter = GetComponent<MeshFilter>();
        timer = FindAnyObjectByType<SkyTimer>();
        _filter.mesh = states[0];
        gameObject.tag = "Untagged";
    }
    void Update()
    {
        currentState = (int)Math.Floor(timer.timePercent / increment);
        if (!IsVisible())
        {
            if (_filter.mesh != states[currentState])
            {
                _filter.mesh = states[currentState];
            }
        }
    }

    private bool IsVisible()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        return planes.All(plane => plane.GetDistanceToPoint(transform.position) >= 0); //Linq.All()
    }
    
}
