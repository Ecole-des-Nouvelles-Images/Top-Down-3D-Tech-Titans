using TMPro;
using UnityEngine;
using System;
using System.Collections;


public class SetTimeTxt : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public string customText = "07/06/2152 ";

    void Start()
    {
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TMP_Text>();
        }
        StartCoroutine(UpdateTime());
    }

    IEnumerator UpdateTime()
    {
        while (true)
        {
            DateTime now = DateTime.Now;
            textMeshPro.text = customText + now.ToString("HH:mm:ss");
            yield return new WaitForSeconds(1f);
        }
    }
}