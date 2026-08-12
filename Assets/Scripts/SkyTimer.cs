using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using RenderSettings = UnityEngine.RenderSettings;

public class SkyTimer : MonoBehaviour
{
    [SerializeField] private GameObject projectionCam;
    [SerializeField] private GameObject moonLightOb;
    [SerializeField] private float intensityPoint;
    private Light _moonLight;
    public float timeMax;
    public float timeIn;
    public float timePercent;

    void Start()
    {
        RenderSettings.skybox.SetFloat("_Exposure", 1.3f);
        _moonLight = moonLightOb.GetComponent<Light>();
        _moonLight.intensity = 0f;
    }
    
    void Update()
    {
        timePercent = TimeUpdate();
        if (timePercent >= 1f)
        {
            Debug.Log("OUT OF TIME!!");
        }
        
        if (timePercent >= intensityPoint)
        {
            float newPercent = (timePercent - intensityPoint) / (1 - intensityPoint);
            RenderSettings.skybox.SetFloat("_Exposure",Mathf.Lerp(1.3f,2.6f,newPercent));
            _moonLight.intensity = Mathf.Lerp(0f, 5, newPercent);
        }
        projectionCam.transform.eulerAngles = new Vector3(Mathf.Lerp(90, 0, timePercent),0,0);
        moonLightOb.transform.eulerAngles = new Vector3(Mathf.Lerp(0, 90, timePercent),-90,0);
    }

    float TimeUpdate()
    {
        timeIn += Time.deltaTime;
        return timeIn / timeMax;
    }
}
