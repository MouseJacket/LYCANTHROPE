using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using RenderSettings = UnityEngine.RenderSettings;

public class SkyTimer : MonoBehaviour
{
    [SerializeField] private GameObject projectionCam;
    [SerializeField] private GameObject moonLightOb;
    [SerializeField] private float intensityPoint;
    [SerializeField] private GameObject gameOverScreen;
    private Light _moonLight;
    public float timeMax;
    public float timeIn;
    public float timePercent;
    public bool paused;
    

    void Start()
    {
        RenderSettings.skybox.SetFloat("_Exposure", 1.3f);
        _moonLight = moonLightOb.GetComponent<Light>();
        _moonLight.intensity = 0f;
        paused = false;
        gameOverScreen.SetActive(false);
    }
    
    void Update()
    {
        if (!paused)
        {
            timePercent = TimeUpdate();
        }
        if (timePercent >= 1f)
        {
            Debug.Log("OUT OF TIME!!");
            paused = true;
            gameOverScreen.SetActive(true);
            Time.timeScale = 0;
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
    {//Setting up manual time scaling should allow for us to avoid some funkiness.
        timeIn += Time.deltaTime;
        return timeIn / timeMax;
    }
}
