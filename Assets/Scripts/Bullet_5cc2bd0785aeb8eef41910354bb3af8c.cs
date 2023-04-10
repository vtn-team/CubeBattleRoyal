using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//ビルドエラーが起きます。コードの本質ではなかったので減点-1としました
//using static UnityEditor.Experimental.GraphView.GraphView;

/// <summary>
/// 課題用の弾処理
/// NOTE: 唯一編集するコード
/// </summary>
public class Bullet_5cc2bd0785aeb8eef41910354bb3af8c : BulletBase
{

    GameObject _player;
    List<Transform> _players = new List<Transform>();

    Vector3 _velocity;
    BoxCollider _collider;

    private void Awake()
    {
        Object.DontDestroyOnLoad(gameObject);
        Time.timeScale = 0;

        _collider = GetComponent<BoxCollider>();
        if (_collider != null)
        {
            _collider.enabled = true;
            _collider.isTrigger = false;
            Bounds bounds = _collider.bounds;
            bounds.Expand(float.MaxValue);
            _collider.size = bounds.size;
        }
    }

    private void Start()
    {
        //エラーが起きたので対応しました。進行はしたので減点-1とします。
        if (_players.Count == 0) return;
        _player = _players[0].gameObject;
        _players.Remove(_players[0]);

        _player.GetComponent<BoxCollider>().enabled = false;

        AllAttack();
    }

    public override void Bang()
    {
        _velocity = Logic.GetRandomTargetVec(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player(Clone)")
        {
            _players.Add(collision.gameObject.transform);
            _players.Sort((a, b) => Vector3.Distance(transform.position, a.position).CompareTo(Vector3.Distance(transform.position, b.position)));    
        }
    }

    void AllAttack()
    {
        foreach(var i in _players)
        {
            Logic.DamageToTarget(i.gameObject,i.gameObject);
        }
    }
}
