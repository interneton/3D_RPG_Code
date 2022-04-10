using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasForward : MonoBehaviour
{

    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
