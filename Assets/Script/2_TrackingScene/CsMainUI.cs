using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// UI 세팅입니다.
/// </summary>
public class CsMainUI : MonoBehaviour
{
    /// <summary>
    /// 상단 UI입니다.
    /// </summary>
    [SerializeField]
    Transform m_trTopUI;
    /// <summary>
    /// 중단 UI입니다.
    /// </summary>
    [SerializeField]
    Transform m_trMiddleUI;

    /// <summary>
    /// 하단 UI입니다.
    /// </summary>
    [SerializeField]
    Transform m_trBottomUI;

    /// <summary>
    /// 각종 버튼 UI입니다.
    /// </summary>
    Button m_btnTopClose;
    Button m_btnTopReturn;
    Button m_btnTopSoundOn;
    Button m_btnTopSoundOff;

    Button m_Btn_Capture;
    Button m_Btn_ProcessOn;
    Button m_Btn_ProcessOff;
    Button m_Btn_Complate;

    /// <summary>
    /// 각종 팝업창 트랜스폼입니다.
    /// </summary>
    Transform m_trImgMarkerRed;
    Transform m_trImgMarkerBlue;

    Transform m_EndUI;

    Transform m_trProcess;

    public Transform EndUI { get { return m_EndUI; } }



    Coroutine coFind;

    /// <summary>
    /// 각종 UI의 정보를 찾아 등록합니다.
    /// </summary>
    void Awake()
    {
        m_trTopUI = transform.Find("TopUI");
        m_trMiddleUI = transform.Find("MiddleUI");
        m_trBottomUI = transform.Find("BottomUI");

        m_btnTopClose = m_trTopUI.Find("BtnClose").GetComponent<Button>();
        m_btnTopReturn = m_trTopUI.Find("BtnReturn").GetComponent<Button>();
        m_btnTopSoundOn = m_trTopUI.Find("BtnSoundOn").GetComponent<Button>();
        m_btnTopSoundOff = m_trTopUI.Find("BtnSoundOff").GetComponent<Button>();

        m_trImgMarkerRed = m_trMiddleUI.Find("MarkerRed");
        m_trImgMarkerBlue = m_trMiddleUI.Find("MarkerBlue");

        m_EndUI = m_trMiddleUI.Find("EndUI");

        m_trImgMarkerRed.gameObject.SetActive(true);
        m_trImgMarkerBlue.gameObject.SetActive(false);

        m_Btn_Capture = m_trBottomUI.Find("Btn_Capture").GetComponent<Button>();
        m_trProcess = m_trBottomUI.Find("Process").transform;
        m_Btn_ProcessOn = m_trProcess.Find("Btn_ProcessOn").GetComponent<Button>();
        m_Btn_ProcessOff = m_trProcess.Find("Btn_ProcessOff").GetComponent<Button>();
        m_Btn_Complate = m_trBottomUI.Find("Btn_Complate").GetComponent<Button>();
    }

    /// <summary>
    /// 반드시 실행되어야 할 기능들을 실행시킵니다.
    /// 컬러 기능 off, 음악 켜기.
    /// </summary>
    private void Start()
    {
        ColorProcessUI(false); // 처음에 컬러 기능 비활성화
        OnAudioOn();
        OnColoringOff();
    }

    /// <summary>
    /// 탐색이 성공되었을경우 UI에서 붉은색, 푸른색 검증 화면을 바꿉니다.
    /// </summary>
    /// <param name="bFind"></param>
    public void FindTargetUI(bool bFind)
    {
        if(bFind)
        {
            // 1초간 푸른색 검증 화면을 바꿉니다.
            coFind = StartCoroutine(FindTargetComplate());
        }
        else
        {
            /// 만약 1초정도뜨는 블루화면이 뜬 상태에서 탐색에 실패하여 FindTargetUI가 출력되면, 코루틴을 제거한다.
            if (coFind != null)
            {
                StopCoroutine(coFind);
                coFind = null;
            }
            m_trImgMarkerRed.gameObject.SetActive(true);
            m_trImgMarkerBlue.gameObject.SetActive(false);
            // 기본적으로 색칠 안한상태로 설정
            OnColoringOff();
        }
    }

