using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    public UnityEvent goal;

    bool goal_1 = false;

    private void Awake() {
        if (gameObject.tag == "Goal1")
            goal_1 = true;
    }

    private void Start() {
        goal.AddListener(GoalHandler);
    }

    void GoalHandler() {
        Debug.Log(gameObject.name + " GOALLLLLLLLLL");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && other.GetComponent<GeneralPlayer>().HoldingFlag) {
            goal.Invoke();
        }
    }
}
