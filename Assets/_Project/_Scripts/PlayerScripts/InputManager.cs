using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private PlayerInputs _inputManager;
    public static Vector3 moveDirection;
    
    
    public static bool StartJump;
    public static bool CastPressed;
    public static bool SpellChangePressed;
    public static bool InteractPressed;

    
    private bool _castPressed;
    private bool _jumpPressed;
    private bool _leftClickDown;
    private bool _leftClickUp;
    void Awake()
    {
        _inputManager = new PlayerInputs();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        
        if (context.performed)
        {
            StartJump = true;
        }
        else if (context.canceled)
        {
            StartJump = false;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = Vector2.ClampMagnitude(_inputManager.PlayerControls.Move.ReadValue<Vector2>(),1f);
    }

    private void OnCast(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CastPressed = true;
        }
        
        else if (context.canceled)
        {
            CastPressed = false;
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            InteractPressed = true;
        }
        else if (context.canceled)
        {
            InteractPressed = false;
        }
    }
    private void OnChangeSpell(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SpellChangePressed = true;
        }
    }
    
    private void OnEnable()
    {
        _inputManager.Enable();
        _inputManager.PlayerControls.Jump.performed += OnJump;
        _inputManager.PlayerControls.Jump.canceled += OnJump;
        _inputManager.PlayerControls.Move.performed += OnMove;
        _inputManager.PlayerControls.Move.canceled +=  OnMove;
        _inputManager.PlayerControls.CastSpell.performed += OnCast;
        _inputManager.PlayerControls.CastSpell.canceled += OnCast;
        _inputManager.PlayerControls.Interact.performed += OnInteract;
        _inputManager.PlayerControls.ChangeSpell.performed += OnChangeSpell;
    }

    private void OnDisable()
    {
        _inputManager.Disable();
        _inputManager.PlayerControls.Jump.performed -= OnJump;
        _inputManager.PlayerControls.Jump.canceled -= OnJump;
        _inputManager.PlayerControls.Move.performed -= OnMove;
        _inputManager.PlayerControls.Move.canceled -=  OnMove;
        _inputManager.PlayerControls.CastSpell.performed -= OnCast;
        _inputManager.PlayerControls.CastSpell.canceled -= OnCast;
        _inputManager.PlayerControls.Interact.performed -= OnInteract;
        _inputManager.PlayerControls.ChangeSpell.performed -= OnChangeSpell;
    }

    void Update()
    {
        
    }
}
