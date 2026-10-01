using UnityEngine;

public class MaintanenceManager : MonoBehaviour
{
    [SerializeField] private int frameRate;
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frameRate;
    }
}
