using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionButton : Interaction
{
    public AudioClip ClickSound;
    public Animation AnimationPress;
    public UnityEvent onPress;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public override void Click()
    {
        Debug.Log("BUTTON CLICKED");
        audioSource.PlayOneShot(ClickSound);
        onPress.Invoke();

    }
}