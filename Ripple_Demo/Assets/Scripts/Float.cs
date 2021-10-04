using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    public float FloatStrength;
    public float RandomRotationStrength;

    void Start()
    {
    }

    void Update()
    {
        transform.GetComponent<Rigidbody>().AddForce(Vector3.up * FloatStrength);
        transform.Rotate(RandomRotationStrength, RandomRotationStrength, RandomRotationStrength);

        
    }

    private void OnMouseDown()
    {
        RandomRotationStrength = 0;
    }


}
