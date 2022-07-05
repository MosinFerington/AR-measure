using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.Events;

public class Snapshot : MonoBehaviour
{
    public Camera cam;
    public GameObject[] HideUI;
    string DeviceDataPath;
    [HideInInspector]
    public Texture2D ImageToSave;
    string fileName = "";
    public GameObject ImagePreviewPanel;
    public Image ImagePreview;

    public UnityEvent AddRoomPlanOption;

    void Start()
    {
        ImagePreviewPanel.gameObject.SetActive(false);

        if (Application.platform == RuntimePlatform.WindowsEditor) {
            DeviceDataPath = Application.dataPath;
        }
        else {
            DeviceDataPath = Application.persistentDataPath;
        }

    }

    public void TransparentSnapshot()
    {
        SnapshotWhite();
    }

    public void SimpleSnapshot()
    {
        Snap();
    }

    void Snap()
    {
        for (int i = 0; i < HideUI.Length; i++)
        {
            if (HideUI[i] != null)
                HideUI[i].SetActive(false);
        }

        StartCoroutine(SnapshotSimple());
    }

    private IEnumerator SnapshotSimple()
    {

        yield return new WaitForEndOfFrame();
        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;

        ImageToSave = new Texture2D(width, height, TextureFormat.RGB24, false);
        // Read screen contents into the texture
        ImageToSave.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        ImageToSave.Apply();

        byte[] bytes;
        bytes = ImageToSave.EncodeToPNG();
        fileName = System.DateTime.Now.ToString("s").Replace("-", "").Replace("-", "").Replace(":", "").Replace(":", "");
        Debug.Log(fileName);

        string path = "";
        if (Application.platform == RuntimePlatform.WindowsEditor)
            path = string.Format(DeviceDataPath + "/Room_5_{0}.png", System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff"));
        else
            path = string.Format(Application.dataPath + "/Room_5_{0}.png", System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff"));

        System.IO.File.WriteAllBytes(path, bytes);
        Debug.Log(path);
        for (int i = 0; i < HideUI.Length; i++)
        {
            if (HideUI[i] != null)
                HideUI[i].SetActive(true);
        }

    }

    void SnapshotWhite()
    {
        cam.cullingMask = LayerMask.GetMask("Player");

        fileName = string.Format(DeviceDataPath + "/Room_5_{0}.png", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff"));
        int width = Screen.width;
        int height = Screen.width; // was height

        // This is slower, but seems more reliable.
        var bak_cam_targetTexture = cam.targetTexture;
        var bak_cam_clearFlags = cam.clearFlags;
        var bak_RenderTexture_active = RenderTexture.active;

        var tex_white = new Texture2D(width, height, TextureFormat.ARGB32, false);
       // var tex_black = new Texture2D(width, height, TextureFormat.ARGB32, false);
       // var tex_transparent = new Texture2D(width, height, TextureFormat.ARGB32, false);
        // Must use 24-bit depth buffer to be able to fill background.
        var render_texture = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.ARGB32);
        var grab_area = new Rect(0, 0, width, height);

        RenderTexture.active = render_texture;
        cam.targetTexture = render_texture;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.white;
        cam.Render();
        tex_white.ReadPixels(grab_area, 0, 0);
        tex_white.Apply();
        //byte[] pngShot = ImageConversion.EncodeToPNG(tex_white);
        //File.WriteAllBytes(fileName, pngShot);

        ImageToSave = tex_white;
        ApplySprite(tex_white);
       
        //Debug.Log("Snapshot Saved: " + fileName);
        cam.clearFlags = bak_cam_clearFlags;
        cam.targetTexture = bak_cam_targetTexture;
        RenderTexture.active = bak_RenderTexture_active;
        RenderTexture.ReleaseTemporary(render_texture);

        AddRoomPlanOption.Invoke();
    }


    IEnumerator CleanScreenshot()
    {
        yield return new WaitForEndOfFrame();
        int width = Screen.width;
        int height = Screen.height;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.cullingMask = LayerMask.GetMask("Player");
        cam.backgroundColor = Color.white;
        cam.Render();
        var tex_white = new Texture2D(width, height, TextureFormat.ARGB32, false);
        var grab_area = new Rect(0, 0, width, height);
        tex_white.ReadPixels(grab_area, 0, 0);
        tex_white.Apply();

        ImageToSave = tex_white;

        ApplySprite(tex_white);

        AddRoomPlanOption.Invoke();
    }

    void ApplySprite(Texture2D texture)
    {
        Sprite thumbnail = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        ImagePreview.sprite = thumbnail as Sprite;

        ImagePreviewPanel.SetActive(true);
    }
}