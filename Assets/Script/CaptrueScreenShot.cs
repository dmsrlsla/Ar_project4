using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class CaptrueScreenShot : MonoBehaviour
{
    public Camera camera;       //�������� ī�޶�.

    private int resWidth;
    private int resHeight;
    string path;
    // Use this for initialization
    void Start()
    {
        resWidth = Screen.width;
        resHeight = Screen.height;
        path = Application.dataPath + "/ScreenShot/";
        Debug.Log(path);
    }

    public void Capture_Button()
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(path);
        }
        string name;
        name = path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(name, bytes);
    }
}

//{
//    public GameObject blink;             // ���� ���� �� ������ ��
//    //public GameObject shareButtons;      // ���� ��ư

//    bool isCoroutinePlaying;             // �ڷ�ƾ �ߺ�����

//    // ���� �ҷ��� �� �ʿ�
//    string albumName = "";           // ������ �ٹ��� �̸�
//    [SerializeField]
//    GameObject panel;                    // ���� ������ �� �г�


//    // ĸ�� ��ư�� ������ ȣ��
//    public void Capture_Button()
//    {
//        // �ߺ����� bool
//        if (!isCoroutinePlaying)
//        {
//            StartCoroutine("captureScreenshot");
//        }
//    }

//    IEnumerator captureScreenshot()
//    {
//        isCoroutinePlaying = true;

//        // UI ���ش�...

//        yield return new WaitForEndOfFrame();

//        // ��ũ���� + ����������
//        ScreenshotAndGallery();

//        yield return new WaitForEndOfFrame();

//        // ��ũ
//        BlinkUI();

//        // ���� ���� �ֱ�...

//        yield return new WaitForEndOfFrame();

//        // UI �ٽ� ���´�...

//        yield return new WaitForSecondsRealtime(0.3f);

//        // ���� ������ ����
//        GetPirctureAndShowIt();

//        isCoroutinePlaying = false;
//    }

//    // ��� ��ũ ����
//    void BlinkUI()
//    {
//        GameObject b = Instantiate(blink);
//        b.transform.SetParent(transform);
//        b.transform.localPosition = new Vector3(0, 0, 0);
//        b.transform.localScale = new Vector3(1, 1, 1);
//    }

//    // ��ũ���� ��� �������� ����
//    void ScreenshotAndGallery()
//    {
//        // ��ũ����
//        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
//        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
//        ss.Apply();

//        // ����������
//        Debug.Log("" + NativeGallery.SaveImageToGallery(ss, albumName,
//            "Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + "{0}.png"));

//        // To avoid memory leaks.
//        // ���� �Ϸ�Ʊ� ������ ���� �޸� ����
//        Destroy(ss);

//    }
//    // ���� ������ Panel�� �����ش�.
//    void GetPirctureAndShowIt()
//    {
//        string pathToFile = GetPicture.GetLastPicturePath();
//        if (pathToFile == null)
//        {
//            return;
//        }
//        Texture2D texture = GetScreenshotImage(pathToFile);
//        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
//        panel.SetActive(true);
//        //shareButtons.SetActive(true);
//        panel.GetComponent<Image>().sprite = sp;
//    }
//    // ���� ������ �ҷ��´�.
//    Texture2D GetScreenshotImage(string filePath)
//    {
//        Texture2D texture = null;
//        byte[] fileBytes;
//        if (File.Exists(filePath))
//        {
//            fileBytes = File.ReadAllBytes(filePath);
//            texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
//            texture.LoadImage(fileBytes);
//        }
//        return texture;
//    }
//}