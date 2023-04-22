using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;

public class Goal : NetworkBehaviour
{
    [SerializeField] private UnityEvent OnGoalTrigger;
    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Player") && other.GetComponent<GeneralPlayer>().Goal == gameObject) {
            if (IsHost && GameManager.Instance.Flag.GetComponent<Flag>().hostHoldingFlag.Value) {
                Debug.Log("SCOOORRE");
                OnGoalTrigger?.Invoke();
            }
            else if (IsClient && !GameManager.Instance.Flag.GetComponent<Flag>().hostHoldingFlag.Value) {
                Debug.Log("SCOOORRE");
                OnGoalTrigger?.Invoke();
            }
        }*/
    }
}
