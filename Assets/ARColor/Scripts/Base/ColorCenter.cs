using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class ColorCenter : BaseColor
{

    public GameObject Atlas;

    public Transform ImList = null;


    public Dictionary<string, Texture2D> Dic_DateAndTe;

    public Dictionary<string, ColorData> Dic_DateAndData;

    public GameObject ARCamera;

    public GameObject UICanvas;

    CsMainUI MainUI;

    private bool ifCanColor = true;

    bool bStartArCamera;

    Coroutine coFindTime = null;

    PlaneManager plane;

    protected override void Start()
    {
        base.Start();
        MainUI = UICanvas.GetComponent<CsMainUI>();
        plane = new PlaneManager();
        //ARCamera.SetActive(false);
    }


    public void Btn_Color()
    {
        //ShotAndColor();
        //delayColor();
        if (ifCanColor)
        {
            //MainUI.FindTargetUI(true);
            ShotAndColor();
            ifCanColor = false;
            if (coFindTime == null)
            {
                coFindTime = StartCoroutine(delayColor());
            }

        }
        //else
        //{
        //    StartCoroutine(delayColor2());
        //    //MainUI.FindTargetUI(false);
        //}

    }

    private void Update()
    {
        //if (bStartArCamera)
        //{
        //    Btn_Color();
        //}
    }


    public void Btn_Clean()
    {
        RemoveTexture();
    }


    public void Btn_AtlasClose()
    {
        Atlas.SetActive(false);
    }


    public void LoadSavedColorDatas()
    {
        //Dic_DateAndTe = new Dictionary<string, Texture2D>();
        //Dic_DateAndData = new Dictionary<string, ColorData>();

        //string _fileNm =CardNm+ ".txt";

        //ColorDatas _colorDatas = SaveColorUtil.GetInstance().LoadColorInfo(_fileNm);

        //if (_colorDatas == null)
        //{
        //    return;
        //}

        //if (_colorDatas.Datas.Length <= 0)
        //{
        //    return;
        //}

        //GameObject _imPre = Resources.Load<GameObject>("Im_Saved");

        //ColorData[] _datas = _colorDatas.Datas;


        //Image[] _oldIms = ImList.GetComponentsInChildren<Image>();
        //if (_oldIms != null)
        //{
        //    if (_oldIms.Length > 0)
        //    {
        //        for (int i = 0; i < _oldIms.Length; i++)
        //        {
        //            Destroy(_oldIms[i].gameObject);
        //        }
        //    }
        //}


        //List<string> _timeDatas = new List<string>();
        //for (int i = 0; i < _datas.Length; i++)
        //{
        //    _timeDatas.Add(_datas[i].TimeDate);
        //}


        //List<Texture2D> _tes = SaveColorUtil.GetInstance().LoadColorTestrues(_timeDatas);


        //for (int i = 0; i < _datas.Length; i++)
        //{

        //    string _timeDate = _datas[i].TimeDate;

        //    GameObject _temp = Instantiate(_imPre);
        //    _temp.transform.SetParent(ImList);


        //    _temp.transform.Find("Tx_Date").GetComponent<Text>().text = _timeDate;

        //    Texture2D tex = _tes[i];


        //    Sprite _sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            
        //    _temp.GetComponent<Image>().sprite = _sprite;

        //    //Dictionary
        //    Dic_DateAndTe.Add(_timeDate, tex);
        //    Dic_DateAndData.Add(_timeDate, _datas[i]);

        //    //Thumbnail button

        //    Button _btn = _temp.GetComponent<Button>();
        //    //Load button

        //    Button _btn_Color = _temp.transform.Find("Pn_Btns/Btn_Load").GetComponent<Button>();
        //    //Delete button

        //    Button _btn_Delete= _temp.transform.Find("Pn_Btns/Btn_Delete").GetComponent<Button>();

            
        //    _btn.onClick.AddListener(delegate ()
        //    {
        //        string _date = _btn.transform.Find("Tx_Date").GetComponent<Text>().text;
        //        Texture2D _savedTe = Dic_DateAndTe[_date];
        //        ColorData _savedDate = Dic_DateAndData[_date];

        //        Vector3 TopLeft_Pl_W = _savedDate.TopLeft_Pl_W;
        //        Vector3 BottomLeft_Pl_W = _savedDate.BottomLeft_Pl_W;
        //        Vector3 TopRight_Pl_W = _savedDate.TopRight_Pl_W;
        //        Vector3 BottomRight_Pl_W = _savedDate.BottomRight_Pl_W;
        //        Matrix4x4 VP = _savedDate.VP;

        //        Set_SavedColor(_savedTe, TopLeft_Pl_W, BottomLeft_Pl_W, TopRight_Pl_W, BottomRight_Pl_W, VP);

        //        Atlas.SetActive(false);
        //    });
        //    _btn_Color.onClick.AddListener(delegate ()
        //    {
        //        string _date = _btn.transform.Find("Tx_Date").GetComponent<Text>().text;
        //        Texture2D _savedTe = Dic_DateAndTe[_date];
        //        ColorData _savedDate = Dic_DateAndData[_date];

        //        Vector3 TopLeft_Pl_W = _savedDate.TopLeft_Pl_W;
        //        Vector3 BottomLeft_Pl_W = _savedDate.BottomLeft_Pl_W;
        //        Vector3 TopRight_Pl_W = _savedDate.TopRight_Pl_W;
        //        Vector3 BottomRight_Pl_W = _savedDate.BottomRight_Pl_W;
        //        Matrix4x4 VP = _savedDate.VP;

        //        Set_SavedColor(_savedTe, TopLeft_Pl_W, BottomLeft_Pl_W, TopRight_Pl_W, BottomRight_Pl_W, VP);

        //        Atlas.SetActive(false);
        //    });
        //    _btn_Delete.onClick.AddListener(delegate ()
        //    {
        //        string _date = _btn.transform.Find("Tx_Date").GetComponent<Text>().text;

        //        SaveColorUtil.GetInstance().DeleteOneColor(CardNm, _date);

        //        Destroy(_btn_Delete.transform.parent.parent.gameObject);
        //    });

        //    _temp.GetComponent<RectTransform>().localScale = new Vector3(1f,1f,1f);
        //}

        //Atlas.SetActive(true);
    }

    IEnumerator delayColor()
    {
        yield return new WaitForSeconds(0.1f);
        ifCanColor = true;
        coFindTime = null;
        AllStopCoroutine();
    }

    IEnumerator delayColor2()
    {
        yield return new WaitForSeconds(0.2f);
        ifCanColor = true;
    }


    public void Btn_DeleteAll()
    {
        SaveColorUtil.GetInstance().DeleteAllColorInfo();
    }

    public void Btn_Start()
    {
        //ARCamera.SetActive(true);
        bStartArCamera = true;
        MainUI.SetCameraOn();
        StartCoroutine(DelayInfinity());
    }

    IEnumerator DelayInfinity()
    {
        while(bStartArCamera)
        {
            ShotAndColor();
            yield return new WaitForSeconds(0.5f);
            //AllStopCoroutine();

            TrackerManager.Instance.GetTracker<PositionalDeviceTracker>().Reset();
            RemoveTexture();
        }
    }
}
