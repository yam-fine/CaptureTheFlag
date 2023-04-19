using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] Transform initPos;
    bool pickedUp = false;
    BoxCollider col;

    public bool PickedUp { get => pickedUp;}

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Flag = gameObject;
        col = GetComponent<BoxCollider>();
    }

    public void CaptureFlag(Transform flagPos) {
        
        pickedUp = true;
        col.enabled = false;
        transform.parent = flagPos;
        transform.position = flagPos.position;
        transform.rotation = flagPos.rotation;
    }

    public void Goal() {
        pickedUp = false;
        transform.parent = null;
        transform.position = initPos.position;
        transform.rotation = initPos.rotation;
        col.enabled = true;
    }
}
