using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomCreater : MonoBehaviour
{
    [MenuItem("GameObject/2D Object/Bridges/2m")]
    private static void CreateBridge()
    {
        CreateBridge(2);
    }
    [MenuItem("GameObject/2D Object/Bridges/3m")]
    private static void CreateBridge3()
    {
        CreateBridge(3);
    }

    [MenuItem("GameObject/2D Object/Bridges/4m")]
    private static void CreateBridge4()
    {
        CreateBridge(4);
    }

    [MenuItem("GameObject/2D Object/Bridges/5m")]
    private static void CreateBridge5()
    {
        CreateBridge(5);
    }

    [MenuItem("GameObject/2D Object/Bridges/6m")]
    private static void CreateBridge6()
    {
        CreateBridge(6);
    }

    [MenuItem("GameObject/2D Object/Bridges/7m")]
    private static void CreateBridge7()
    {
        CreateBridge(7);
    }

    [MenuItem("GameObject/2D Object/Bridges/8m")]
    private static void CreateBridge8()
    {
        CreateBridge(8);
    }

    [MenuItem("GameObject/2D Object/Bridges/9m")]
    private static void CreateBridge9()
    {
        CreateBridge(9);
    }

    [MenuItem("GameObject/2D Object/Bridges/10m")]
    private static void CreateBridge10()
    {
        CreateBridge(10);
    }

    [MenuItem("GameObject/2D Object/Bridges/11m")]
    private static void CreateBridge11()
    {
        CreateBridge(11);
    }

    [MenuItem("GameObject/2D Object/Bridges/12m")]
    private static void CreateBridge12()
    {
        CreateBridge(10);
    }

    [MenuItem("GameObject/2D Object/Bridges/13m")]
    private static void CreateBridge13()
    {
        CreateBridge(13);
    }

    [MenuItem("GameObject/2D Object/Bridges/14m")]
    private static void CreateBridge14()
    {
        CreateBridge(14);
    }

    [MenuItem("GameObject/2D Object/Bridges/15m")]
    private static void CreateBridge15()
    {
        CreateBridge(15);
    }

    static void CreateBridge(int length)
    {
        GameObject g = Resources.Load<GameObject>("Bridge Builder");
        BridgeBuilder b = Instantiate(g).GetComponent<BridgeBuilder>();
        b.length = length;
        b.BuildObject();
        DestroyImmediate(b.gameObject);
    }
}

