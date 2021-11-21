using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsCheckOnModel : MonoBehaviour
{
    MeshRenderer mesh;
    public CsMainUI MainUI;

    [SerializeField]
    public Vector3 MovePosition;

    [SerializeField]
    public Vector3 MoveRotation;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mesh.enabled)
        {
            MainUI.TargetModel = this;
        }
    }
}
