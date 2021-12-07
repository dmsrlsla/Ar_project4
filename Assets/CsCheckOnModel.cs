using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsCheckOnModel : MonoBehaviour
{
    MeshRenderer mesh;
    public CsMainUI MainUI;
    public ColorCenter Center;

    [SerializeField]
    public Vector3 MovePosition;

    [SerializeField]
    public Vector3 MoveRotation;

    [SerializeField]
    public float Width = 1.0f;

    [SerializeField]
    public float Height = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    public void OnEventTargetOn()
    {
        Center.OnTargetFind();
        MainUI.TargetModel = this;
    }

    public void OnEventTargetOff()
    {
        Center.OnTargetLost();
        MainUI.TargetModel = this;
    }
}
