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

    //스와이프와 터치
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

                //스와이프. 터치의 x이동거리나 y이동거리가 민감도보다 크면
                if (Mathf.Abs(touchDif.y) > swipeSensitivity || Mathf.Abs(touchDif.x) > swipeSensitivity)
                {
                    if (touchDif.y > 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                    {
                        Debug.Log("up");
                    }
                    else if (touchDif.y < 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                    {
                        Debug.Log("down");
                    }
                    else if (touchDif.x > 0 && Mathf.Abs(touchDif.y) < Mathf.Abs(touchDif.x))
                    {
                        m_trTutorialMini2.DOMoveX(-1440, 1f);
                    }
                    else if (touchDif.x < 0 && Mathf.Abs(touchDif.y) < Mathf.Abs(touchDif.x))
                    {
                        m_trTutorialMini2.DOMoveX(1440, 1f);
                    }
                }
                //터치.
                else
                {
                    Debug.Log("touch");
                }
            }
        }
    }

    public void Swipe2()
    {
        // 로딩 끝날시 처리.
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

            //스와이프. 터치의 x이동거리나 y이동거리가 민감도보다 크면
            if (Mathf.Abs(MouseDif.y) > MouseSwipeSensitivity || Mathf.Abs(MouseDif.x) > MouseSwipeSensitivity)
            {

                if (MouseDif.x > 0 && Mathf.Abs(MouseDif.y) < Mathf.Abs(MouseDif.x))
                {
                    if (PageNum+1 > listTr.Count)
                        return;

                    listTr[PageNum].DOMoveX(-720, 1f);
                    listTr[PageNum + 1].DOMoveX(720, 1f);
                    PageNum++;
                    MouseBeganPos = Vector2.zero;
                    MouseEndedPos = Vector2.zero;
                    MouseDif = Vector2.zero;
                }
            }
        }
    }

    ////스와이프
    //public void Swipe2()
    //{
    //    if (Input.touches.Length > 0)
    //    {
    //        Touch t = Input.GetTouch(0);
    //        if (t.phase == TouchPhase.Began)
    //        {
    //            //save began touch 2d point
    //            firstPressPos = new Vector2(t.position.x, t.position.y);
    //        }
    //        if (t.phase == TouchPhase.Ended)
    //        {
    //            //save ended touch 2d point
    //            secondPressPos = new Vector2(t.position.x, t.position.y);

    //            //create vector from the two points
    //            currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

    //            //normalize the 2d vector
    //            currentSwipe.Normalize();

    //            //swipe upwards
    //            if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
    //            {
    //                Debug.Log("up swipe");
    //            }
    //            //swipe down
    //            if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
    //            {
    //                Debug.Log("down swipe");
    //            }
    //            //swipe left
    //            if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
    //            {
    //                Debug.Log("left swipe");
    //            }
    //            //swipe right
    //            if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
    //            {
    //                Debug.Log("right swipe");
    //            }
    //        }
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        //m_trTutorialMini1 = m_trTutorialPage.GetChild(0);
        //m_trTutorialMini2 = m_trTutorialPage.GetChild(1);
        //m_trTutorialMini3 = m_trTutorialPage.GetChild(2);

        listTr.Add(m_trTutorialPage.GetChild(0));
        listTr.Add(m_trTutorialPage.GetChild(1));
        listTr.Add(m_trTutorialPage.GetChild(2));

        StartCoroutine(FadeOut(m_trBackPage.GetComponent<Image>()));
    }

    // Update is called once per frame
    void Update()
    {
        //Swipe1();
        if (Input.GetMouseButtonDown(0) && !firstTouch)
        {
            StartCoroutine(FadeOut2(m_trLoadingPage.GetComponent<Image>()));
        }
        else
        {
            Swipe2();
        }
    }

    IEnumerator FadeOut(Image co)
    {
        float fadeCount = 1;
        while (fadeCount > 0.0)
        {
            fadeCount -= 0.005f;
            yield return new WaitForSeconds(0.01f);
            co.color = new Color(0, 0, 0, fadeCount);
        }
    }

    IEnumerator FadeOut2(Image co)
    {
        float fadeCount = 1;
        fadeCount = 1;
        while (fadeCount > 0.0f)
        {
            fadeCount -= 0.005f;
            yield return new WaitForSeconds(0.006f);
            co.color = new Color(255, 255, 255, fadeCount);
        }
        firstTouch = true;
    }

    public void OnStartBtnClose()
    {
        Debug.LogError("페이지 넘김");
        SceneManager.LoadScene("ColorScene");
    }
}
