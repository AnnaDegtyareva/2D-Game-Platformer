using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
