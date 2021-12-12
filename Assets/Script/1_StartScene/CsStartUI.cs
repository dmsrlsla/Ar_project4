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
    private Vector2 MouseEndedPos;
    private Vector2 MouseDif;
    private float MouseSwipeSensitivity = 100;

    private int PageNum = 0;

    List<Transform> listTr = new List<Transform>();

    bool firstTouch = false;

    bool TutorialTouch = false;

    /// ����� �� ���Ͽ��� �ݵ�� üũ�� �����Ͻʽÿ�!!!
    /// Ʃ�丮����� ���� ������ �˴ϴ�.
    [SerializeField]
    bool ResetTutoria_Please_Uncheck_For_Build = false;

    //���������� ��ġ
    public void Swipe1()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);


            if (touch.phase == TouchPhase.Began)
            {
                touchBeganPos = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                touchEndedPos = touch.position;
                touchDif = (touchEndedPos - touchBeganPos);

                //��������. ��ġ�� x�̵��Ÿ��� y�̵��Ÿ��� �ΰ������� ũ��
                if (Mathf.Abs(touchDif.y) > swipeSensitivity || Mathf.Abs(touchDif.x) > swipeSensitivity)
                {
                    if (touchDif.x > 0 && Mathf.Abs(touchDif.y) < Mathf.Abs(touchDif.x))
                    {
                        if (PageNum - 1 <=0)
                            return;

                        if (listTr[PageNum - 1] != null)
                        {
                            OnTutorialSlide(listTr[PageNum - 1]);
                        }
                        if (listTr[PageNum - 2] != null)
                        {
                            OnTutorialSlide(listTr[PageNum - 2]);
                        }
                            //listTr[PageNum + 1].DOMoveX(720, 1f);
                            PageNum--;
                            MouseBeganPos = Vector2.zero;
                            MouseEndedPos = Vector2.zero;
                            MouseDif = Vector2.zero;
                    }
                }
                //��ġ.
                else
                {
                    Debug.Log("touch");
                }
            }
        }
    }

    public void Swipe2()
    {
        // �ε� ������ ó��.
        if (Input.GetMouseButtonDown(0))
        {
            MouseBeganPos = Input.mousePosition;
            Debug.Log("MouseBeganPos");
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("MouseEndedPos");
            MouseEndedPos = Input.mousePosition;

            MouseDif = MouseBeganPos - MouseEndedPos;

            //��������. ��ġ�� x�̵��Ÿ��� y�̵��Ÿ��� �ΰ������� ũ��
            if (Mathf.Abs(MouseDif.y) > MouseSwipeSensitivity || Mathf.Abs(MouseDif.x) > MouseSwipeSensitivity)
            {

                if (MouseDif.x > 0 && Mathf.Abs(MouseDif.y) < Mathf.Abs(MouseDif.x))
                {
                    if (PageNum-1 <= 0)
                        return;

                    if (listTr[PageNum - 1] != null && listTr[PageNum - 2] != null)
                    {
                        if (listTr[PageNum - 1] != null)
                        {
                            OnTutorialSlide(listTr[PageNum - 1]);
                        }
                        if (listTr[PageNum - 2] != null)
                        {
                            OnTutorialSlide(listTr[PageNum - 2]);
                        }
                        //listTr[PageNum + 1].DOMoveX(720, 1f);
                        PageNum--;
                        MouseBeganPos = Vector2.zero;
                        MouseEndedPos = Vector2.zero;
                        MouseDif = Vector2.zero;
                    }
                }
            }
        }
    }

    private void OnTutorialSlide(Transform trTutorial)
    {
        Animator animator = trTutorial.GetComponent<Animator>();

        if (animator == null)
            return;

        if(!animator.enabled)
        {
            animator.enabled = true;
        }
        else
        {
            animator.SetBool("Next", true);
        }
    }

    // Start is called before the first frame update
    void Start()
    { 
        listTr.Add(m_trTutorialPage.GetChild(0));
        listTr.Add(m_trTutorialPage.GetChild(1));
        listTr.Add(m_trTutorialPage.GetChild(2));

        PageNum = listTr.Count;

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
            // Ʃ�丮�� �����̵�
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
            // ����ȭ�� �Ѿ�� ���ؼ�
            firstTouch = true;
        }
        else if (TutorialTouch == true) // Ʃ�丮�� Ȱ��ȭ���̶��
        {
            // Ʃ�丮�� �����̵�
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
        Debug.Log("ī�޶� ȭ������ �ѱ�");
        SceneManager.LoadScene("TrackingScenes");
    }
}
