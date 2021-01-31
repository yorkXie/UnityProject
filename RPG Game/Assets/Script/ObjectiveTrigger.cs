using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        PlayerCharacter playerCharacter = other.gameObject.GetComponent<PlayerCharacter>();
        if (playerCharacter != null)
        {
            Managers.Mission.ReachOjective();
        }
    }
}
