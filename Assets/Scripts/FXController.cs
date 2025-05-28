using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXController : MonoBehaviour
{
    public bool triggerEnd = false;
    void Awake()
    {
        triggerEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerEnd) Destroy(gameObject);
    }
}
