using UnityEditor;

[CustomEditor(typeof(Interactable), true)]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Interactable interactable = (Interactable)target;
        if(target.GetType() == typeof(EventOnly))
        {
            interactable.PromptMessage = EditorGUILayout.TextField("Prompt Message", interactable.PromptMessage);
            EditorGUILayout.HelpBox("EventOnlyInteract Can only use unity events", MessageType.Info);

            if(interactable.GetComponent<InteractionEvent>() == null)
            {
                interactable.useEvent = true;
                interactable.gameObject.AddComponent<InteractionEvent>();
            }
        }
        else
        {
            base.OnInspectorGUI();
            if (interactable.useEvent)
            {
                // adding the components
                if (interactable.GetComponent<InteractionEvent>() == null)
                {
                    interactable.gameObject.AddComponent<InteractionEvent>();
                }
            }
            else // remove the components
            {
                if (interactable.GetComponent<InteractionEvent>() != null)
                {
                    DestroyImmediate(interactable.GetComponent<InteractionEvent>());
                }
            }
        }
       
    }
}
