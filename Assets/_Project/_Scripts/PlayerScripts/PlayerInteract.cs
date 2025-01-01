using _Project._Scripts;
using UnityEngine;
public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactRange;
    private IInteractable _interactable;
    private IInteractable _lastInteractable;
    private bool _canInteract = false;
    
    private void Update()
    {
        CheckInteract();
        if (InputManager.State.IsInteracting && _canInteract)
        {
            Interact();
            InputManager.State.IsInteracting = false;
        }
        HighlightInteractable(_interactable);
    }
    
    private void Interact()
    {
        _interactable?.onInteract();
    }

    private void HighlightInteractable(IInteractable interactable)
    {
        if (_lastInteractable != _interactable)
        {
            interactable?.Highlight();
            _lastInteractable?.EndHighlight();
        }
    }
    
    private void CheckInteract()
    {
        _lastInteractable = _interactable;
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