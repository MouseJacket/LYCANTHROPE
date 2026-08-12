using System;
using UnityEngine;

public class SkyTimer : MonoBehaviour
{
    [SerializeField] private GameObject projectionCam;
    public float timeMax;
    public float timeIn;
    public float timePercent;

    void Start()
    {
        RenderSettings.skybox.SetFloat("_Exposure", 1.3f);
    }
    
    void Update()
    {
        timePercent = TimeUpdate();
        
        if (timePercent >= 0.9f)
        {
            RenderSettings.skybox.SetFloat("_Exposure",Mathf.Lerp(1.3f,2.6f,(timePercent-0.9f)/0.1f));
        }
        projectionCam.transform.eulerAngles = new Vector3(Mathf.Lerp(90, 0, timePercent),0,0);
    }

    float TimeUpdate()
    {
        timeIn += Time.deltaTime;
        return timeIn / timeMax;
    }
}
