using TMPro;
using UnityEngine;
using System.Collections;

public class TextScroll : MonoBehaviour
{
    public float typingSpeed = 0.05f;
    public float blinkSpeed = 0.5f;
    private TMP_Text textMeshPro;
    private string fullText;
    private string currentText = "";
    private bool isBlinking = false;

    void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
        fullText = textMeshPro.text;
        textMeshPro.text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in fullText.ToCharArray())
        {
            currentText += letter;
            textMeshPro.text = currentText;
            yield return new WaitForSeconds(typingSpeed);
        }

        StartCoroutine(BlinkLastLetter());
    }

    IEnumerator BlinkLastLetter()
    {
        isBlinking = true;
        while (isBlinking)
        {
            textMeshPro.text = currentText;
            yield return new WaitForSeconds(blinkSpeed);
            textMeshPro.text = currentText.Substring(0, currentText.Length - 1);
            yield return new WaitForSeconds(blinkSpeed);
        }
    }
}