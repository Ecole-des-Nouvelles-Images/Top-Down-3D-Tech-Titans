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

    void Start()
    {
        button = GetComponent<Button>();
        buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        ResetColors();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = highlightedTextColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetColors();
    }

    private void ResetColors()
    {
        buttonText.color = normalTextColor;
    }
}