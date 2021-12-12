using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using System.IO;
using Vuforia;
using DG.Tweening;

/// <summary>
/// UI�� �÷� ó���θ� ������ �Ŵ����Դϴ�.(�̱��� ����)
/// </summary>
public class ProgramManager : MonoBehaviour
{
    public static ProgramManager instance = null;

    //[SerializeField]
    //List<DefaultTrackableEventHandler> ListEventHandler = new List<DefaultTrackableEventHandler>();

    /// <summary>
    /// �÷� ó���� �Դϴ�.
    /// </summary>
    [SerializeField]
    public ColorCenter ColorCenters;

    /// <summary>
    /// ���� UI�Դϴ�.
    /// </summary>
    [SerializeField]
    public CsMainUI MainUI;

    /// <summary>
    /// AR ī�޶� �Դϴ�.
    /// </summary>
    [SerializeField]
    public Camera Arcamera;

    /// <summary>
    /// ���忡�� ����� Ÿ�� ī�޶� �Դϴ�.
    /// </summary>
    [SerializeField]
    Camera TargetModelcamera;

    /// <summary>
    /// ĸ�� ���� �Ǻ� �����Դϴ�.
    /// </summary>
    private bool onCapture = false;

    /// <summary>
    /// ���� ī�޶� ��ġ�� Ÿ�� �������Դϴ�. �� ������ �𵨸��� �޸� CsCheckModel���� ���޵˴ϴ�.
    /// </summary>
    private CsCheckOnModel _TargetModel;

    public CsCheckOnModel TargetModel { get { return _TargetModel; } set { _TargetModel = value; } }

    /// <summary>
    /// ���忡�� ȭ�鿡 ���߾��� Ŭ�� ���� ���鶧 �ʿ��� �������Դϴ�.
    /// </summary>
    private CsCheckOnModel m_CheckModel;
    private Texture2D m_texIns;

    /// <summary>
    /// ���� ���� ��ư�� ���ȴ��� üũ�մϴ�.
    /// </summary>
    public bool IsComplate { get; set; }

