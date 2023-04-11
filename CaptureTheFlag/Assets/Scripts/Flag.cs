using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] Transform initPos;
    bool pickedUp = false;
    BoxCollider col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    public void CaptureFlag(Transform flagPos) {
        pickedUp = true;
        col.enabled = false;
        transform.parent = flagPos;
    }

    public void Goal() {
        pickedUp = false;
        transform.parent = null;
        transform.position = initPos.position;
        col.enabled = true;
    }
}
