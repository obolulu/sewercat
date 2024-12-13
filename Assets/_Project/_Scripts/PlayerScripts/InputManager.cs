using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInputs _inputManager;
    public static Vector2 moveDirection;
    public static Vector3 moveDirectionNormalized => moveDirection.normalized;
    
    public static Vector3 MousePosition => Mouse.current.position.ReadValue();
    
    public static bool StartJump;
    public static bool CastPressed;
    public static bool SpellChangePressed;
    public static bool InteractPressed;
    public static bool OpenInventory;
    
    public static bool SaveButtonPressed;
    public static bool LoadButtonPressed;
    
    public static event Action SaveGame;
    public static event Action LoadGame;
    
    public static event Action OpenInventoryEvent;
    public static event Action CloseInventoryEvent;
    
    public static event Action LeftClickDown;
    public  static  event  Action LeftClickUp;
    
    public static event Action RightClickDown;
    public static event Action RightClickUp;
    
    public static event Action OpenPauseMenu;

    public static event Action OpenInventoryMenu;
    
    private bool _castPressed;
    private bool _jumpPressed;
    private bool _leftClickDown;
    private bool _leftClickUp;

    public static Vector2 GetMouseDelta()
    {
        return Mouse.current.delta.ReadValue();
    }
    
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
        moveDirection = _inputManager.PlayerControls.Move.ReadValue<Vector2>();
    }

    private void OnLeftMouseDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LeftClickDown?.Invoke();
        }
        
        else if (context.canceled)
        {
            LeftClickUp?.Invoke();
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
            OpenInventoryEvent?.Invoke();
        }
        else if(context.canceled)
        {
            CloseInventoryEvent?.Invoke();
        }
    }
    private void OnOpenInventoryMenu(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            OpenInventoryMenu?.Invoke();
        }
    }

    private void OnSaveGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SaveGame?.Invoke();
        }
    }

    private void OnLoadGame(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            LoadGame?.Invoke();
        }
    }
    
    private void OnopenMenu(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            OpenPauseMenu?.Invoke();
        }
    }

    private void HandleMenuToggle(bool menuOpen)
    {
        if (menuOpen)
        {
            _inputManager.PlayerControls.Disable();
            _inputManager.UI.Enable();
        }
        else
        {
            _inputManager.PlayerControls.Enable();
            _inputManager.UI.Disable();        
        }
    }
    private void OnCloseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UIManager.Instance.CloseCurrentMenu();
        }
    }

    
    private void OnEnable()
    {
        _inputManager.Enable();
        _inputManager.PlayerControls.Jump.performed          += OnJump;
        _inputManager.PlayerControls.Jump.canceled           += OnJump;
        _inputManager.PlayerControls.Move.performed          += OnMove;
        _inputManager.PlayerControls.Move.canceled           += OnMove;
        _inputManager.PlayerControls.CastSpell.performed     += OnLeftMouseDown;
        _inputManager.PlayerControls.CastSpell.canceled      += OnLeftMouseDown;
        _inputManager.PlayerControls.Interact.performed      += OnInteract;
        _inputManager.PlayerControls.ChangeSpell.performed   += OnChangeSpell;
        _inputManager.PlayerControls.OpenInventory.performed += OnOpenInventory;
        _inputManager.PlayerControls.Save.performed          += OnSaveGame;
        _inputManager.PlayerControls.Load.performed          += OnLoadGame;
        _inputManager.PlayerControls.OpenPauseMenu.performed += OnopenMenu;
        _inputManager.PlayerControls.OpenInventoryMenu.performed += OnOpenInventoryMenu;


        _inputManager.UI.CloseMenu.performed += OnCloseMenu;
        
        UIManager.OnMenuToggle += HandleMenuToggle;

    }

    private void OnDisable()
    {
        _inputManager.Disable();
        _inputManager.PlayerControls.Jump.performed              -= OnJump;
        _inputManager.PlayerControls.Jump.canceled               -= OnJump;
        _inputManager.PlayerControls.Move.performed              -= OnMove;
        _inputManager.PlayerControls.Move.canceled               -= OnMove;
        _inputManager.PlayerControls.CastSpell.performed         -= OnLeftMouseDown;
        _inputManager.PlayerControls.CastSpell.canceled          -= OnLeftMouseDown;
        _inputManager.PlayerControls.Interact.performed          -= OnInteract;
        _inputManager.PlayerControls.ChangeSpell.performed       -= OnChangeSpell;
        _inputManager.PlayerControls.OpenInventory.performed     -= OnOpenInventory;
        _inputManager.PlayerControls.Save.performed              -= OnSaveGame;
        _inputManager.PlayerControls.Load.performed              -= OnLoadGame;
        _inputManager.PlayerControls.OpenPauseMenu.performed     -= OnopenMenu;
        _inputManager.PlayerControls.OpenInventoryMenu.performed -= OnOpenInventoryMenu;
        
        _inputManager.UI.CloseMenu.performed -= OnCloseMenu;

        UIManager.OnMenuToggle -= HandleMenuToggle;
    }
}
