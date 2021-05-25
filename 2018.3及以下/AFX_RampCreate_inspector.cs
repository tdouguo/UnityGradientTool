using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AFX_RampCreate_inspector : MonoBehaviour {

    [Header("调整渐变")]
    public Gradient gradient = new Gradient();
    [Header("图像输出宽高")]
    public Vector2 resolution = new Vector2(256, 8);
    Color[] colorall;
    [Header("是否带Alpha通道")]
    public bool isAlpha;
    [Header("输出文件命名")]
    public string tex2dName = "AFX_Ramp";
    private Texture tex;
    [Header("右键脚本小齿轮选择路径")]
    public string path = ">>还未选择保存路径";
    [Header("设置输出名字（每次保存后面自动序号+1）")]
    private string texname = "AFX_Ramp";
    private int valueRampsource = 4;
    private int serial = 1;
    private float[] gaodus;
    [ContextMenu("设置输出大小为256*8")]
    void SetWH()
    {
        resolution = new Vector2(256, 8);
    }
    [ContextMenu("设置输出大小为512*8")]
    void SetWH1()
    {
        resolution = new Vector2(512, 8);
    }
    [ContextMenu("选择保存位置")]
    void SetPath1()
    {
        path = EditorUtility.OpenFolderPanel("", "", "");
        Debug.Log("你的保存路径为：" + path);
    }
    [ContextMenu("保存图像")]
    void Save()
    {
        OutRampTex();
    }
    [ContextMenu("查看使用教程")]
    void JiaoCheng()
    {
        Application.OpenURL("https://space.bilibili.com/7234711/channel/detail?cid=112022&ctype=0");
    }
    void OutRampTex()
    {
        colorall = new Color[(int)(resolution.x * resolution.y)];
        if (isAlpha == false)
        {
            gaodus = new float[(int)resolution.y];
            gaodus[0] = 0;
            float gao = 0;
            for (int g = 0; g < resolution.y; g++)
            {
                if (g == 0)
                {
                }
                else
                {
                    gao += resolution.x;
                    gaodus[g] = gao;
                }
            }
            for (int a = 0; a < resolution.y; a++)
            {
                for (int c = 0; c < resolution.x; c++)
                {
                    float temp = c / resolution.x;
                    colorall[(int)gaodus[a] + c] = gradient.Evaluate(temp);
                }
            }
        }
        else
        {
            gaodus = new float[(int)resolution.y];
            gaodus[0] = 0;
            float gao = 0;
            for (int g = 0; g < resolution.y; g++)
            {
                if (g == 0)
                {
                }
                else
                {
                    gao += resolution.x;
                    gaodus[g] = gao;
                }
            }
            for (int a = 0; a < resolution.y; a++)
            {
                for (int c = 0; c < resolution.x; c++)
                {
                    float temp = c / resolution.x;
                    colorall[(int)gaodus[a] + c] = gradient.Evaluate(temp);
                    colorall[(int)gaodus[a] + c].a = gradient.Evaluate(temp).a;
                }
            }
        }
        Save(colorall);
        Debug.Log("Ramp图已生成," + "名称：" + tex2dName + ",保存路径：" + path);
    }
    void Save(Color[] colors)
    {
        TextureFormat _texFormat;
        if (isAlpha)
        {
            _texFormat = TextureFormat.ARGB32;
        }
        else
        {
            _texFormat = TextureFormat.RGB24;
        }
        Texture2D tex = new Texture2D((int)resolution.x, (int)resolution.y, _texFormat, false);
        tex.SetPixels(colors);
        tex.Apply();
        byte[] bytes;
        bytes = tex.EncodeToPNG();
        string sname = tex2dName + "_" + serial;
        serial += 1;
        File.WriteAllBytes(path + "/" + sname + ".png", bytes);
        AssetDatabase.Refresh();
    }
}
