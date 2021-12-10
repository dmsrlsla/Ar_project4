using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsHandShake : MonoBehaviour
{
    // 핸드쉐이크 보정
    // 카메라에 단다.
    // 시간을 늘리면, 흔들림 정도를 줄일수 있다.
    [SerializeField]
    float CurrentTime = 0.5f;
    float RealTime = 0;

    Vector3 PrePos;
    Quaternion PreQuater;
    // Start is called before the first frame update

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
