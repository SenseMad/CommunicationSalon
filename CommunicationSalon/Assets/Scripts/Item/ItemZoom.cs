using UnityEngine;
using UnityEngine.InputSystem;

public class ItemZoom : MonoBehaviour
{
  [SerializeField, Min(0)] private float _zoomSpeed = 5f;
  [SerializeField, Min(0)] private float _zoomDistance = 1f;
  [SerializeField, Min(0)] private float _returnSpeed = 3f;

  //------------------------------------

  private IA_Player IA_Player;
  private Character character;

  private Item item;

  private bool isZooming = false;
  private Vector3 originalLocalPosition;
  private Vector3 targetLocalPosition;

  //====================================

  private void Awake()
  {
    IA_Player = new IA_Player();

    character = Character.Instance;

    item = GetComponent<Item>();
  }

  private void Start()
  {
    originalLocalPosition = Vector3.zero;
  }

  private void OnEnable()
  {
    IA_Player.Enable();

    IA_Player.Player.Zoom.started += StartZoom;
    IA_Player.Player.Zoom.canceled += StartZoom;
  }

  private void OnDisable()
  {
    IA_Player.Disable();

    IA_Player.Player.Zoom.started -= StartZoom;
    IA_Player.Player.Zoom.canceled -= StartZoom;
  }

  private void Update()
  {
    if (!item.IsActive)
      return;

    if (isZooming)
      transform.localPosition = Vector3.Lerp(transform.localPosition, originalLocalPosition + new Vector3(0, 0, -_zoomDistance), _zoomSpeed * Time.deltaTime);
    else
      transform.localPosition = Vector3.Lerp(transform.localPosition, originalLocalPosition, _returnSpeed * Time.deltaTime);
  }

  //====================================

  private void StartZoom(InputAction.CallbackContext context)
  {
    switch (context.phase)
    {
      case InputActionPhase.Started:
        isZooming = true;
        break;
      case InputActionPhase.Canceled:
        isZooming = false;
        break;
    }
  }

  //====================================
}