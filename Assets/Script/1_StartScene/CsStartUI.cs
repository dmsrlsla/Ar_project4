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

    private int PageNum = 0;

    private int PageNum2 = 0;

    List<Transform> listTr = new List<Transform>();

    bool firstTouch = false;

    bool TutorialTouch = false;

    bool bNextSlide = false;

    [SerializeField]
    Canvas Maincanvas;

    /// 빌드시 씬 파일에서 반드시 체크를 해제하십시오!!!
    /// 튜토리얼씬이 무한 리셋이 됩니다.
    [SerializeField]
    bool ResetTutoria_Please_Uncheck_For_Build = false;

    //스와이프와 터치
    void Start()
    {
        listTr.Add(m_trTutorialPage.GetChild(0));
        listTr.Add(m_trTutorialPage.GetChild(1));
        listTr.Add(m_trTutorialPage.GetChild(2));

        PageNum = listTr.Count;

        m_trClosetPage.gameObject.SetActive(false);

        firstTouch = false;
        ///
        /// 빌드시 씬에서 반드시 체크를 해제하십시오!!!
        /// 튜토리얼씬에서 무한 리셋이 됩니다.
        if (ResetTutoria_Please_Uncheck_For_Build == true)
        {
            Debug.LogError("Reset");
            PlayerPrefs.SetInt("CheckFirst", 0);
            PlayerPrefs.Save();
        }
    }
    public void Swipe1()
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
            // 로딩 끝날시 처리.
    }

    public void Swipe2()
    {
        // 로딩 끝날시 처리.
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

    private void OnTutorialSlide(Transform trTutorial)
    {
        //Animator animator = trTutorial.GetComponent<Animator>();

        //if (animator == null)
        //    return;

        //if(!animator.enabled)
        //{
        //    animator.enabled = true;
        //}
        //else
        //{
        //    animator.SetBool("Next", true);
        //}
    }

    // Start is called before the first frame update


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
            // 다음화면 넘어가기 위해서
            firstTouch = true;
        }
        else if (TutorialTouch == true) // 튜토리얼 활성화중이라면
        {
            // 튜토리얼 슬라이드
            Swipe2();
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
            // 다음화면 넘어가기 위해서
            firstTouch = true;
        }
        else if (TutorialTouch == true) // 튜토리얼 활성화중이라면
        {
            // 튜토리얼 슬라이드
            Swipe1();
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
        Debug.Log("카메라 화면으로 넘김");
        SceneManager.LoadScene("TrackingScenes");
    }
}
