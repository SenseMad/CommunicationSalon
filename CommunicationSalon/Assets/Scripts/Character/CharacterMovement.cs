using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour, IBootstrap
{
  [Header("Speed")]
  [SerializeField] private float _speedWalking = 7.0f;
  [SerializeField] private float _speedRunning = 12.0f;

  [Header("Boost")]
  [SerializeField] private float _acceleration = 9.0f;
  [SerializeField] private float _deceleration = 11.0f;

  [Header("Gravity")]
  [SerializeField] private float _gravity = 35.0f;
  [SerializeField] private float _jumpForce = 1.5f;

  //------------------------------------

  private Character character;

  private Vector3 velocity;

  private bool wasGrounded;

  private bool isJumping;
  private float lastJumpTime;

  //====================================

  public CharacterController Controller { get; private set; }

  public bool IsGrounded { get; private set; }

  //====================================

  public void CustomAwake()
  {
    character = GetComponent<Character>();

    Controller = GetComponent<CharacterController>();
  }

  public void CustomStart() { }

  private void Update()
  {
    IsGrounded = Controller.isGrounded;

    if (IsGrounded && !wasGrounded)
    {
      isJumping = false;
      //character.SetJump(false);
      lastJumpTime = 0.0f;
    }
    else if (wasGrounded && !IsGrounded)
      lastJumpTime = Time.time;

    Move();

    wasGrounded = IsGrounded;
  }

  //====================================

  public void Jump()
  {
    if (!IsGrounded)
      return;

    isJumping = true;
    velocity = new Vector3(velocity.x, Mathf.Sqrt(2.0f * _jumpForce * _gravity), velocity.z);

    lastJumpTime = Time.time;
  }

  //====================================

  private void Move()
  {
    Vector2 frameInput = Vector3.ClampMagnitude(character.InputHandler.Move(), 1.0f);
    var desiredDirection = new Vector3(frameInput.x, 0.0f, frameInput.y);

    if (!Controller.enabled)
      desiredDirection = Vector3.zero;

    desiredDirection *= !character.IsRunning ? (character.IsCrouch ? _speedWalking / 2.0f : _speedWalking) : (character.IsCrouch ? _speedWalking / 2.0f : _speedRunning);

    desiredDirection = transform.TransformDirection(desiredDirection);

    velocity = Vector3.Lerp(velocity, new Vector3(desiredDirection.x, velocity.y, desiredDirection.z), Time.deltaTime * (desiredDirection.sqrMagnitude > 0.0f ? _acceleration : _deceleration));

    if (!IsGrounded)
    {
      if (wasGrounded && !isJumping)
        velocity.y = 0.0f;

      velocity.y -= _gravity * Time.deltaTime;
    }

    Vector3 applied = velocity * Time.deltaTime;

    if (Controller.isGrounded && !isJumping)
      applied.y -= 0.03f;

    Controller.Move(applied);
  }

  //====================================
}