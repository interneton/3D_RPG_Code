using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableUI : MonoBehaviour
{
    public float DisableTimer = 0.5f;
    private void OnEnable()
    {
        CancelInvoke();
        Invoke("Disable", DisableTimer);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}
