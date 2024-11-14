using UnityEngine;

public class CharacterFootsteps : MonoBehaviour, IBootstrap
{
  [SerializeField] private AudioClip _footstepSound;
  [SerializeField] private float _stepInterval = 0.5f;
  [SerializeField] private float _footstepVolume = 0.5f;

  //------------------------------------

  private Character character;
  private CharacterMovement movement;

  private float stepTimer = 0f;

  //====================================

  public void CustomAwake()
  {
    character = GetComponent<Character>();

    movement = GetComponent<CharacterMovement>();
  }

  public void CustomStart() { }

  private void Update()
  {
    if (_footstepSound == null)
      return;

    if (movement.IsGrounded && character.InputHandler.Move() != Vector2.zero)
    {
      stepTimer += Time.deltaTime;

      if (stepTimer >= _stepInterval)
      {
        PlayFootstepSound();
        stepTimer = 0f;
      }

      return;
    }

    stepTimer = _stepInterval;
  }

  //====================================

  private void PlayFootstepSound()
  {
    if (_footstepSound == null)
      return;

    AudioSource.PlayClipAtPoint(_footstepSound, transform.position, _footstepVolume);
  }

  //====================================
}