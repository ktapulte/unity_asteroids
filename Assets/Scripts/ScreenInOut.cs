using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenInOut : MonoBehaviour
{
    // exit window
    private void OutsideWindow()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPos.x < 0) screenPos.x = Screen.width - 1;
        if (screenPos.x > Screen.width) screenPos.x = 1;
        if (screenPos.y < 0) screenPos.y = Screen.height - 1;
        if (screenPos.y > Screen.height) screenPos.y = 1;
        transform.position = Camera.main.ScreenToWorldPoint(screenPos);
    }

    // Update is called once per frame
    void Update()
    {
        OutsideWindow();
    }
}
