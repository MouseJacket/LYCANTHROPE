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

public class AudioCutsceneScript : MonoBehaviour
{
    public string message;
    public float introSpeed;
    public float letterDelta;
    private Color backgroundColor;
    [SerializeField] private string nextScene;
    [SerializeField] private RawImage background;
    [SerializeField] private TextMeshProUGUI screenText;
    [SerializeField] private float postLineWait;
    [SerializeField] private float duration;
    [SerializeField] private AudioSource source;
    private bool skippable;

    private void Start()
    {
        backgroundColor = background.color;
        screenText.text = "";
        duration = source.clip.length;
        StartCoroutine(DelayedIntroFade());
        source = GetComponent<AudioSource>();
        screenText = FindAnyObjectByType<TextMeshProUGUI>();
        skippable = false;
    }

    private IEnumerator DelayedIntroFade()
    {
        yield return null;
        StartCoroutine(IntroFade());
        yield return new WaitForSeconds(1);
        skippable = true;
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
        StartCoroutine(TypeText(message));
        yield return new WaitForSeconds(duration);
    }

    private void Update()
    {
        if (InputSystem.actions["Jump"].ReadValue<float>() > 0 ||
            InputSystem.actions["Interact"].ReadValue<float>() > 0 ||
            InputSystem.actions["Attack"].ReadValue<float>() > 0)
        {
            if (skippable)
            {
                Skip();
            }
        }
    }

    public void Skip()
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

    public IEnumerator TypeText(string line)
    {
        int index = 0;
        while (index+1 <= line.Length)
        {
            screenText.text = line.Substring(0, index);
            yield return new WaitForSeconds(letterDelta);
            index++;
        }
    }
}
