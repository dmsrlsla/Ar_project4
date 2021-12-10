using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsCheckOnModel : MonoBehaviour
{
    MeshRenderer mesh;

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
        ProgramManager.instance.OnEventTargetOn(this);
    }

    public void OnEventTargetOff()
    {
        ProgramManager.instance.OnEventTargetOff();
    }
}
