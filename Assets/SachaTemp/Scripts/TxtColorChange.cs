using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;
    public TextMeshProUGUI buttonText;
    public Color normalTextColor = Color.green;
    public Color highlightedTextColor = Color.black;
    public AudioClip highlightSound; 
    private AudioSource audioSource; 

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = highlightedTextColor;
        PlayHighlightSound();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetColors();
    }

    
    public void OnPointerEnter(BaseEventData eventData)
    {
        buttonText.color = highlightedTextColor;
        PlayHighlightSound(); 
    }

    public void OnPointerExit(BaseEventData eventData)
    {
        ResetColors();
    }

    private void PlayHighlightSound()
    {
       
        if (highlightSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(highlightSound);
        }
    }

    private void ResetColors()
    {
        buttonText.color = normalTextColor;
    }
}