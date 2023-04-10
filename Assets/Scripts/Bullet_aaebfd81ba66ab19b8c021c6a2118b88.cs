using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
/// <summary>
/// TheSmashingPumpkins-BulletWithButterflyWings
/// https://youtu.be/6_9sa-0F3R4
/// ちくしょー 自分を認識さえできたら
/// </summary>
public class Bullet_aaebfd81ba66ab19b8c021c6a2118b88 : BulletBase
{
    static CinemachineTargetGroup _targetGroup;
    Vector3 _velocity;
    GameObject[] _rootObject;
    static GameObject _ownPlayer;
    Scene _thisScene;
    int _ownplayernum = 0;
    public string playername;
    private void Awake()//再ロードで破壊されないようにする
    {
        GetCinemaTargetGroup();
        //DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        Bang();
    }
    public override void Bang()
    {
        if(_targetGroup ==null)
        {
            GetCinemaTargetGroup();
        }
        _velocity = Logic.GetRandomTargetVec(this);
        playername = this.GetType().ToString();
        var list = _targetGroup.m_Targets;
        for (int i = 0; i < list.Length; i++)
        {
            if(list[i].target != null)
            {
                GameObject player = list[i].target.gameObject;
                Logic.DamageToTarget(player, player);
            }
        }
    }

    private void Update()
    {
        Debug.Log(_targetGroup);
        
        Vector3 before = this.transform.position;
        this.transform.position += _velocity*2;
        Vector3 after = this.transform.position;
        //テスト用に距離を出す
        //Debug.Log((after - before).magnitude);
        _rootObject = _thisScene.GetRootGameObjects();
        for (int i = 0; i < _rootObject.Length; i++)
        {
            if (!_rootObject[i].GetComponent<Bullet_aaebfd81ba66ab19b8c021c6a2118b88>())
            {
                _rootObject[i].SetActive(false);
            }

        }

    }

    void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < 10; i++)
        {
            //第一引数に自分渡して当たったら球が消えるってことはcollision渡せばロジックが消してくれるってことやんけ
            //一応念入りに一度に体力０にして殺しとこか
            Logic.DamageToTarget(collision.gameObject, collision.gameObject);
        }
    }
    
    
    /// <summary>
    /// ヒエラルキーから全てのオブジェクトを取得
    /// CinemaSceneTargetGroup取得
    /// </summary>
    void GetCinemaTargetGroup()
    {
        _thisScene = SceneManager.GetActiveScene();
        _rootObject = _thisScene.GetRootGameObjects();
        for(int i =0; i < _rootObject.Length;i++)
        {
            if(_rootObject[i].name == "TargetGroup1")
            {
                _targetGroup = _rootObject[i].GetComponent<CinemachineTargetGroup>();
            }

        }

    }
    void SceneReLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void OnDestroy()
    {
       SceneReLoad();
    }
}
