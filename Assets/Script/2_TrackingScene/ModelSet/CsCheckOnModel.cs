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
    /// 뷰포리아에서 타겟을 찾으면 모델쪽에서 프로그램매니져로 자신의 정보를 보낼 구문입니다. 
    /// 이벤트를 Image Target에 등록하여 사용합니다.
    /// 지우면 안됩니다.
    /// </summary>
    public void OnEventTargetOn()
    {
        ProgramManager.instance.OnEventTargetOn(this);
    }

    /// <summary>
    /// 뷰포리아에서 타겟을 손실하면 모델쪽에서 모델탐색이 종료되었다고 알립니다.
    /// 역시 지우면 안됩니다.
    /// </summary>

    public void OnEventTargetOff()
    {
        ProgramManager.instance.OnEventTargetOff();
    }
}
