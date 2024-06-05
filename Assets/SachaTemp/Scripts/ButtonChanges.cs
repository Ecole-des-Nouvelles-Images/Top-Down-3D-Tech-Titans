using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonColorChange : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Button button;
    public TextMeshProUGUI buttonText;
    public Color normalTextColor = Color.green;
    public Color selectedTextColor = Color.black;
    private AudioSource audioSource; // Référence à l'AudioSource
    public AudioClip selectSound; // Clip audio à jouer lors de la sélection du bouton

    void Start()
    {
        button = GetComponent<Button>();
        buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        ResetColors();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (selectSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(selectSound);
        }
        buttonText.color = selectedTextColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        audioSource.Stop();
        ResetColors();
    }

    private void ResetColors()
    {
        buttonText.color = normalTextColor;
    }
}
