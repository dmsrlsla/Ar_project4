using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsHandShake : MonoBehaviour
{
    [SerializeField]
    float CurrentTime = 0.5f;
    float RealTime = 0;

    Vector3 PrePos;
    Quaternion PreQuater;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        RealTime += Time.deltaTime;

        if (RealTime > CurrentTime)
        {
            RealTime = 0;
            PrePos = transform.position;
            PreQuater = transform.rotation;
        }
        else
        {
            transform.position = PrePos;
            transform.rotation = PreQuater;
        }
    }
}
