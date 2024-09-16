// すごろくのマス情報

using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
    // すごろくの座標を取得するために各マスのオブジェクトを格納(transformのリストでもいい)
    public GameObject[] MassGameObjects;
    public Sprite[] background;
}
