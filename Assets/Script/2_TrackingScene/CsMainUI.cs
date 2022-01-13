using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// UI �����Դϴ�.
/// </summary>
public class CsMainUI : MonoBehaviour
{
    /// <summary>
    /// ��� UI�Դϴ�.
    /// </summary>
    [SerializeField]
    Transform m_trTopUI;
    /// <summary>
    /// �ߴ� UI�Դϴ�.
    /// </summary>
    [SerializeField]
    Transform m_trMiddleUI;

    /// <summary>
    /// �ϴ� UI�Դϴ�.
    /// </summary>
    [SerializeField]
    Transform m_trBottomUI;

    /// <summary>
    /// ���� ��ư UI�Դϴ�.
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
    /// ���� �˾�â Ʈ�������Դϴ�.
    /// </summary>
    Transform m_trImgMarkerRed;
    Transform m_trImgMarkerBlue;

    Transform m_EndUI;

    Transform m_trProcess;

    public Transform EndUI { get { return m_EndUI; } }



    Coroutine coFind;

    /// <summary>
    /// ���� UI�� ������ ã�� ����մϴ�.
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
    /// �ݵ�� ����Ǿ�� �� ��ɵ��� �����ŵ�ϴ�.
    /// �÷� ��� off, ���� �ѱ�.
    /// </summary>
    private void Start()
    {
        ColorProcessUI(false); // ó���� �÷� ��� ��Ȱ��ȭ
        OnAudioOn();
        OnColoringOff();
    }

    /// <summary>
    /// Ž���� �����Ǿ������ UI���� ������, Ǫ���� ���� ȭ���� �ٲߴϴ�.
    /// </summary>
    /// <param name="bFind"></param>
    public void FindTargetUI(bool bFind)
    {
        if(bFind)
        {
            // 1�ʰ� Ǫ���� ���� ȭ���� �ٲߴϴ�.
            coFind = StartCoroutine(FindTargetComplate());
        }
        else
        {
            /// ���� 1�������ߴ� ���ȭ���� �� ���¿��� Ž���� �����Ͽ� FindTargetUI�� ��µǸ�, �ڷ�ƾ�� �����Ѵ�.
            if (coFind != null)
            {
                StopCoroutine(coFind);
                coFind = null;
            }
            m_trImgMarkerRed.gameObject.SetActive(true);
            m_trImgMarkerBlue.gameObject.SetActive(false);
            // �⺻������ ��ĥ ���ѻ��·� ����
            OnColoringOff();
        }
    }

    // 1�ʰ� Ǫ���� ���� ȭ���� �ٲߴϴ�.
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
    /// ���� ȭ���� ĸ���� �������� �����մϴ�.
    /// </summary>
    public void OnClickCapture()
    {
        ProgramManager.instance.OnClickCapture();
    }

    /// <summary>
    /// �����Ͻðڽ��ϱ�? ȭ�� ���.
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
    /// �����Ͻðڽ��ϱ� ��ư���� ���Ḧ ������ �� ����
    /// </summary>
    public void OnEndApp()
    {
        Application.Quit();
    }

    /// <summary>
    /// ���� On/Off
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
    /// ��ĥ�ϱ� ����. ��ư�� ���� ��Ȱ���� �Ѵ�.
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
    /// ���� ����
    /// </summary>
    public void OnComplate()
    {
        // ���尡 ������� �ʰ�, �˻��� ���� ������, ���� �÷��� ����� �������̶��(���� �����ٸ�)
        if (!ProgramManager.instance.IsComplate && ProgramManager.instance.TargetModel != null && ProgramManager.instance.ColorCenters.bStartColoring)
        {

            ProgramManager.instance.OnViewModeOn();
            TargetMakerUIOff();
            m_trProcess.gameObject.SetActive(false);
        }
        else if(ProgramManager.instance.IsComplate) // �ƴ϶�� �׳� ����.
        {
            FindTargetUI(true);
            ProgramManager.instance.OnViewModeOff();
            m_trProcess.gameObject.SetActive(true);
            ColorProcessUI(false);
            FindTargetUI(false);
        }
    }

    // ��ĥ�ϱ��ư������ Ȱ��ȭ/��Ȱ��ȭ
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
