using UnityEngine;
using UnityEngine.InputSystem;

public class ItemRotator : MonoBehaviour
{
  [SerializeField, Min(0)] private float _rotationStepRegular = 45f;
  [SerializeField, Min(0)] private float _rotationStepShift = 90f;
  [SerializeField, Min(0)] private float _rotationSpeed = 100f;

  //------------------------------------

  private IA_Player IA_Player;

  private Item item;

  private bool isRotating = false;
  private bool isShiftPressed = false;
  private float targetRotationAngle;

  //====================================

  private void Awake()
  {
    IA_Player = new IA_Player();

    item = GetComponent<Item>();
  }

  private void OnEnable()
  {
    IA_Player.Enable();

    IA_Player.Player.Rotate.performed += StartRotation;

    IA_Player.Player.Boost.performed += OnShiftPressed;
    IA_Player.Player.Boost.canceled += OnShiftPressed;
  }

  private void OnDisable()
  {
    IA_Player.Disable();

    IA_Player.Player.Rotate.performed -= StartRotation;

    IA_Player.Player.Boost.performed -= OnShiftPressed;
    IA_Player.Player.Boost.canceled -= OnShiftPressed;
  }

  private void Update()
  {
    if (isRotating && item.IsActive)
    {
      float step = _rotationSpeed * Time.deltaTime;
      Quaternion targetRotation = Quaternion.Euler(0, targetRotationAngle, 0);
      transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, step);

      if (Quaternion.Angle(transform.localRotation, targetRotation) < 0.1f)
      {
        transform.localRotation = targetRotation;
        isRotating = false;
      }
    }
  }

  //====================================

  private void StartRotation(InputAction.CallbackContext context)
  {
    if (!item.IsActive)
      return;

    if (isRotating)
      return;

    float rotationAngel = isShiftPressed ? _rotationStepShift : _rotationStepRegular;
    targetRotationAngle = transform.localEulerAngles.y + rotationAngel;

    isRotating = true;
  }

  private void OnShiftPressed(InputAction.CallbackContext context)
  {
    switch (context.phase)
    {
      case InputActionPhase.Performed:
        isShiftPressed = true;
        break;
      case InputActionPhase.Canceled:
        isShiftPressed = false;
        break;
    }
  }

  //====================================
}