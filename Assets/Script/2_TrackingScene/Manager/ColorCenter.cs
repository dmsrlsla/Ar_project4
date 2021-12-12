using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

/// <summary>
/// 컬러를 처리합니다. 실질적 처리는 BaseColor에서 합니다.
/// </summary>
public class ColorCenter : BaseColor
{

    public GameObject Atlas;

    public Transform ImList = null;


    public Dictionary<string, Texture2D> Dic_DateAndTe;

    public Dictionary<string, ColorData> Dic_DateAndData;

    private bool ifCanColor = true;

    public bool bStartColoring;

    Coroutine coFindTime = null;

    PlaneManager plane;

    protected override void Start()
    {
        base.Start();

        plane = new PlaneManager();
    }

    #region 사용하지 않는 기존 앱 스크립트
    public void Btn_Color()
    {
        if (ifCanColor)
        {
            ShotAndColor();
            ifCanColor = false;
            if (coFindTime == null)
            {
                coFindTime = StartCoroutine(delayColor());
            }

        }
    }

    IEnumerator delayColor()
    {
        yield return new WaitForSeconds(0.1f);
        ifCanColor = true;
        coFindTime = null;
        AllStopCoroutine();
    }


    public void Btn_Clean()
    {
        RemoveTexture();
    }


    public void Btn_AtlasClose()
    {
        Atlas.SetActive(false);
    }


    public void LoadSavedColorDatas()
    {

    }
    #endregion
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>



    public void Btn_DeleteAll()
    {
        SaveColorUtil.GetInstance().DeleteAllColorInfo();
    }


    /// <summary>
    /// 색칠모드가 실행되면, 카메라에서 텍스쳐를 구성해서 컬러를 입힙니다. 
    /// 주기는 0.3초로 해놨는데 더 빠르게 해도 상관은 없지만 부하가 심합니다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator DelayInfinity()
    {
        while(bStartColoring)
        {
            StartCoroutine(ShotAndColor2());
            yield return new WaitForSeconds(0.3f);
        }
    }

    /// <summary>
    /// 타겟을 찾으면, 새롭게 메테리얼을 구성합니다.
    /// </summary>
    public void OnTargetFind()
    {
        if (!ProgramManager.instance.IsComplate)
        {
            SetNewMaterial(); // 베이스 컬러에서 메테리얼을 새로이 구성합니다.
            ProgramManager.instance.MainUI.FindTargetUI(true);
        }
    }

    // 타겟을 잃어버렸다면, 끕니다.
    public void OnTargetLost()
    {
        ResetMaterial();// 택스쳐를 리셋합니다.
        if (ProgramManager.instance.Arcamera.enabled)
        {
            ProgramManager.instance.MainUI.FindTargetUI(false);
        }
    }

    /// <summary>
    /// 컬러링을 하기로 했으면, DelayInfinity를 실행해 컬러링을 진행합니다.
    /// </summary>
    public void OnColoringOn()
    {
        bStartColoring = true;
        SetNewMaterial();
        StartCoroutine(DelayInfinity());
    }

    /// <summary>
    /// 컬러링을 끄고 투명상태로 바꿉니다.
    /// </summary>
    public void OnColoringOff()
    {
        bStartColoring = false;
        StopColoring(); // 이부분이 별도로 쓰일 가능성이 있어 분리했습니다.
    }

    /// <summary>
    /// 모델을 리셋하는 함수지만 실질적으로 사용안함
    /// </summary>
    public void ResetModel()
    {
        TrackerManager.Instance.GetTracker<PositionalDeviceTracker>().Reset();
        RemoveTexture();
    }

    /// <summary>
    /// 컬러링에 관련된 모든 기능을 끄고 리셋시킵니다.
    /// </summary>
    public void StopColoring()
    {
        bStartColoring = false;
        StopCoroutine(ShotAndColor2());
        StopCoroutine(DelayInfinity());
        ResetMaterial();
        TrackerManager.Instance.GetTracker<PositionalDeviceTracker>().Reset();
        RemoveTexture();
    }
}
