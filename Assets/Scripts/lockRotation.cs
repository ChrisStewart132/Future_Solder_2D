using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockRotation : MonoBehaviour
{
    void FixedUpdate()
    {
        // Lock the rotation to zero
        transform.rotation = Quaternion.identity;
    }
}
