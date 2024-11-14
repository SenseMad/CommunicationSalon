using UnityEngine;
using Zenject;

public class CameraBobbing : MonoBehaviour
{
  [SerializeField, Min(0)] private float _bobFrequency = 10.0f;
  [SerializeField, Min(0)] private float _bobAmplitude = 0.07f;
  [SerializeField, Min(0)] private float _bobSpeedThreshold = 0.1f;
  [SerializeField, Min(0)] private float _smoothingSpeed = 5.0f;

  //------------------------------------

  private InputHandler inputHandler;

  private float timer = 0.0f;
  private Vector3 initialPosition;

  private float currentAmplitude = 0.0f;

  //====================================

  [Inject]
  private void Construct(InputHandler parInputHandler)
  {
    inputHandler = parInputHandler;
  }

  //====================================

  private void Start()
  {
    initialPosition = transform.localPosition;
  }

  private void Update()
  {
    Bobbing();
  }

  //====================================

  private void Bobbing()
  {
    float bobOffset = 0;

    if (inputHandler.Move() != Vector2.zero)
    {
      currentAmplitude = Mathf.Lerp(currentAmplitude, _bobAmplitude, Time.deltaTime * _smoothingSpeed);
      timer += Time.deltaTime * _bobFrequency;

      bobOffset = Mathf.Sin(timer) * currentAmplitude;
      transform.localPosition = initialPosition + new Vector3(0, bobOffset, 0);

      return;
    }

    currentAmplitude = Mathf.Lerp(currentAmplitude, 0, Time.deltaTime * _smoothingSpeed);

    bobOffset = Mathf.Sin(timer) * currentAmplitude;
    transform.localPosition = initialPosition + new Vector3(0, bobOffset, 0);
  }

  //====================================
}