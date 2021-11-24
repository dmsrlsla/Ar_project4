﻿using System.Collections;
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

    public bool bStartArCamera;

    Coroutine coFindTime = null;

    PlaneManager plane;

    protected override void Start()
    {
        base.Start();
        MainUI = UICanvas.GetComponent<CsMainUI>();
        plane = new PlaneManager();
    }


    public void Btn_Color()
    {
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

    }

    public IEnumerator DelayInfinity()
    {
        while(bStartArCamera)
        {
            StartCoroutine(ShotAndColor2());
            yield return new WaitForSeconds(0.3f);


            //TrackerManager.Instance.GetTracker<PositionalDeviceTracker>().Reset();
            //RemoveTexture();
        }
    }

    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.3f);
    }

    public void OnTargetFind()
    {
        MainUI.FindTargetUI(true);
        StartCoroutine(WaitFind());
    }

    public void OnColoringOn()
    {
        bStartArCamera = true;
        StartCoroutine(DelayInfinity());
    }

    public void OnColoringOff()
    {
        bStartArCamera = false;
        StopCoroutine(ShotAndColor2());
        StopCoroutine(DelayInfinity());
        TrackerManager.Instance.GetTracker<PositionalDeviceTracker>().Reset();
        RemoveTexture();
    }

    IEnumerator WaitFind()
    {
        yield return new WaitForSeconds(1.0f);
        bStartArCamera = true;
    }

    public void ResetModel()
    {
        TrackerManager.Instance.GetTracker<PositionalDeviceTracker>().Reset();
        RemoveTexture();
    }
}
