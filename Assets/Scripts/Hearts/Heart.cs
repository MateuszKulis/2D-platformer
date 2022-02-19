using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private Animator animator;

    private void OnEnable()
    {
        animator = this.GetComponent<Animator>();
        animator.enabled = false;
    }

    public void PlayAnim()
    {
        animator.enabled = true;
    }

    public void EndAnim()
    {
        gameObject.SetActive(false);
    }
}
