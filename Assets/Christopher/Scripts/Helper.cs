using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static void PourcentStateBarre(RectTransform rectTransform, char axe, float currentValue, float maxValue) //modifie le scale du rectTransform selon l'axe désigné
    {
        char axeX = 'x';
        char axeY = 'y';
        char axeZ = 'z';
        LimitSystem(currentValue, maxValue);
        if(axe == axeX) rectTransform.localScale = new Vector3(CurrentPourcentValue(currentValue, maxValue) / 100, 1f, 1f);
        else if(axe == axeY) rectTransform.localScale = new Vector3(1f, CurrentPourcentValue(currentValue, maxValue) / 100, 1f);
        else if (axe == axeZ) rectTransform.localScale = new Vector3(1f, 1f, CurrentPourcentValue(currentValue, maxValue) / 100);
        else
        {
            Debug.Log("Axe incorrect, renvoi de valeur à défaut sur l'axe X.");
            rectTransform.localScale = new Vector3(CurrentPourcentValue(currentValue, maxValue) / 100, 1f, 1f);
        }

    }
    public static void LimitSystem(float current,float max)
    {
        if(current > max)current = max;
    }

    public static float CurrentPourcentValue(float value, float maxValue)
    {
        return value * 100 / maxValue;

    }
}
