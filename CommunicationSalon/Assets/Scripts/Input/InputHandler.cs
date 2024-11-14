using UnityEngine;

public class InputHandler : MonoBehaviour
{
  public IA_Player IA_Player { get; private set; }

  //====================================

  private void Awake()
  {
    IA_Player = new IA_Player();

    SetCursor(false);
  }

  private void OnEnable()
  {
    IA_Player.Enable();
  }

  private void OnDisable()
  {
    IA_Player.Disable();
  }

  //====================================

  public void SetCursor(bool parValue)
  {
    Cursor.visible = parValue;
    Cursor.lockState = parValue ? CursorLockMode.None : CursorLockMode.Locked;
  }

  //====================================

  public Vector2 Move()
  {
    return IA_Player.Player.Move.ReadValue<Vector2>();
  }

  public Vector2 Look()
  {
    return IA_Player.Player.Look.ReadValue<Vector2>();
  }

  //====================================
}