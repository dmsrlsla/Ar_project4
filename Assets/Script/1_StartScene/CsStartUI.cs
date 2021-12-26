using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CsStartUI : MonoBehaviour
{
    [SerializeField]
    Transform m_trBackPage;

    [SerializeField]
    Transform m_trLoadingPage;

    [SerializeField]
    Transform m_trTutorialPage;

    [SerializeField]
    Transform m_trClosetPage;

    Transform m_trTutorialMini1;
    Transform m_trTutorialMini2;
    Transform m_trTutorialMini3;

    private Vector2 touchBeganPos;
    private Vector2 touchEndedPos;
    private Vector2 touchDif;
    private float swipeSensitivity;

    private Vector2 MouseBeganPos;
    private Vector2 MouseDrawPos;
    private Vector2 MouseDrawPos2;
    private Vector2 MouseEndedPos;
    private Vector2 MouseDif;
    [SerializeField]
    private float MouseSwipeSensitivity = 200;

    private int PageNum2 = 0;

    List<Transform> listTr = new List<Transform>();

    bool firstTouch = false;

    bool TutorialTouch = false;

    bool bNextSlide = false;

    [SerializeField]
    Canvas Maincanvas;

    /// ����� �� ���Ͽ��� �ݵ�� üũ�� �����Ͻʽÿ�!!!
    /// Ʃ�丮����� ���� ������ �˴ϴ�.
    [SerializeField]
    bool ResetTutoria_Please_Uncheck_For_Build = false;

    [SerializeField]
    bool Tutoria_Touch = false;

    //���������� ��ġ
    void Start()
    {
        listTr.Add(m_trTutorialPage.GetChild(0));
        listTr.Add(m_trTutorialPage.GetChild(1));
        listTr.Add(m_trTutorialPage.GetChild(2));

        m_trClosetPage.gameObject.SetActive(false);

        firstTouch = false;
        ///
        /// ����� ������ �ݵ�� üũ�� �����Ͻʽÿ�!!!
        /// Ʃ�丮������� ���� ������ �˴ϴ�.
        if (ResetTutoria_Please_Uncheck_For_Build == true)
        {
            Debug.LogError("Reset");
            PlayerPrefs.SetInt("CheckFirst", 0);
            PlayerPrefs.Save();
        }


        // ���۽� ��ġ ����(���۽� ��ġ �������� ���� ����)
        m_trTutorialPage.position = new Vector3(0, m_trTutorialPage.position.y, m_trTutorialPage.position.z);
    }
    // �ȵ���̵��� �����̵�
    public void Swipe_Slide_Android()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                MouseBeganPos = touch.position;
                MouseDrawPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                MouseDrawPos2 = touch.position;

                Vector2 Newdif = MouseDrawPos - MouseDrawPos2;

                if (m_trTutorialPage.position.x > 0)
                {
                    m_trTutorialPage.position = new Vector3(0, m_trTutorialPage.position.y, m_trTutorialPage.position.z);
                }
                else if (m_trTutorialPage.position.x < -(m_trTutorialPage.childCount - 1) * Screen.width * Maincanvas.transform.localScale.x)
                {
                    m_trTutorialPage.position = new Vector3(-(m_trTutorialPage.childCount-1) * Screen.width * Maincanvas.transform.localScale.x, m_trTutorialPage.position.y, m_trTutorialPage.position.z);
                }
                else
                {
                    m_trTutorialPage.position = new Vector3((m_trTutorialPage.position.x - (Newdif.x) * Maincanvas.transform.localScale.x), m_trTutorialPage.position.y, m_trTutorialPage.position.z);
                }
                MouseDrawPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {

                MouseEndedPos = touch.position;

                MouseDif = MouseBeganPos - MouseEndedPos;

                if (MouseDif.x > 0)
                {
                    Debug.LogError("MouseDif.x" + MouseDif.x);
                    //if (Mathf.Abs(MouseDif.x) > MouseSwipeSensitivity)
                    {
                        if (Mathf.Abs(MouseDif.x) > MouseSwipeSensitivity)
                        {
                            bNextSlide = true;

                            PageNum2++;
                            if (PageNum2 >= (m_trTutorialPage.childCount - 1))
                            {
                                PageNum2 = (m_trTutorialPage.childCount - 1);
                            }
                        }
                        else
                        {
                            bNextSlide = false;
                        }
                    }

                    bNextSlide = false;
                }
                else if (MouseDif.x < 0)
                {
                    Debug.LogError("MouseDif.x" + MouseDif.x);
                    //if (Mathf.Abs(MouseDif.x) > MouseSwipeSensitivity)
                    {
                        if (Mathf.Abs(MouseDif.x) > MouseSwipeSensitivity)
                        {
                            bNextSlide = true;
                            PageNum2--;

                            if (PageNum2 <= 0)
                            {
                                PageNum2 = 0;
                            }
                        }
                        else
                        {
                            bNextSlide = false;
                        }

                    }
                    bNextSlide = false;
                }
                m_trTutorialPage.DOMoveX((-(PageNum2) * Screen.width) * Maincanvas.transform.localScale.x, 0.5f);

            }

        }
            // �ε� ������ ó��.
    }

    // �����Ϳ� �����̵�
    public void Swipe_Slide_Editor()
    {
        // �ε� ������ ó��.
        if (Input.GetMouseButtonDown(0))
        {
            MouseBeganPos = Input.mousePosition;
            MouseDrawPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            MouseDrawPos2 = Input.mousePosition;

            Vector2 Newdif = MouseDrawPos - MouseDrawPos2;

            if (m_trTutorialPage.position.x > 0)
            {
                m_trTutorialPage.position = new Vector3(0, m_trTutorialPage.position.y, m_trTutorialPage.position.z);
            }
            else if (m_trTutorialPage.position.x < -(m_trTutorialPage.childCount - 1) * Screen.width * Maincanvas.transform.localScale.x)
            {
                m_trTutorialPage.position = new Vector3(-(m_trTutorialPage.childCount - 1) * Screen.width * Maincanvas.transform.localScale.x, m_trTutorialPage.position.y, m_trTutorialPage.position.z);
            }
            else
            {
                m_trTutorialPage.position = new Vector3((m_trTutorialPage.position.x - (Newdif.x) * Maincanvas.transform.localScale.x), m_trTutorialPage.position.y, m_trTutorialPage.position.z);
            }
            MouseDrawPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {

            MouseEndedPos = Input.mousePosition;

            MouseDif = MouseBeganPos - MouseEndedPos;

            if (MouseDif.x > 0)
            {
                //if (Mathf.Abs(MouseDif.x) > MouseSwipeSensitivity)
                {
                    if (Mathf.Abs(MouseDif.x) > MouseSwipeSensitivity)
                    {
                        bNextSlide = true;

                        PageNum2++;
                        if (PageNum2 >= (m_trTutorialPage.childCount - 1))
                        {
                            PageNum2 = (m_trTutorialPage.childCount - 1);
                        }
                    }
                    else
                    {
                        bNextSlide = false;
                    }
                }

                bNextSlide = false;
            }
            else if (MouseDif.x < 0)
            {
                //if (Mathf.Abs(MouseDif.x) > MouseSwipeSensitivity)
                {
                    if (Mathf.Abs(MouseDif.x) > MouseSwipeSensitivity)
                    {
                        bNextSlide = true;
                        PageNum2--;

                        if (PageNum2 <= 0)
                        {
                            PageNum2 = 0;
                        }
                    }
                    else
                    {
                        bNextSlide = false;
                    }

                }
                bNextSlide = false;
            }
            m_trTutorialPage.DOMoveX((-(PageNum2) * Screen.width) * Maincanvas.transform.localScale.x, 0.5f);

        }
    }

    // Start is called before the first frame update
    // �ȵ���̵��� ��ġ
    public void Swipe_Touch_Android()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                MouseBeganPos = touch.position;
                //MouseDrawPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {

                MouseEndedPos = touch.position;

                MouseDif = MouseBeganPos - MouseEndedPos;

                if (MouseEndedPos.x > Screen.width / 2)
                {
                    PageNum2++;
                    if (PageNum2 > listTr.Count - 1)
                    {
                        PageNum2 = listTr.Count - 1;
                    }
                }
                else
                {
                    PageNum2--;
                    if (PageNum2 < 0)
                    {
                        PageNum2 = 0;
                    }
                }
                m_trTutorialPage.DOMoveX((-(PageNum2) * Screen.width) * Maincanvas.transform.localScale.x, 0.5f);

            }

        }
        // �ε� ������ ó��.
    }
    // �ȵ���̵��� �����̵�
    public void Swipe_Touch_Editor()
    {
        // �ε� ������ ó��.
        if (Input.GetMouseButtonDown(0))
        {
            MouseBeganPos = Input.mousePosition;
            //MouseDrawPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {

            MouseEndedPos = Input.mousePosition;

            MouseDif = MouseBeganPos - MouseEndedPos;

            if (MouseEndedPos.x > Screen.width / 2 )
            {
                PageNum2++;
                if(PageNum2 > listTr.Count - 1)
                {
                    PageNum2 = listTr.Count - 1;
                }
            }
            else
            {
                PageNum2--;
                if (PageNum2 < 0)
                {
                    PageNum2 = 0;
                }
            }
            m_trTutorialPage.DOMoveX((-(PageNum2) * Screen.width) * Maincanvas.transform.localScale.x, 0.5f);

        }
    }



    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && PlayerPrefs.GetInt("CheckFirst") == 0)
        {
            PlayerPrefs.SetInt("CheckFirst", 1);
            PlayerPrefs.Save();
            m_trClosetPage.gameObject.SetActive(true);
            StartCoroutine(FadeOut2(m_trLoadingPage.GetComponent<Image>()));
            TutorialTouch = true;
        }
        else if (firstTouch == true && PlayerPrefs.GetInt("CheckFirst") != 0 && TutorialTouch == false)
        {
            StartCoroutine(FadeOut2(m_trLoadingPage.GetComponent<Image>()));
            OnStartBtnClose();
        }
        else if (Input.GetMouseButtonDown(0) && firstTouch == false)
        {
            // ����ȭ�� �Ѿ�� ���ؼ�
            firstTouch = true;
        }
        else if (TutorialTouch == true) // Ʃ�丮�� Ȱ��ȭ���̶��
        {
            if(Tutoria_Touch)
            {
                // Ʃ�丮�� ��ġ
                Swipe_Touch_Editor();
            }
            else
            {
                // Ʃ�丮�� �����̵�
                Swipe_Slide_Editor();
            }
        }
