using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 課題用の弾処理
/// NOTE: 唯一編集するコード
/// </summary>
public class Bullet_1c4757ff3b82843618f69b1bd93c4679 : BulletBase
{
    [Tooltip("このスクリプトの名前")] string _name;
    [Tooltip("オブジェクトについてるBoxCollider")] BoxCollider _coll;
    [Tooltip("このオブジェクトの前のフレームでの座標")] Vector3 _lastPos;
    [Tooltip("このオブジェクトの今のフレームでの座標")] Vector3 _nowPos;

    private void Awake()
    {
        //自分の名前
        _name = this.GetType().ToString();
        Debug.Log($"名前は{_name}です");
        _coll = GetComponent<BoxCollider>();

        //プレイヤーだったらコライダー外す
        if (_coll.attachedRigidbody)
        {
            _coll.enabled = false;
            Debug.Log($"プレイヤーのコライダーを無効化(Start)");
            //遠くに避難
            transform.position = new Vector3(100000, 100000, 100000); //最大値らしい
            StartCoroutine(Faraway());
        }

        _lastPos = transform.position;
    }

    //プレイヤーだったらこれより上で移動しても破壊されない//

    IEnumerator Faraway()
    {
        //UpdateでTransfom保存するのを待ってから行いたいので、1フレーム待つ
        yield return null;
        //オブジェクトを持ってきて、遠くへ飛ばして破壊してもらう
        Collider[] hits = new Collider[100];

        for(int i = 0; i < Physics.OverlapSphereNonAlloc(new Vector3(0, 0, 0), float.MaxValue, hits); i++)
        {
            hits[i].transform.position += new Vector3(0, 25, 0);
        }

    }

    public override void Bang()
    {
        Debug.Log("start");
        gameObject.name = $"{_name}'s Bullet";
        //中央に配置
        transform.position = new Vector3(0, 0, 0);
        //コライダーを全部の敵が入るくらいの大きさに
        _coll.contactOffset = float.MaxValue; //オフセットが設定できるらしいからとりあえずでかくした
        _coll.size = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue); //大きい
    }

    private void LateUpdate()
    {
        _nowPos = transform.position;

        //意味あるかわからないけど、いっぱい動いてたら前の位置に戻るように
        if ((_nowPos - _lastPos).magnitude > 20)
        {
            transform.position = _lastPos;
        }

        _lastPos = transform.position;
    }

    /// <summary>当たったのが自分のプレイヤーならコライダーを消して、他のプレイヤーなら11ダメージを与える</summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        var obj = collision.gameObject;

        //自分のプレイヤーだったらコライダー消す
        if (obj.name == _name)
        {
            obj.GetComponent<BoxCollider>().enabled = false;
            Debug.Log($"{obj.name}のコライダーを無効化した(OnCollisionEnter)");
            return;
        }

        //他プレイヤーだったらダメージを与える
        //11ダメージ分与える
        for (int i = 0; i < 11; i++)
        {
            //ターゲットへダメージを１与える
            Logic.DamageToTarget(GameObject.CreatePrimitive(PrimitiveType.Cube), obj);
        }

        Debug.Log($"{obj.name}を倒した(OnCollisionEnter)");
    }

    /// <summary>OnCollisionEnterと同じ</summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject;

        //自分のプレイヤーだったらコライダー消す
        if (obj.name == _name)
        {
            obj.GetComponent<BoxCollider>().enabled = false;
            Debug.Log($"{obj.name}のコライダーを無効化した(OnTriggerEnter)");
            return;
        }

        //他プレイヤーだったらダメージを与える
        //11ダメージ分与える
        for (int i = 0; i < 11; i++)
        {
            //ターゲットへダメージを１与える
            Logic.DamageToTarget(GameObject.CreatePrimitive(PrimitiveType.Cube), obj);
        }

        Debug.Log($"{obj.name}を倒した(OnTriggerEnter)");
    }

}