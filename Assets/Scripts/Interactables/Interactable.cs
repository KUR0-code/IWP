using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // use it for interaction events 
    public bool useEvent;
    [SerializeField]
    public string PromptMessage;

    public void BaseInteract()
    {
        if(useEvent)
        {
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        }

        Interact();
    }
    protected virtual void Interact()
    {
        // to be over written
    }
}
