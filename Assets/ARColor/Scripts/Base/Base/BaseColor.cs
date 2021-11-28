using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseColor : MonoBehaviour
{
    #region variable 变量

    public string CardNm = "";

    public GameObject[] ColorParts;
    /// <summary>
    /// Do you want to render the map
    /// 是否要渲染贴图
    /// </summary>
    public bool BLrenderIntoTexture = false;
    /// <summary>
    /// Transparency texture
    /// 透明贴图
    /// </summary>
    public Texture Te_Tran;
    /// <summary>
    /// Do you want to save the texture
    /// 是否要保存贴图
    /// </summary>
    public bool IfSaveColor = false;

    /// <summary>
    /// The width of track image in Unity Engine
    /// 识别图在Unity中的宽度
    /// </summary>
    public float ImageWidth = 1f;
    /// <summary>
    /// The height of track image in Unity Engine
    /// 识别图在Unity中的高度
    /// </summary>
    public float ImageHeight = 1f;

    #region protected variable 继承类可用变量


    protected Texture2D ColorTe;

    //World coordinate of the points on the card
    protected Vector3 TopLeft_Pl_W;
    protected Vector3 BottomLeft_Pl_W;
    protected Vector3 TopRight_Pl_W;
    protected Vector3 BottomRight_Pl_W;
    protected Matrix4x4 VP; 
    protected Vector3 Center_Card=new Vector3();
    protected float Half_W;
    protected float Half_H;
    #endregion

    #endregion

    IEnumerator coFindTime1 = null;
    IEnumerator coFindTime2 = null;
    IEnumerator coFindTime3 = null;
    IEnumerator coFindTime4 = null;
    IEnumerator coFindTime5 = null;

    Rect FindRect;
    Vector4 _UvTopLeft;
    Vector4 _UvButtomLeft;
    Vector4 _UvTopRight;
    Vector4 _UvBottomRight;

    public RectTransform rectT;

    public GameObject UICanvas;

    protected CsMainUI MainUI;

    protected virtual void Start()
    {
        MainUI = UICanvas.GetComponent<CsMainUI>();
        Half_W = 0.5f * ImageWidth;
        Half_H = 0.5f * ImageHeight;

        TopLeft_Pl_W = Center_Card + new Vector3(-Half_W, 0, Half_H);
        BottomLeft_Pl_W = Center_Card + new Vector3(-Half_W, 0, -Half_H);
        TopRight_Pl_W = Center_Card + new Vector3(Half_W, 0, Half_H);
        BottomRight_Pl_W = Center_Card + new Vector3(Half_W, 0, -Half_H);
        ColorTe = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        FindRect = new Rect(0, 0, Screen.width, Screen.height);
        _UvTopLeft = new Vector4(TopLeft_Pl_W.x, TopLeft_Pl_W.y, TopLeft_Pl_W.z, 1f);
        _UvButtomLeft = new Vector4(BottomLeft_Pl_W.x, BottomLeft_Pl_W.y, BottomLeft_Pl_W.z, 1f);
        _UvTopRight = new Vector4(TopRight_Pl_W.x, TopRight_Pl_W.y, TopRight_Pl_W.z, 1f);
        _UvBottomRight = new Vector4(BottomRight_Pl_W.x, BottomRight_Pl_W.y, BottomRight_Pl_W.z, 1f);
    }


    public void ShotAndColor()
    {
        foreach (var item in ColorParts)
        {
            Vector3[] corners = new Vector3[4];
            rectT.GetWorldCorners(corners);
            var startX = corners[0].x;
            var startY = corners[0].y;

            int width = (int)corners[3].x - (int)corners[0].x;
            int height = (int)corners[1].y - (int)corners[0].y;
            FindRect = new Rect(0, 0, Screen.width, Screen.height);
            _UvTopLeft = new Vector4(TopLeft_Pl_W.x, TopLeft_Pl_W.y, TopLeft_Pl_W.z, 1f);
            _UvButtomLeft = new Vector4(BottomLeft_Pl_W.x, BottomLeft_Pl_W.y, BottomLeft_Pl_W.z, 1f);
            _UvTopRight = new Vector4(TopRight_Pl_W.x, TopRight_Pl_W.y, TopRight_Pl_W.z, 1f);
            _UvBottomRight = new Vector4(BottomRight_Pl_W.x, BottomRight_Pl_W.y, BottomRight_Pl_W.z, 1f);
            ColorTe.ReadPixels(FindRect, 0, 0);
            ColorTe.Apply();


            item.GetComponent<Renderer>().material.mainTexture = ColorTe;


            Matrix4x4 P = GL.GetGPUProjectionMatrix(Camera.main.projectionMatrix, BLrenderIntoTexture);
            Matrix4x4 V = Camera.main.worldToCameraMatrix;
            VP = P * V;


            item.GetComponent<Renderer>().material.SetVector("_UvTopLeft", _UvTopLeft);
            item.GetComponent<Renderer>().material.SetVector("_UvButtomLeft", _UvButtomLeft);
            item.GetComponent<Renderer>().material.SetVector("_UvTopRight", _UvTopRight);
            item.GetComponent<Renderer>().material.SetVector("_UvBottomRight", _UvBottomRight);

            item.GetComponent<Renderer>().material.SetMatrix("_VP", VP);
        }
    }

    public IEnumerator ShotAndColor2()
    {

        foreach (var item in ColorParts)
        {
            Half_W = 0.5f * ImageWidth;
            Half_H = 0.5f * ImageHeight;

            TopLeft_Pl_W = Center_Card + new Vector3(-Half_W, 0, Half_H);
            BottomLeft_Pl_W = Center_Card + new Vector3(-Half_W, 0, -Half_H);
            TopRight_Pl_W = Center_Card + new Vector3(Half_W, 0, Half_H);
            BottomRight_Pl_W = Center_Card + new Vector3(Half_W, 0, -Half_H);

            yield return new WaitForEndOfFrame();
            FindRect = new Rect(0, 0, Screen.width, Screen.height);
            _UvTopLeft = new Vector4(TopLeft_Pl_W.x, TopLeft_Pl_W.y, TopLeft_Pl_W.z, 1f);
            _UvButtomLeft = new Vector4(BottomLeft_Pl_W.x, BottomLeft_Pl_W.y, BottomLeft_Pl_W.z, 1f);
            _UvTopRight = new Vector4(TopRight_Pl_W.x, TopRight_Pl_W.y, TopRight_Pl_W.z, 1f);
            _UvBottomRight = new Vector4(BottomRight_Pl_W.x, BottomRight_Pl_W.y, BottomRight_Pl_W.z, 1f);
            ColorTe.ReadPixels(FindRect, 0, 0);
            ColorTe.Apply();


            item.GetComponent<Renderer>().material.mainTexture = ColorTe;


            if(!MainUI.Arcamera.enabled)
            {
                yield return null;
            }
            Matrix4x4 P = GL.GetGPUProjectionMatrix(MainUI.Arcamera.projectionMatrix, BLrenderIntoTexture);
            Matrix4x4 V = MainUI.Arcamera.worldToCameraMatrix;
            VP = P * V;


            item.GetComponent<Renderer>().material.SetVector("_UvTopLeft", _UvTopLeft);
            item.GetComponent<Renderer>().material.SetVector("_UvButtomLeft", _UvButtomLeft);
            item.GetComponent<Renderer>().material.SetVector("_UvTopRight", _UvTopRight);
            item.GetComponent<Renderer>().material.SetVector("_UvBottomRight", _UvBottomRight);

            item.GetComponent<Renderer>().material.SetMatrix("_VP", VP);
        }
    }

    //Transfer information to shader
    IEnumerator Get_Position()
    {
        yield return new WaitForEndOfFrame();

        Matrix4x4 P = GL.GetGPUProjectionMatrix(Camera.main.projectionMatrix, BLrenderIntoTexture);
        Matrix4x4 V = Camera.main.worldToCameraMatrix;
        VP = P * V;


        foreach (var item in ColorParts)
        {
            item.GetComponent<Renderer>().material.SetVector("_UvTopLeft", new Vector4(TopLeft_Pl_W.x, TopLeft_Pl_W.y, TopLeft_Pl_W.z, 1f));
            item.GetComponent<Renderer>().material.SetVector("_UvButtomLeft", new Vector4(BottomLeft_Pl_W.x, BottomLeft_Pl_W.y, BottomLeft_Pl_W.z, 1f));
            item.GetComponent<Renderer>().material.SetVector("_UvTopRight", new Vector4(TopRight_Pl_W.x, TopRight_Pl_W.y, TopRight_Pl_W.z, 1f));
            item.GetComponent<Renderer>().material.SetVector("_UvBottomRight", new Vector4(BottomRight_Pl_W.x, BottomRight_Pl_W.y, BottomRight_Pl_W.z, 1f));

            item.GetComponent<Renderer>().material.SetMatrix("_VP", VP);
        }
      
    }

    ////ScreenShot
    IEnumerator ScreenShot()
    {
        yield return new WaitForEndOfFrame();
        //yield return null;  //When public you can use this

        ColorTe = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ColorTe.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ColorTe.Apply();


        foreach (var item in ColorParts)
        {
            item.GetComponent<Renderer>().material.mainTexture = ColorTe;
        }
    }


    IEnumerator Save_Position(string time)
    {
        yield return new WaitForEndOfFrame();
        SaveColorUtil.GetInstance().SaveCardPoints(CardNm, time, TopLeft_Pl_W, BottomLeft_Pl_W, TopRight_Pl_W, BottomRight_Pl_W, VP);
    }


    IEnumerator SaveShot(string time)
    {
        yield return new WaitForEndOfFrame();

        if (ColorTe==null)
        {
            ColorTe = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        }

        SaveColorUtil.GetInstance().SaveTe(ColorTe, time);
    }

    public void RemoveTexture()
    {
        foreach (var item in ColorParts)
        {
            item.GetComponent<Renderer>().material.mainTexture = Te_Tran;
        }
    }


    public void Set_SavedColor(Texture te, Vector3 topLeft_Pl_W, Vector3 bottomLeft_Pl_W, Vector3 topRight_Pl_W, Vector3 bottomRight_Pl_W, Matrix4x4 vP)
    {
        foreach (var item in ColorParts)
        {
            item.GetComponent<Renderer>().material.SetVector("_UvTopLeft", new Vector4(topLeft_Pl_W.x, topLeft_Pl_W.y, topLeft_Pl_W.z, 1f));
            item.GetComponent<Renderer>().material.SetVector("_UvButtomLeft", new Vector4(bottomLeft_Pl_W.x, bottomLeft_Pl_W.y, bottomLeft_Pl_W.z, 1f));
            item.GetComponent<Renderer>().material.SetVector("_UvTopRight", new Vector4(topRight_Pl_W.x, topRight_Pl_W.y, topRight_Pl_W.z, 1f));
            item.GetComponent<Renderer>().material.SetVector("_UvBottomRight", new Vector4(bottomRight_Pl_W.x, bottomRight_Pl_W.y, bottomRight_Pl_W.z, 1f));

            item.GetComponent<Renderer>().material.SetMatrix("_VP", vP);

            item.GetComponent<Renderer>().material.mainTexture = te;
        }
    }

    public void AllStopCoroutine()
    {
        if (coFindTime1 != null)
        {
            StopCoroutine(coFindTime1);
            coFindTime1 = null;
        }
        if (coFindTime2 != null)
        {
            StopCoroutine(coFindTime2);
            coFindTime2 = null;
        }
        if (coFindTime3 != null)
        {
            StopCoroutine(coFindTime3);
            coFindTime3 = null;
        }
        if (coFindTime4 != null)
        {
            StopCoroutine(coFindTime4);
            coFindTime4 = null;
        }
    }
}
