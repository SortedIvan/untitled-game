using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPoint : MonoBehaviour
{
    [SerializeField] Transform pointToFollow;

    private void Update()
    {
        this.transform.position = pointToFollow.position;
    }
}
