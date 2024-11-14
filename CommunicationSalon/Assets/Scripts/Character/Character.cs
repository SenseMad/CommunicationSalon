using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Zenject;

public class Character : SingletonInSceneNoInstance<Character>, IBootstrap
{
  public InputHandler InputHandler { get; private set; }

  public CharacterMovement Movement { get; private set; }
  public CharacterDetection Detection { get; private set; }
  public CharacterCrouch Crouch { get; private set; }

  public Inventory Inventory { get; private set; }

  public Camera MainCamera { get; private set; }

  public bool IsRunning { get; private set; }
  public bool IsCrouch { get; private set; }

  //====================================

  [Inject]
  private void Construct(InputHandler parInputHandler)
  {
    InputHandler = parInputHandler;
  }

  //====================================

  public void CustomAwake()
  {
    MainCamera = Camera.main;

    Movement = GetComponent<CharacterMovement>();
    Detection = GetComponent<CharacterDetection>();
    Crouch = GetComponent<CharacterCrouch>();

    Inventory = GetComponent<Inventory>();
  }

  public void CustomStart() { }

  private void OnEnable()
  {
    InputHandler.IA_Player.Player.Boost.started += OnRun;
    InputHandler.IA_Player.Player.Boost.canceled += OnRun;

    InputHandler.IA_Player.Player.Jump.performed += OnJump;

    InputHandler.IA_Player.Player.Crouch.performed += OnCrouch;

    InputHandler.IA_Player.Player.Interaction.performed += OnDetection;

    InputHandler.IA_Player.Player.Drop.performed += OnDrop;

    InputHandler.IA_Player.InventoryActions.SelectItem.performed += OnSelectItem;
  }

  private void OnDisable()
  {
    InputHandler.IA_Player.Player.Boost.started -= OnRun;
    InputHandler.IA_Player.Player.Boost.canceled -= OnRun;

    InputHandler.IA_Player.Player.Jump.performed -= OnJump;

    InputHandler.IA_Player.Player.Crouch.performed -= OnCrouch;

    InputHandler.IA_Player.Player.Interaction.performed -= OnDetection;

    InputHandler.IA_Player.Player.Drop.performed -= OnDrop;

    InputHandler.IA_Player.InventoryActions.SelectItem.performed -= OnSelectItem;
  }

  //====================================

  private void OnRun(InputAction.CallbackContext context)
  {
    switch (context.phase)
    {
      case InputActionPhase.Started:
        IsRunning = true;
        break;
      case InputActionPhase.Canceled:
        IsRunning = false;
        break;
    }
  }

  private void OnJump(InputAction.CallbackContext context)
  {
    switch (context.phase)
    {
      case InputActionPhase.Performed:
        Movement.Jump();
        break;
    }
  }

  private void OnDrop(InputAction.CallbackContext context)
  {
    switch (context.phase)
    {
      case InputActionPhase.Performed:
        Inventory.DropActiveItem();
        break;
    }
  }

  private void OnDetection(InputAction.CallbackContext context)
  {
    switch (context.phase)
    {
      case InputActionPhase.Performed:
        Detection.Detection();
        break;
    }
  }

  private void OnCrouch(InputAction.CallbackContext context)
  {
    switch (context.phase)
    {
      case InputActionPhase.Performed:
        Crouch.Crouch();
        IsCrouch = !IsCrouch;
        break;
    }
  }

  private void OnSelectItem(InputAction.CallbackContext context)
  {
    int itemIndex = (int)context.control.displayName[0] - '1';

    Inventory.SelectItem(itemIndex);
  }

  //====================================
}