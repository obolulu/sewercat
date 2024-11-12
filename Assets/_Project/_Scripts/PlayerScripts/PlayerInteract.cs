using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactRange;
    private IInteractable _interactable;
    private bool _canInteract = false;
    
    private void FixedUpdate()
    {
        CheckInteract();
        if (InputManager.InteractPressed)
        {
            Interact();
            InputManager.InteractPressed = false;
        }
    }
    
    private void Interact()
    {
        _interactable?.onInteract();
    }

    private void CheckInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactRange))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
               if(!_canInteract)
               {
                   _interactable = interactable;
                   _canInteract = true;
               }
            }
            else
            {
                if(_canInteract)
                {
                    _interactable = null;
                    _canInteract = false;
                }
            }
        }
        else
        {
            if(_canInteract)
            {
                _interactable = null;
                _canInteract = false;
            }
        }
    }
    
}