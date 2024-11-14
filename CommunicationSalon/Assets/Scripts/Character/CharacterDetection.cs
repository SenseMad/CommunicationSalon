using UnityEngine;

public class CharacterDetection : MonoBehaviour, IBootstrap
{
  [SerializeField, Min(0)] private float _detectingRange = 2.0f;

  //====================================

  private Character character;

  //====================================

  public void CustomAwake()
  {
    character = Character.Instance;
  }

  public void CustomStart() { }

  //====================================

  public void Detection()
  {
    Vector2 screenCenterPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
    Ray ray = character.MainCamera.ScreenPointToRay(screenCenterPosition);

    if (!Physics.Raycast(ray, out RaycastHit hit, _detectingRange))
      return;

    if (!hit.collider.TryGetComponent(out IInteractable parInteractable))
      return;

    parInteractable.Interact(hit.collider.gameObject);
  }

  //====================================
}