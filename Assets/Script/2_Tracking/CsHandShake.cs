using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsHandShake : MonoBehaviour
{
    // �ڵ彦��ũ ����
    // ī�޶� �ܴ�.
    // �ð��� �ø���, ��鸲 ������ ���ϼ� �ִ�.
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
