using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Managers.Inventory.AddItem(this.itemName);
        Messenger.Broadcast(GameEvent.REFRESH_REPOSITORY);
        Destroy(this.gameObject);
    }
}
