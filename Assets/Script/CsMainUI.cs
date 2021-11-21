using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Android;

public class CsMainUI : MonoBehaviour
{
    [SerializeField]
    Transform m_trTopUI;
    [SerializeField]
    Transform m_trMiddleUI;
    [SerializeField]
    Transform m_trBottomUI;
    [SerializeField]
    CaptrueScreenShot CaptureCamera;

    [SerializeField]
    ColorCenter ColorCenters;

    [SerializeField]
    Camera Arcamera;

    [SerializeField]
    Camera TargetModelcamera;

    [SerializeField]
    Material mat;

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

    AudioSource m_BGMPlayer;

    bool IsComplate;

    GameObject ComplateModel;

    public bool hideGUI = false;
    public Texture2D texture;
    public Text console;
    public CanvasGroup ui;
    public Image screenshot;
    private string paths = null;

    CsCheckOnModel m_CheckModel;
    Texture2D m_texIns;

    public CsCheckOnModel TargetModel;
    public Transform EndUI { get { return m_EndUI; } }

    bool onCapture = false;

    public void PressBtnCapture()
    {
        if (onCapture == false)
        {
            //StartCoroutine("CRSaveScreenshot");
        }
    }

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
                //다이얼로그를 위해 별도의 플러그인을 사용했었다. 이 코드는 주석 처리함.
                //AGAlertDialog.ShowMessageDialog("권한 필요", "스크린샷을 저장하기 위해 저장소 권한이 필요합니다.",
                //"Ok", () => OpenAppSetting(),
                //"No!", () => AGUIMisc.ShowToast("저장소 요청 거절됨"));

                // 별도로 확인 팝업을 띄우지 않을꺼면 OpenAppSetting()을 바로 호출함.
                OpenAppSetting();

                onCapture = false;
                yield break;
            }
        }

        string fileLocation = "mnt/sdcard/DCIM/Screenshots/";
        string filename = Application.productName + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        string finalLOC = fileLocation + filename;

        if (!Directory.Exists(fileLocation))
        {
            Directory.CreateDirectory(fileLocation);
        }

        byte[] imageByte; //스크린샷을 Byte로 저장.Texture2D use 
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

        //아래 한 줄 또한 별도의 안드로이드 플러그인. 별도로 만들어서 호출하는 함수를 넣어주면 된다.
        //AGUIMisc.ShowToast(finalLOC + "로 저장했습니다.");
        onCapture = false;
    }


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
        m_Btn_ProcessOn = m_trBottomUI.Find("Btn_ProcessOn").GetComponent<Button>();
        m_Btn_ProcessOff = m_trBottomUI.Find("Btn_ProcessOff").GetComponent<Button>();
        m_Btn_Complate = m_trBottomUI.Find("Btn_Complate").GetComponent<Button>();

        m_BGMPlayer = transform.GetComponent<AudioSource>();

        CaptureCamera = Arcamera.GetComponent<CaptrueScreenShot>();

        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
        }
        else
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
        }
        else
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
    }

    private void Start()
    {
        ColorProcessUI(false); // 처음에 컬러 기능 비활성화
        OnAudioOn();
        OnColoringOff();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void FindTargetUI(bool bFind)
    {
        if(bFind)
        {
            StartCoroutine(FindTargetComplate());
        }
        else
        {
            m_trImgMarkerRed.gameObject.SetActive(true);
            m_trImgMarkerBlue.gameObject.SetActive(false);
        }
    }

    public void ColorProcessUI(bool bFind)
    {
        if (bFind)
        {
            m_Btn_ProcessOn.gameObject.SetActive(false);
            Debug.LogError("깜박");
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
        Debug.LogError("깜박");
        m_trImgMarkerRed.gameObject.SetActive(false);
        m_trImgMarkerBlue.gameObject.SetActive(false);
    }

    public void OnClickCapture()
    {
        if (onCapture == false)
        {
            StartCoroutine("CRSaveScreenshot");
        }
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
        ColorCenters.ImageWidth = 1f;
        ColorCenters.ImageHeight = 1f;
    }

    public void OnColoringOn()
    {
        m_Btn_ProcessOn.gameObject.SetActive(false);
        m_Btn_ProcessOff.gameObject.SetActive(true);

        if(TargetModel.name == "3MiLook")
        {
            ColorCenters.ImageWidth = 1.35f;
            ColorCenters.ImageHeight = 1.35f;
        }
        else if (TargetModel.name == "1GamYoung")
        {
            ColorCenters.ImageWidth = 0.9f;
            ColorCenters.ImageHeight = 0.9f;
        }
        else
        {
            ColorCenters.ImageWidth = 1f;
            ColorCenters.ImageHeight = 1f;
        }
        ColorCenters.OnColoringOn();
    }

    public void OnColoringOff()
    {
        m_Btn_ProcessOn.gameObject.SetActive(true);
        m_Btn_ProcessOff.gameObject.SetActive(false);
        ColorCenters.OnColoringOff();
    }

    public void OnComplate()
    {
        if (!IsComplate)
        {
            if (TargetModel != null)
            {
                Arcamera.gameObject.SetActive(false);
                TargetModelcamera.gameObject.SetActive(true);

                m_CheckModel = Instantiate<CsCheckOnModel>(TargetModel);
                m_CheckModel.GetComponent<MeshRenderer>().enabled = true;

                Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                tex = m_CheckModel.GetComponent<MeshRenderer>().materials[0].GetTexture("_MainTex") as Texture2D;
                m_texIns = Instantiate<Texture2D>(tex);

                if (tex == null && m_texIns == null)
                {
                    Debug.LogError("못가져옴");
                }

                m_CheckModel.transform.DOMove(m_CheckModel.MovePosition, 1.0f);
                m_CheckModel.transform.DORotate(m_CheckModel.MoveRotation, 1.0f);
                m_CheckModel.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", m_texIns);
                StopCoroutine(ColorCenters.ShotAndColor2());
                ColorCenters.bStartArCamera = true;
                IsComplate = true;
            }
            else
            {
                Debug.LogError("타겟이 없음");
            }
        }
        else
        {
            Arcamera.gameObject.SetActive(true);
            TargetModelcamera.gameObject.SetActive(false);

            Destroy(m_CheckModel.gameObject);
            Destroy(m_texIns);

            TargetModel = null;
            IsComplate = false;
            ColorCenters.ResetModel();
        }
    }
}
