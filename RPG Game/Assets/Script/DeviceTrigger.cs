using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceTrigger : MonoBehaviour
{
    public bool requireKey;

    //激活器要激活的目标对象列表
    [SerializeField] private GameObject[] targets;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (requireKey && Managers.Inventory.equippedItem != "key")
        {
            return;
        }
        else if (requireKey)
        {
            if (Managers.Inventory.ConsumeItem("key"))
            {
                requireKey = false;
            }
        }

        foreach (GameObject target in targets)
        {
            target.SendMessage("Activate");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (GameObject target in targets)
        {
            target.SendMessage("Deactivate");
        }
    }
}
