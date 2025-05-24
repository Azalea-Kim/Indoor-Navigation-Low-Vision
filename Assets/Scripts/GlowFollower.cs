using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowFollower : MonoBehaviour
{
    public Transform target; 

    void Update()
    {
        if (target != null)
            transform.position = target.position;
    }
}
