using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
  [SerializeField] private int _maxNumberItemsInventory = 3;

  [SerializeField] private Transform _container;

  [SerializeField] private List<Item> _items = new List<Item>();

  //====================================

  public Item ActiveItem { get; private set; }
  public int ActiveItemIndex { get; private set; }

  //====================================

  public void SelectItem(int parIndex)
  {
    if (parIndex >= 0 && parIndex < _maxNumberItemsInventory)
    {
      if (_items.Count > ActiveItemIndex)
        _items[ActiveItemIndex].gameObject.SetActive(false);

      ActiveItem = null;

      ActiveItemIndex = parIndex;

      Item item = null;
      if (_items.Count > ActiveItemIndex)
        item = _items[ActiveItemIndex];

      Equip(item);
    }
  }

  //====================================

  public Item Equip(Item parItem)
  {
    if (parItem == null)
      return ActiveItem;

    if (_items.Count == 0)
      return ActiveItem;

    if (!_items.Contains(parItem))
      return ActiveItem;

    if (parItem == ActiveItem)
      return ActiveItem;

    if (ActiveItem != null)
      ActiveItem.gameObject.SetActive(false);

    ActiveItem = parItem;
    ActiveItem.gameObject.SetActive(true);

    return ActiveItem;
  }

  public bool Add(Item parItem)
  {
    if (parItem == null)
      return false;

    if (_items.Count >= _maxNumberItemsInventory)
      return false;

    parItem.transform.SetParent(_container);
    parItem.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    _items.Add(parItem);

    ActiveItemIndex = _items.IndexOf(parItem);
    Equip(parItem);

    return true;
  }

  public void DropActiveItem()
  {
    Drop(ActiveItem);

    ActiveItem = null;
  }

  public void Drop(Item parItem)
  {
    if (parItem == null)
      return;

    parItem.transform.SetParent(null);
    parItem.Drop();

    if (!parItem.gameObject.activeSelf)
      parItem.gameObject.SetActive(true);

    _items.Remove(parItem);

    Debug.Log($"{_items.Count}");
  }

  //====================================
}