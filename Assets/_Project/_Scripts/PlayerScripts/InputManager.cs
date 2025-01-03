using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private PlayerInputs _inputs;
    
    public class InputState
    {
        public Vector2 MoveDirection { get; set; }
        public Vector2 MousePosition => Mouse.current.position.ReadValue();
        public Vector2 MouseDelta => Mouse.current.delta.ReadValue();
        public bool IsJumping { get; set; }
        public bool IsCasting { get; set; }
        public bool IsChangingSpell { get; set; }
        public bool IsInteracting { get; set; }
    }
    
    public static InputState State { get; private set; } = new InputState();

    public static event Action SaveGame;
    public static event Action LoadGame;
    public static event Action OpenInventory;
    public static event Action CloseInventory;
    public static event Action LeftClickDown;
    public static event Action LeftClickUp;
    public static event Action RightClickDown;
    public static event Action RightClickUp;
    public static event Action OpenPauseMenu;
    public static event Action OpenInventoryMenu;
    public static event Action Crouch;
    public static event Action PutWeaponDown;
    public static event Action Special;
    public static event Action<int> HotbarSlotSelected;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        _inputs = new PlayerInputs();
        BindInputActions();
    }

    private void BindInputActions()
    {
        BindAction(_inputs.PlayerControls.Move, OnMove);
        BindContextAction(_inputs.PlayerControls.Jump, OnJump);
        BindContextAction(_inputs.PlayerControls.CastSpell, OnCastSpell);
        BindContextAction(_inputs.PlayerControls.RightClick, OnRightClick);
        BindContextAction(_inputs.PlayerControls.Interact, OnInteract);
        
        BindPerformedAction(_inputs.PlayerControls.OpenInventory, _ => OpenInventory?.Invoke());
        BindPerformedAction(_inputs.PlayerControls.OpenInventoryMenu, _ => OpenInventoryMenu?.Invoke());
        BindPerformedAction(_inputs.PlayerControls.OpenPauseMenu, _ => OpenPauseMenu?.Invoke());
        
        BindPerformedAction(_inputs.PlayerControls.Save, _ => SaveGame?.Invoke());
        BindPerformedAction(_inputs.PlayerControls.Load, _ => LoadGame?.Invoke());
        
        BindPerformedAction(_inputs.PlayerControls.Crouch, _ => Crouch?.Invoke());
        BindPerformedAction(_inputs.PlayerControls.PutWeaponDown, _ => PutWeaponDown?.Invoke());
        BindPerformedAction(_inputs.PlayerControls.Special, _ => Special?.Invoke());
        
        BindPerformedAction(_inputs.UI.CloseMenu, OnCloseMenu);
        
        BindHotbarKeys();
    }

    private void BindHotbarKeys()
    {
        var hotbarActions = new[]
        {
            _inputs.PlayerControls.One,
            _inputs.PlayerControls.Two,
            _inputs.PlayerControls.Three,
            _inputs.PlayerControls.Four,
            _inputs.PlayerControls.Five,
        };

        for (int i = 0; i < hotbarActions.Length; i++)
        {
            int slotIndex = i;
            BindPerformedAction(hotbarActions[i], _ => HotbarSlotSelected?.Invoke(slotIndex));
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        State.MoveDirection = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        State.IsJumping = context.performed;
    }

    private void OnCastSpell(InputAction.CallbackContext context)
    {
        if (context.performed)
            LeftClickDown?.Invoke();
        else if (context.canceled)
            LeftClickUp?.Invoke();
    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.performed)
            RightClickDown?.Invoke();
        else if (context.canceled)
            RightClickUp?.Invoke();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        State.IsInteracting = context.performed;
    }

    private void OnCloseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
            UIManager.Instance.CloseCurrentMenu();
    }

    public void HandleMenuToggle(bool menuOpen)
    {
        if (menuOpen)
        {
            _inputs.PlayerControls.Disable();
            _inputs.UI.Enable();
        }
        else
        {
            _inputs.PlayerControls.Enable();
            _inputs.UI.Disable();
        }
    }

    private void BindAction(InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.performed += callback;
        action.canceled += callback;
    }

    private void BindContextAction(InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.performed += callback;
        action.canceled += callback;
    }

    private void BindPerformedAction(InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.performed += callback;
    }

    private void OnEnable()
    {
        _inputs.Enable();
        UIManager.OnMenuToggle += HandleMenuToggle;
    }

    private void OnDisable()
    {
        _inputs.Disable();
        UIManager.OnMenuToggle -= HandleMenuToggle;
    }
}