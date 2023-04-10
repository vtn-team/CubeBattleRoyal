using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 課題用プレイヤー処理
/// NOTE: 編集はできないコード
/// </summary>
public class NamePlate : MonoBehaviour
{
    GameObject _target;
    [SerializeField] UnityEngine.UI.Text _text;

    public void Setup(GameObject go, string targetName)
    {
        _target = go;
        _text.text = targetName;
    }

    void Update()
    {
        if (!_target)
        {
            this.gameObject.SetActive(false);
            return;
        }

        this.transform.position = Camera.main.WorldToScreenPoint(_target.transform.position);
    }
}