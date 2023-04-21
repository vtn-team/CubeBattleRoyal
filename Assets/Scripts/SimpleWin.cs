using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SimpleWin : MonoBehaviour
{

    void Start()
    {
        // シーン内の全てのオブジェクトを取得
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // オブジェクトを１つずつ確認し、自分とライトとカメラ以外は破壊する
        foreach (GameObject obj in allObjects)
        {
            if (obj != this.gameObject && obj.GetComponent<Light>() == null && obj.GetComponent<Camera>() == null)
            {
                Destroy(obj);
            }
        }
    }
    void OnDestroy()
    {
        // 自身が破壊されたら現在のシーンを再読み込みする
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
