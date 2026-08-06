using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinds : Interaction
{
    public Animator animator;
    public string OpenAnimation;
    public string CloseAnimation;

    public CustomerAI customerScript;

    private bool isOpened = false;
    public override void Click()
    {
        isOpened = !isOpened;
        if (isOpened == true)
        {
            animator.Play(OpenAnimation);
            customerScript.AIRestart();
        }
        else if (isOpened == false)
        {
            animator.Play(CloseAnimation);
            Invoke("Close", animator.playbackTime);
        }
    }

    private void Close()
    {
        customerScript.Delete();
    }
}
