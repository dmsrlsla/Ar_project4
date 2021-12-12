using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using System.IO;
using Vuforia;
using DG.Tweening;

/// <summary>
/// UI와 컬러 처리부를 연결할 매니저입니다.(싱글톤 구조)
/// </summary>
public class ProgramManager : MonoBehaviour
{
    public static ProgramManager instance = null;

    //[SerializeField]
    //List<DefaultTrackableEventHandler> ListEventHandler = new List<DefaultTrackableEventHandler>();

    /// <summary>
    /// 컬러 처리부 입니다.
    /// </summary>
    [SerializeField]
    public ColorCenter ColorCenters;

    /// <summary>
    /// 메인 UI입니다.
    /// </summary>
    [SerializeField]
    public CsMainUI MainUI;

    /// <summary>
    /// AR 카메라 입니다.
    /// </summary>
    [SerializeField]
    public Camera Arcamera;

    /// <summary>
    /// 뷰모드에서 사용할 타겟 카메라 입니다.
    /// </summary>
    [SerializeField]
    Camera TargetModelcamera;

    /// <summary>
    /// 캡쳐 여부 판별 변수입니다.
    /// </summary>
    private bool onCapture = false;

    /// <summary>
    /// 현재 카메라에 비치는 타겟 모델정보입니다. 이 정보는 모델링에 달린 CsCheckModel에서 전달됩니다.
    /// </summary>
    private CsCheckOnModel _TargetModel;

    public CsCheckOnModel TargetModel { get { return _TargetModel; } set { _TargetModel = value; } }

    /// <summary>
    /// 뷰모드에서 화면에 비추어줄 클론 모델을 만들때 필요한 변수들입니다.
    /// </summary>
    private CsCheckOnModel m_CheckModel;
    private Texture2D m_texIns;

    /// <summary>
    /// 뷰모드 진입 버튼이 눌렸는지 체크합니다.
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
    /// 카메라 캡쳐 실행입니다.
    /// 갤러리에 현재 캡쳐화면을 저장합니다. MainUI호출
    /// </summary>


    public void OnClickCapture()
    {
        if (onCapture == false)
        {
            StartCoroutine(CRSaveScreenshot());
        }
    }

    /// <summary>
    /// 모델에서 검출이 되면, 타겟을 찾고 해당 타겟 모델을 등록함
    /// </summary>
    /// <param name="_NewTargetModel"></param>
    public void OnEventTargetOn(CsCheckOnModel _NewTargetModel)
    {
        ColorCenters.OnTargetFind();
        _TargetModel = _NewTargetModel;
    }

    /// <summary>
    ///  색칠 모드를 종료하고 타겟을 리셋함.
    /// </summary>
    public void OnEventTargetOff()
    {
        ColorCenters.OnTargetLost();
        _TargetModel = null;
    }

    /// <summary>
    /// 색입히기를 실행합니다. UI버튼이 눌리면, 캡쳐할 화면 비율을 넣고 컬러센터에 컬러링 기능을 On합니다.
    /// </summary>
    public void OnColoringOn()
    {
        ColorCenters.ImageWidth = TargetModel.Width;
        ColorCenters.ImageHeight = TargetModel.Height;
        ColorCenters.OnColoringOn();
    }

    /// <summary>
    /// 색입히기 기능을 종료합니다.
    /// </summary>
    public void OnColoringOff()
    {
        ColorCenters.OnColoringOff();
    }

    /// <summary>
    /// 음악을 틀고 끕니다.
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
    /// 뷰모드 입니다. AR카메라를 끄고, 색칠된 모델을 화면 중앙에 배치
    /// </summary>
    public void OnViewModeOn()
    {
        // AR 카메라 먼저 종료합니다.
        Arcamera.enabled = false;
        //foreach (DefaultTrackableEventHandler itb in ListEventHandler)
        //{
        //    itb.gameObject.SetActive(false);
        //}

        // 모델 카메라를 켭니다.
        TargetModelcamera.gameObject.SetActive(true);


        // 뷰 모델에 쓸 더미 모과 텍스쳐를 델을 만듭니다.
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

        // 만든 더미모델을 배치합니다.
        m_CheckModel.transform.DOMove(TargetModel.MovePosition, 1.0f);
        m_CheckModel.transform.DORotate(TargetModel.MoveRotation, 1.0f);
        m_CheckModel.GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", m_texIns);

        // 해당 기능을 실행했다고 체크합니다
        IsComplate = true;

        // 색칠기능은 뷰모드에서 정지합니다.
        ColorCenters.OnColoringOff();
    }
    /// <summary>
    /// 뷰 모드를 종료하고 다시 AR카메라로 돌아옴
    /// </summary>
    public void OnViewModeOff()
    {
        Arcamera.enabled = true;
        //foreach (DefaultTrackableEventHandler itb in ListEventHandler)
        //{
        //    itb.gameObject.SetActive(true);
        //}
        // 모델 카메라를 끕니다.
        TargetModelcamera.gameObject.SetActive(false);

        // 타겟모델이 있다면, 제거합니다.
        if (_TargetModel != null)
        {
            _TargetModel.GetComponent<MeshRenderer>().enabled = false;
            _TargetModel = null;
        }

        IsComplate = false;
        // 컬러링 기능을 끕니다.

        ColorCenters.OnColoringOff();

        // 더미 모델이 있다면, 제거합니다.
        if (m_CheckModel != null)
        {
            Destroy(m_texIns);
            Destroy(m_CheckModel.gameObject);
            m_CheckModel = null;
        }
    }

    /// <summary>
    /// 캡쳐화면을 저장하는 구문입니다.
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

        // 파일 저장 위치입니다.
        string fileLocation = "mnt/sdcard/DCIM/Screenshots/";
        string filename = Application.productName + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        string finalLOC = fileLocation + filename;

        if (!Directory.Exists(fileLocation))
        {
            Directory.CreateDirectory(fileLocation);
        }

        // 현재 화면을 저장하는 구문
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

    // 앱을 오픈하는 세팅을 합니다.
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