#endif
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0 && PlayerPrefs.GetInt("CheckFirst") == 0)

        {
            PlayerPrefs.SetInt("CheckFirst", 1);
            PlayerPrefs.Save();
            m_trClosetPage.gameObject.SetActive(true);
            StartCoroutine(FadeOut2(m_trLoadingPage.GetComponent<Image>()));
            TutorialTouch = true;
        }
        else if (firstTouch == true && PlayerPrefs.GetInt("CheckFirst") != 0 && TutorialTouch == false)
        {
            StartCoroutine(FadeOut2(m_trLoadingPage.GetComponent<Image>()));
            OnStartBtnClose();
        }
        else if (Input.touchCount > 0 && firstTouch == false)
        {
            // ����ȭ�� �Ѿ�� ���ؼ�
            firstTouch = true;
        }
        else if (TutorialTouch == true) // Ʃ�丮�� Ȱ��ȭ���̶��
        {
            if (Tutoria_Touch)
            {
                // Ʃ�丮�� ��ġ
                Swipe_Touch_Android();
            }
            else
            {
                // Ʃ�丮�� �����̵�
                Swipe_Slide_Android();
            }
        }
#endif
    }

    IEnumerator FadeOut(Image co)
    {
        float fadeCount = 1;
        while (fadeCount > 0.0)
        {
            fadeCount -= 0.015f;
            yield return null;
        }
        yield return new WaitForSeconds(0.01f);
        co.color = new Color(0, 0, 0, fadeCount);
    }

    IEnumerator FadeOut2(Image co)
    {
        float fadeCount = 1;
        fadeCount = 1;
        while (fadeCount > 0.0f)
        {
            fadeCount -= 0.015f;
            yield return new WaitForSeconds(0.006f);
            co.color = new Color(255, 255, 255, fadeCount);
        }

        firstTouch = true;
    }

    public void OnStartBtnClose()
    {
        Debug.Log("ī�޶� ȭ������ �ѱ�");
        SceneManager.LoadScene("TrackingScenes");
    }
}
