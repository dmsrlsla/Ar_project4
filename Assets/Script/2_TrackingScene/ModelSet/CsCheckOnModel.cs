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

    /// <summary>
    /// �������ƿ��� Ÿ���� ã���� ���ʿ��� ���α׷��Ŵ����� �ڽ��� ������ ���� �����Դϴ�. 
    /// �̺�Ʈ�� Image Target�� ����Ͽ� ����մϴ�.
    /// ����� �ȵ˴ϴ�.
    /// </summary>
    public void OnEventTargetOn()
    {
        ProgramManager.instance.OnEventTargetOn(this);
    }

    /// <summary>
    /// �������ƿ��� Ÿ���� �ս��ϸ� ���ʿ��� ��Ž���� ����Ǿ��ٰ� �˸��ϴ�.
    /// ���� ����� �ȵ˴ϴ�.
    /// </summary>

    public void OnEventTargetOff()
    {
        ProgramManager.instance.OnEventTargetOff();
    }
}
