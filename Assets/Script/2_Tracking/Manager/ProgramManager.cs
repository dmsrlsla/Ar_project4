using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using System.IO;
using Vuforia;
using DG.Tweening;

public class ProgramManager : MonoBehaviour
{
    public static ProgramManager instance = null;

    public bool onCapture = false;

    public CsCheckOnModel TargetModel;

    [SerializeField]
    List<DefaultTrackableEventHandler> ListEventHandler = new List<DefaultTrackableEventHandler>();

    [SerializeField]
    public ColorCenter ColorCenters;

    [SerializeField]
    public Camera Arcamera;

    CaptrueScreenShot CaptureCamera;

    [SerializeField]
    Camera TargetModelcamera;

    CsCheckOnModel m_CheckModel;
    Texture2D m_texIns;


    public CsMainUI MainUI;

    public bool IsComplate = false;
    public bool IsShowMode = false;

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
        CaptureCamera = Arcamera.GetComponent<CaptrueScreenShot>();
        IsShowMode = false;
    }

    // 카메라 캡쳐 실행
    public void OnClickCapture()
    {
        if (onCapture == false)
        {
            StartCoroutine(CRSaveScreenshot());
        }
    }

    // 실행되면, 타겟을 찾고 해당 타겟 모델을 등록함
    public void OnEventTargetOn(CsCheckOnModel _TargetModel)
    {
        ColorCenters.OnTargetFind();
        TargetModel = _TargetModel;
    }

    // 색칠 모드를 종료하고 타겟을 리셋함.
    public void OnEventTargetOff()
    {
        ColorCenters.OnTargetLost();
        TargetModel = null;
    }

    public void OnColoringOn()
    {
        ColorCenters.ImageWidth = TargetModel.Width;
        ColorCenters.ImageHeight = TargetModel.Height;
        ColorCenters.OnColoringOn();
    }

    public void OnColoringOff()
    {
        ColorCenters.OnColoringOff();
    }

    // AR카메라를 끄고, 색칠된 모델을 화면 중앙에 배치
    public void OnViewModeOn()
    {
        if (TargetModel != null)
        {
            Debug.LogError("Test");
            // 카메라 먼저
            Arcamera.enabled = false;
            foreach (DefaultTrackableEventHandler itb in ListEventHandler)
            {
                itb.gameObject.SetActive(false);
            }
            IsShowMode = true;

            StopCoroutine(ColorCenters.ShotAndColor2());

            TargetModelcamera.gameObject.SetActive(true);

            m_CheckModel = Instantiate<CsCheckOnModel>(TargetModel);
            m_CheckModel.GetComponent<MeshRenderer>().enabled = true;

            TargetModel.GetComponent<MeshRenderer>().enabled = false;

            Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            tex = m_CheckModel.GetComponent<MeshRenderer>().materials[0].GetTexture("_MainTex") as Texture2D;
            m_texIns = Instantiate<Texture2D>(tex);

            if (tex == null && m_texIns == null)
            {
                Debug.LogError("못가져옴");
            }

            m_CheckModel.transform.DOMove(TargetModel.MovePosition, 1.0f);
            m_CheckModel.transform.DORotate(TargetModel.MoveRotation, 1.0f);
            m_CheckModel.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", m_texIns);

            ColorCenters.bStartArCamera = true;
            IsComplate = true;
            StopCoroutine(ColorCenters.ShotAndColor2());
        }
    }

    // 뷰 모드를 종료하고 다시 AR카메라로 돌아옴
    public void OnViewModeOff()
    {
        Arcamera.enabled = true;
        foreach (DefaultTrackableEventHandler itb in ListEventHandler)
        {
            itb.gameObject.SetActive(true);
        }
        IsShowMode = false;


        TargetModelcamera.gameObject.SetActive(false);

        Destroy(m_CheckModel.gameObject);
        Destroy(m_texIns);
        TargetModel.GetComponent<MeshRenderer>().enabled = false;
        ColorCenters.bStartArCamera = false;
        TargetModel = null;
        IsComplate = false;
        ColorCenters.ResetModel();
        ColorCenters.OnColoringOff();

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
}
