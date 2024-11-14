using UnityEngine;

public class CharacterCrouch : MonoBehaviour, IBootstrap
{
  [SerializeField] private Transform _cameraTransform;
  [SerializeField, Min(0)] private float _crouchHeight = 1.2f;
  [SerializeField, Min(0)] private float _standHeight = 2f;
  [SerializeField, Min(0)] private float _crouchSpeed = 8f;

  //------------------------------------

  private Character character;

  private Vector3 originalCameraPosition;
  private float targetHeight;
  private Vector3 targetCameraPosition;

  //====================================

  public void CustomAwake()
  {
    character = Character.Instance;
  }

  public void CustomStart()
  {
    originalCameraPosition = _cameraTransform.localPosition;

    targetHeight = _standHeight;

    targetCameraPosition = originalCameraPosition;
  }

  private void Update()
  {
    var characterController = character.Movement.Controller;

    characterController.height = Mathf.MoveTowards(characterController.height, targetHeight, _crouchSpeed * Time.deltaTime);
  }

  //====================================

  public void Crouch()
  {
    if (character.IsCrouch)
    {
      targetHeight = _standHeight;
      targetCameraPosition = originalCameraPosition;
    }
    else
    {
      targetHeight = _crouchHeight;
      targetCameraPosition = originalCameraPosition + new Vector3(0, -(_standHeight - _crouchHeight) / 2, 0);
    }

    _cameraTransform.localPosition = targetCameraPosition;
  }

  //====================================
}