    // 1초간 푸른색 검증 화면을 바꿉니다.
    IEnumerator FindTargetComplate()
    {
        m_trImgMarkerRed.gameObject.SetActive(false);
        m_trImgMarkerBlue.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        m_trImgMarkerRed.gameObject.SetActive(false);
        m_trImgMarkerBlue.gameObject.SetActive(false);
    }

    public void TargetMakerUIOff()
    {
        m_trImgMarkerRed.gameObject.SetActive(false);
        m_trImgMarkerBlue.gameObject.SetActive(false);
    }


    /// <summary>
    /// 현재 화면을 캡쳐해 갤러리로 저장합니다.
    /// </summary>
    public void OnClickCapture()
    {
        ProgramManager.instance.OnClickCapture();
    }

    /// <summary>
    /// 종료하시겠습니까? 화면 출력.
    /// </summary>
    public void OnEndUIOn()
    {
        m_EndUI.gameObject.SetActive(true);
    }

    public void OnEndUIOff()
    {
        m_EndUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// 종료하시겠습니까 버튼에서 종료를 누를시 앱 종료
    /// </summary>
    public void OnEndApp()
    {
        Application.Quit();
    }

    /// <summary>
    /// 뮤직 On/Off
    /// </summary>
    public void OnAudioOn()
    {
        ProgramManager.instance.OnMusicOn();
        m_btnTopSoundOn.gameObject.SetActive(false);
        m_btnTopSoundOff.gameObject.SetActive(true);
    }

    public void OnAudioOff()
    {
        ProgramManager.instance.OnMusicOff();
        m_btnTopSoundOn.gameObject.SetActive(true);
        m_btnTopSoundOff.gameObject.SetActive(false);
    }

    /// <summary>
    /// 색칠하기 실행. 버튼도 같이 비활성하 한다.
    /// </summary>
    public void OnColoringOn()
    {
        if (ProgramManager.instance.TargetModel != null)
        {
            ColorProcessUI(true);
            ProgramManager.instance.OnColoringOn();
        }
    }

    public void OnColoringOff()
    {
        //if(ProgramManager.instance.TargetModel != null)
        {
            ColorProcessUI(false);
            ProgramManager.instance.OnColoringOff();
        }
    }

    /// <summary>
    /// 뷰모드 실행
    /// </summary>
    public void OnComplate()
    {
        // 뷰모드가 실행되지 않고, 검색된 모델이 있으며, 현재 컬러링 기능이 실행중이라면(색이 입혔다면)
        if (!ProgramManager.instance.IsComplate && ProgramManager.instance.TargetModel != null && ProgramManager.instance.ColorCenters.bStartColoring)
        {

            ProgramManager.instance.OnViewModeOn();
            TargetMakerUIOff();
            m_trProcess.gameObject.SetActive(false);
        }
        else if(ProgramManager.instance.IsComplate) // 아니라면 그냥 리셋.
        {
            FindTargetUI(true);
            ProgramManager.instance.OnViewModeOff();
            m_trProcess.gameObject.SetActive(true);
            ColorProcessUI(false);
            FindTargetUI(false);
        }
    }

    // 색칠하기버튼아이콘 활성화/비활성화
    public void ColorProcessUI(bool bFind)
    {
        if (bFind)
        {
            m_Btn_ProcessOn.gameObject.SetActive(false);
            m_Btn_ProcessOff.gameObject.SetActive(true);
        }
        else
        {
            m_Btn_ProcessOn.gameObject.SetActive(true);
            m_Btn_ProcessOff.gameObject.SetActive(false);
        }
    }

    public void OnBtnReturn()
    {
        SceneManager.LoadScene("StartScene");
    }
}
