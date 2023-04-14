using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    [SerializeField] private UnityEvent OnGoalTrigger;
    private void OnTriggerEnter(Collider other)
    {
        bool isPlayerAllowed = other.GetComponent<GeneralPlayer>().Goal == gameObject;
        if (other.CompareTag("Player") && other.GetComponent<GeneralPlayer>().HoldingFlag && isPlayerAllowed) {
            Debug.Log("SCOOORRE");
            OnGoalTrigger?.Invoke();
        }
    }
}
