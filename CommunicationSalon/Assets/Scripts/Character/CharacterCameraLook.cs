using UnityEngine;

public class CharacterCameraLook : MonoBehaviour, IBootstrap
{
  [SerializeField, Min(0)] private float _sensitivity = 1.0f;
  [SerializeField] private Vector2 _angleRotation = new Vector2(-80, 80);

  //------------------------------------

  private Character character;

  private Quaternion rotationCharacter;

  private Quaternion rotationCamera;

  //====================================

  public void CustomAwake()
  {
    character = Character.Instance;

    rotationCharacter = character.transform.localRotation;

    rotationCamera = transform.localRotation;
  }

  public void CustomStart() { }

  private void LateUpdate()
  {
    Look();
  }

  //====================================

  private void Look()
  {
    Vector2 frameInput = character.InputHandler.Look();
    frameInput *= _sensitivity;

    Quaternion rotationYaw = Quaternion.Euler(0.0f, frameInput.x, 0.0f);
    Quaternion rotationPitch = Quaternion.Euler(-frameInput.y, 0.0f, 0.0f);

    rotationCamera *= rotationPitch;
    rotationCamera = Clamp(rotationCamera);
    rotationCharacter *= rotationYaw;

    Quaternion localRotation = transform.localRotation;
    localRotation = Quaternion.Slerp(localRotation, rotationCamera, Time.deltaTime * 45.0f);
    localRotation = Clamp(localRotation);

    character.transform.rotation = Quaternion.Slerp(character.transform.rotation, rotationCharacter, Time.deltaTime * 45.0f);

    transform.localRotation = localRotation;
  }

  //====================================

  private Quaternion Clamp(Quaternion rotation)
  {
    rotation.x /= rotation.w;
    rotation.y /= rotation.w;
    rotation.z /= rotation.w;
    rotation.w = 1.0f;

    float pitch = 2.0f * Mathf.Rad2Deg * Mathf.Atan(rotation.x);

    pitch = Mathf.Clamp(pitch, _angleRotation.x, _angleRotation.y);
    rotation.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * pitch);

    return rotation;
  }

  //====================================
}