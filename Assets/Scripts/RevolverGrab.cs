using UnityEngine;

public class RevolverGrab : MonoBehaviour
{
    [SerializeField] private GameObject revolverObj;
    [SerializeField] private GameObject revolverHeld;
    private InteractionPrepper _interaction;
    [SerializeField] private WerewolfTransform werewolf;

    private void Start()
    {
        _interaction = FindAnyObjectByType<InteractionPrepper>();
        werewolf = FindAnyObjectByType<WerewolfTransform>();
    }
    public void GetGun()
    {
        _interaction.hasGun = true;
        werewolf.MakeInteractable();
        revolverHeld.SetActive(true);
        Destroy(revolverObj);
    }
    
}
