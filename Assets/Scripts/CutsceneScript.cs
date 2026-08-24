using System;
using System.Collections;
using TMPro;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class CutsceneScript : MonoBehaviour
{
    
    public string[] script;
    public AudioClip[] audioScript;
    public Color[] textColor;
    public float introSpeed;
    public int currentLine;
    public float letterDelta;
    private Color backgroundColor;
    private float storedTime;
    [SerializeField] private bool skippable;
    [SerializeField] private string nextScene;
    [SerializeField] private RawImage background;
    [SerializeField] private TextMeshProUGUI screenText;
    [SerializeField] private float lineBuffer;
    [SerializeField] private float pauseLength;
    [SerializeField] private float postLineWait;
    [SerializeField] private float pitchShift;
    private AudioSource _audio;

    
    
    private void Start()
    {
        backgroundColor = background.color;
        screenText.text = "";
        _audio = GetComponent<AudioSource>();
        skippable = true;
        StartCoroutine(DelayedIntroFade());
    }

    private IEnumerator DelayedIntroFade()
    {
        yield return null; // Wait one frame for UI initialization
        StartCoroutine(IntroFade());
    }

    public IEnumerator IntroFade()
    {
        float r = backgroundColor.r;
        float g = backgroundColor.g;
        float b = backgroundColor.b;
        float fadePercent = 0;
        while (fadePercent < 1)
        {
            fadePercent += Time.deltaTime * introSpeed;
            background.color = new Color(r, g, b, Mathf.Lerp(1, 0, fadePercent));
            yield return new WaitForSeconds(Time.deltaTime);
        }
        nextLine();
    }

    private void Update()
    {
        if (Time.time >= storedTime + lineBuffer)
        {
            skippable = true;
        }
        if (InputSystem.actions["Jump"].ReadValue<float>() > 0 ||
            InputSystem.actions["Interact"].ReadValue<float>() > 0 ||
            InputSystem.actions["Attack"].ReadValue<float>() > 0)
        {
            if (skippable)
            {
                nextLine();
            }
        }
    }

    public void nextLine()
    {
        storedTime = Time.time;
        skippable = false;
        StopAllCoroutines();
        background.color = new Color(0, 0, 0, 0);
        screenText.text = "";
        currentLine += 1;
        if (currentLine >= script.Length)
        {
            if (nextScene != "")
            {
                Debug.Log("Loading Scene");
                SceneManager.LoadScene(nextScene);
            }
            else
            {
                Debug.Log("Quitting");
                Application.Quit();
            }
        }
        
        screenText.color = textColor[currentLine];
        
        if (audioScript[currentLine] != null)
        {
            PlayAudio(currentLine);
        }
        
        
        StartCoroutine(TypeText(script[currentLine]));
        
    }

    private void PlayAudio(int clip)
    {
        _audio.clip = audioScript[clip];
        _audio.pitch = UnityEngine.Random.Range(1 - pitchShift, 1 + pitchShift);
        _audio.Play();
    }

    public IEnumerator TypeText(string line)
    {
        int index = 0;
        while (index+1 <= line.Length)
        {
            
            screenText.text = line.Substring(0, index);
            if ((line[index] == '.' && line[index + 1] == '.') || line[index] == ',')
            {
                Debug.Log("pause");
                yield return new WaitForSeconds(letterDelta*pauseLength);
            }
            else
            {
                yield return new WaitForSeconds(letterDelta);
            }
            index++;
        }

        yield return new WaitForSeconds(postLineWait);
        nextLine();
    }
}
