using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    Camera mycam;

    private void Start()
    {
        mycam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);

    }


    public void MiniMapButton(int index)
    {
        if(index == 1 && mycam.fieldOfView < 120 )
        {
            mycam.fieldOfView += 30;
        }
        else if (index == 0 && mycam.fieldOfView > 30)
        {
            mycam.fieldOfView -= 30;
        }


    }
}