    AudioSource m_BGMPlayer;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }

        ColorCenters = GameObject.Find("ColorProcess").GetComponent<ColorCenter>();
        IsComplate = false;

        m_BGMPlayer = transform.GetComponent<AudioSource>();
    }

    /// <summary>
    /// ī�޶� ĸ�� �����Դϴ�.
    /// �������� ���� ĸ��ȭ���� �����մϴ�. MainUIȣ��
    /// </summary>


    public void OnClickCapture()
    {
        if (onCapture == false)
        {
            StartCoroutine(CRSaveScreenshot());
        }
    }

    /// <summary>
    /// �𵨿��� ������ �Ǹ�, Ÿ���� ã�� �ش� Ÿ�� ���� �����
    /// </summary>
    /// <param name="_NewTargetModel"></param>
    public void OnEventTargetOn(CsCheckOnModel _NewTargetModel)
    {
        ColorCenters.OnTargetFind();
        _TargetModel = _NewTargetModel;
    }

    /// <summary>
    ///  ��ĥ ��带 �����ϰ� Ÿ���� ������.
    /// </summary>
    public void OnEventTargetOff()
    {
        ColorCenters.OnTargetLost();
        _TargetModel = null;
    }

    /// <summary>
    /// �������⸦ �����մϴ�. UI��ư�� ������, ĸ���� ȭ�� ������ �ְ� �÷����Ϳ� �÷��� ����� On�մϴ�.
    /// </summary>
    public void OnColoringOn()
    {
        ColorCenters.ImageWidth = TargetModel.Width;
        ColorCenters.ImageHeight = TargetModel.Height;
        ColorCenters.OnColoringOn();
    }

    /// <summary>
    /// �������� ����� �����մϴ�.
    /// </summary>
    public void OnColoringOff()
    {
        ColorCenters.OnColoringOff();
    }

    /// <summary>
    /// ������ Ʋ�� ���ϴ�.
    /// </summary>
    public void OnMusicOn()
    {
        m_BGMPlayer.Play();
    }

    public void OnMusicOff()
    {
        m_BGMPlayer.Stop();
    }

    /// <summary>
    /// ���� �Դϴ�. ARī�޶� ����, ��ĥ�� ���� ȭ�� �߾ӿ� ��ġ
    /// </summary>
    public void OnViewModeOn()
    {
        // AR ī�޶� ���� �����մϴ�.
        Arcamera.enabled = false;
        //foreach (DefaultTrackableEventHandler itb in ListEventHandler)
        //{
        //    itb.gameObject.SetActive(false);
        //}

        // �� ī�޶� �մϴ�.
        TargetModelcamera.gameObject.SetActive(true);


        // �� �𵨿� �� ���� ��� �ؽ��ĸ� ���� ����ϴ�.
        m_CheckModel = Instantiate<CsCheckOnModel>(TargetModel);
        m_CheckModel.GetComponent<MeshRenderer>().enabled = true;

        TargetModel.GetComponent<MeshRenderer>().enabled = false;

        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        tex = m_CheckModel.GetComponent<MeshRenderer>().materials[0].GetTexture("_MainTex") as Texture2D;
        m_texIns = Instantiate<Texture2D>(tex);

        if (tex == null && m_texIns == null)
        {
            Debug.LogError("��������");
        }

        // ���� ���̸��� ��ġ�մϴ�.
        m_CheckModel.transform.DOMove(TargetModel.MovePosition, 1.0f);
        m_CheckModel.transform.DORotate(TargetModel.MoveRotation, 1.0f);
        m_CheckModel.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", m_texIns);

        // �ش� ����� �����ߴٰ� üũ�մϴ�
        IsComplate = true;

        // ��ĥ����� ���忡�� �����մϴ�.
        ColorCenters.OnColoringOff();
    }
    /// <summary>
    /// �� ��带 �����ϰ� �ٽ� ARī�޶�� ���ƿ�
    /// </summary>
    public void OnViewModeOff()
    {
        Arcamera.enabled = true;
        //foreach (DefaultTrackableEventHandler itb in ListEventHandler)
        //{
        //    itb.gameObject.SetActive(true);
        //}
        // �� ī�޶� ���ϴ�.
        TargetModelcamera.gameObject.SetActive(false);

        // Ÿ�ٸ��� �ִٸ�, �����մϴ�.
        if (_TargetModel != null)
        {
            _TargetModel.GetComponent<MeshRenderer>().enabled = false;
            _TargetModel = null;
        }

        IsComplate = false;
        // �÷��� ����� ���ϴ�.

        ColorCenters.OnColoringOff();

        // ���� ���� �ִٸ�, �����մϴ�.
        if (m_CheckModel != null)
        {
            Destroy(m_texIns);
            Destroy(m_CheckModel.gameObject);
            m_CheckModel = null;
        }
    }

    /// <summary>
    /// ĸ��ȭ���� �����ϴ� �����Դϴ�.
    /// </summary>
    /// <returns></returns>
    IEnumerator CRSaveScreenshot()
    {
        onCapture = true;

        yield return new WaitForEndOfFrame();

        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) == false)
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);

            yield return new WaitForSeconds(0.2f);
            yield return new WaitUntil(() => Application.isFocused == true);

            if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) == false)
            {
                OpenAppSetting();

                onCapture = false;
                yield break;
            }
        }

        // ���� ���� ��ġ�Դϴ�.
        string fileLocation = "mnt/sdcard/DCIM/Screenshots/";
        string filename = Application.productName + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        string finalLOC = fileLocation + filename;

        if (!Directory.Exists(fileLocation))
        {
            Directory.CreateDirectory(fileLocation);
        }

        // ���� ȭ���� �����ϴ� ����
        byte[] imageByte; //��ũ������ Byte�� ����.Texture2D use 
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);
        tex.Apply();

        imageByte = tex.EncodeToPNG();
        DestroyImmediate(tex);

        File.WriteAllBytes(finalLOC, imageByte);


        AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent", new object[2] { "android.intent.action.MEDIA_SCANNER_SCAN_FILE", classUri.CallStatic<AndroidJavaObject>("parse", "file://" + finalLOC) });
        objActivity.Call("sendBroadcast", objIntent);

        //�Ʒ� �� �� ���� ������ �ȵ���̵� �÷�����. ������ ���� ȣ���ϴ� �Լ��� �־��ָ� �ȴ�.
        //AGUIMisc.ShowToast(finalLOC + "�� �����߽��ϴ�.");
        onCapture = false;
    }

    // ���� �����ϴ� ������ �մϴ�.
    // https://forum.unity.com/threads/redirect-to-app-settings.461140/
    private void OpenAppSetting()
    {
        {
#if UNITY_ANDROID
            using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                string packageName = currentActivityObject.Call<string>("getPackageName");

                using (var uriClass = new AndroidJavaClass("android.net.Uri"))
                using (AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromParts", "package", packageName, null))
                using (var intentObject = new AndroidJavaObject("android.content.Intent", "android.settings.APPLICATION_DETAILS_SETTINGS", uriObject))
                {
                    intentObject.Call<AndroidJavaObject>("addCategory", "android.intent.category.DEFAULT");
                    intentObject.Call<AndroidJavaObject>("setFlags", 0x10000000);
                    currentActivityObject.Call("startActivity", intentObject);
                }
            }
#endif
        }
    }
}
