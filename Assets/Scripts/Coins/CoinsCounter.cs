using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsCounter : MonoBehaviour
{
    public static int coinsQuantity;
    private TextMeshProUGUI counterText;

    private void Awake()
    {
        counterText = GetComponentInChildren<TextMeshProUGUI>();
        counterText.text = coinsQuantity.ToString();

    }

    public void UpdateText(int value)
    {
        coinsQuantity += value;
        counterText.text = coinsQuantity.ToString();
    }
}
