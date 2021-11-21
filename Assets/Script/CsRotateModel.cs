using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsRotateModel : MonoBehaviour
{
    [SerializeField]
    float RotateSpeed = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, RotateSpeed, 0) * Time.deltaTime);
    }
}
