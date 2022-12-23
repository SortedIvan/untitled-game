using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sparks_direction_script : MonoBehaviour
{

    public Transform _part3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.LookAt(_part3);

    }
}
