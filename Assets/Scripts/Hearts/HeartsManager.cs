using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsManager : MonoBehaviour
{
    public static int heartsQuantity = 3;
    [SerializeField] private Heart[] hearts = new Heart[3];

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            LostHearts(1);
        }
    }

    public void LostHearts(int value)
    {
        if (heartsQuantity > 0)
        {
            heartsQuantity -= value;
            hearts[heartsQuantity].PlayAnim();
        }
    }

    public int GetHeartsQuantity()
    {
        return heartsQuantity;
    }
}
