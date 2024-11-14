using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
  private Rigidbody itemRigidbody;
  private Collider itemCollider;

  //====================================

  public bool IsActive {  get; private set; }

  //====================================

  private void Awake()
  {
    itemRigidbody = GetComponent<Rigidbody>();
    itemCollider = GetComponent<Collider>();
  }

  //====================================

  public void Drop()
  {
    itemRigidbody.isKinematic = false;
    itemCollider.enabled = true;

    IsActive = false;
  }

  public void Interact(GameObject parGameObject)
  {
    if (!parGameObject.TryGetComponent(out Item parItem))
      return;

    if (!Character.Instance.Inventory.Add(parItem))
      return;

    IsActive = true;
    itemRigidbody.isKinematic = true;
    itemCollider.enabled = false;
  }

  //====================================
}