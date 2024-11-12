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
    public static bool OpenInventory;
    
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
        else if (context.canceled)
        {
            SpellChangePressed = false;
        }
    }

    private void OnOpenInventory(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            OpenInventory = true;
        }
        else if (context.canceled)
        {
            OpenInventory = false;
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
        _inputManager.PlayerControls.OpenInventory.performed += OnOpenInventory;
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
        _inputManager.PlayerControls.OpenInventory.performed -= OnOpenInventory;

    }
}
