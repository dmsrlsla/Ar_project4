using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsMainUI : MonoBehaviour
{
    public Transform m_trTopUI;
    public Transform m_trMiddleUI;
    public Transform m_trBottomUI;

    public Button m_btnTopClose;
    public Button m_btnTopReturn;
    public Button m_btnTopSound;

    public Button m_btnMiddleStart;

    public Transform m_trTutorial;
    public List<Transform> m_listTutorialPage = new List<Transform>();
    public Transform m_trImgMarkerRed;
    public Transform m_trImgMarkerBlue;

    // Start is called before the first frame update
    void Start()
    {
        m_trTopUI = transform.Find("TopUI");
        m_trMiddleUI = transform.Find("MiddleUI");
        m_trBottomUI = transform.Find("BottomUI");

        m_btnTopClose = m_trTopUI.Find("BtnClose").GetComponent<Button>();
        m_btnTopClose = m_trTopUI.Find("BtnReturn").GetComponent<Button>();
        m_btnTopClose = m_trTopUI.Find("BtnSound").GetComponent<Button>();

        m_trImgMarkerRed = m_trMiddleUI.Find("MarkerRed");
        m_trImgMarkerBlue = m_trMiddleUI.Find("MarkerBlue");

        m_trImgMarkerRed.gameObject.SetActive(false);
        m_trImgMarkerBlue.gameObject.SetActive(false);

        m_btnMiddleStart = m_trMiddleUI.Find("BtnStart").GetComponent<Button>();

        m_trTutorial = m_trBottomUI.Find("Tutorial");

        for (int i = 0; i < m_trTutorial.childCount; i++)
        {
            m_listTutorialPage.Add(m_trTutorial.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCameraOn()
    {
        m_btnMiddleStart.gameObject.SetActive(false);
        m_trImgMarkerRed.gameObject.SetActive(true);
        m_btnMiddleStart.gameObject.SetActive(false);
    }

    public void FindTargetUI(bool bFind)
    {
        if(bFind)
        {
            m_trImgMarkerRed.gameObject.SetActive(false);
            m_trImgMarkerBlue.gameObject.SetActive(true);
        }
        else
        {
            m_trImgMarkerRed.gameObject.SetActive(true);
            m_trImgMarkerBlue.gameObject.SetActive(false);
        }
    }

    public void TutorialScene()
    {

    }
}
