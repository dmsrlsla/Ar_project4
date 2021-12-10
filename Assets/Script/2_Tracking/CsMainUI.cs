using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CsMainUI : MonoBehaviour
{
    [SerializeField]
    Transform m_trTopUI;
    [SerializeField]
    Transform m_trMiddleUI;
    [SerializeField]
    Transform m_trBottomUI;

    Button m_btnTopClose;
    Button m_btnTopReturn;
    Button m_btnTopSoundOn;
    Button m_btnTopSoundOff;

    Transform m_trImgMarkerRed;
    Transform m_trImgMarkerBlue;

    Button m_Btn_Capture;
    Button m_Btn_ProcessOn;
    Button m_Btn_ProcessOff;
    Button m_Btn_Complate;

    Transform m_EndUI;

    Transform m_trProcess;

    AudioSource m_BGMPlayer;

    public Transform EndUI { get { return m_EndUI; } }



    Coroutine coFind;


    // Start is called before the first frame update
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

        m_BGMPlayer = transform.GetComponent<AudioSource>();
    }

    private void Start()
    {
        ColorProcessUI(false); // 처음에 컬러 기능 비활성화
        OnAudioOn();
        OnColoringOff();
    }

    public void FindTargetUI(bool bFind)
    {
        if(bFind)
        {
            coFind = StartCoroutine(FindTargetComplate());
        }
        else
        {
            if (coFind != null)
            {
                StopCoroutine(coFind);
                coFind = null;
            }
            m_trImgMarkerRed.gameObject.SetActive(true);
            m_trImgMarkerBlue.gameObject.SetActive(false);
            OnColoringOff();
        }
    }

    public void ColorProcessUI(bool bFind)
    {
        if (bFind)
        {
            m_Btn_ProcessOn.gameObject.SetActive(false);
            m_Btn_ProcessOff.gameObject.SetActive(true); // 컬러 기능 끄기 버튼 활성화
        }
        else
        {
            m_Btn_ProcessOn.gameObject.SetActive(true); // 컬러 기능 켜기 벼튼 활성화
            m_Btn_ProcessOff.gameObject.SetActive(false);
        }
    }

    IEnumerator FindTargetComplate()
    {
        m_trImgMarkerRed.gameObject.SetActive(false);
        m_trImgMarkerBlue.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        m_trImgMarkerRed.gameObject.SetActive(false);
        m_trImgMarkerBlue.gameObject.SetActive(false);
    }

    public void OnClickCapture()
    {
        ProgramManager.instance.OnClickCapture();
    }

    public void OnEndUIOn()
    {
        m_EndUI.gameObject.SetActive(true);
    }

    public void OnEndUIOff()
    {
        m_EndUI.gameObject.SetActive(false);
    }

    public void OnEndApp()
    {
        Application.Quit();
    }

    public void OnAudioOn()
    {
        m_BGMPlayer.Play();
        m_btnTopSoundOn.gameObject.SetActive(false);
        m_btnTopSoundOff.gameObject.SetActive(true);
    }

    public void OnAudioOff()
    {

        m_BGMPlayer.Stop();
        m_btnTopSoundOn.gameObject.SetActive(true);
        m_btnTopSoundOff.gameObject.SetActive(false);
    }

    public void OnColoringOn()
    {
        if (ProgramManager.instance.TargetModel != null)
        {
            m_Btn_ProcessOn.gameObject.SetActive(false);
            m_Btn_ProcessOff.gameObject.SetActive(true);
            ProgramManager.instance.OnColoringOn();
        }
    }

    public void OnColoringOff()
    {
        if(ProgramManager.instance.TargetModel != null)
        { 
            m_Btn_ProcessOn.gameObject.SetActive(true);
            m_Btn_ProcessOff.gameObject.SetActive(false);
            ProgramManager.instance.OnColoringOff();
        }
    }

    public void OnComplate()
    {
        if (!ProgramManager.instance.IsComplate)
        {
            ProgramManager.instance.OnViewModeOn();
            m_trProcess.gameObject.SetActive(false);
        }
        else
        {
            ProgramManager.instance.OnViewModeOff();
            m_trProcess.gameObject.SetActive(true);
            ColorProcessUI(false);
            FindTargetUI(false);
        }
    }
}
