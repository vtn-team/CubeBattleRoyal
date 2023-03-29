using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 課題用ターゲッティング処理
/// </summary>
public class Logic
{
    static public Vector3 GetRandomTargetVec(BulletBase bullet)
    {
        //クソ重いけど課題のため
        var list = GameObject.FindObjectsOfType<Player>();
        int index = UnityEngine.Random.Range(0, list.Length);
        var player = list[index];
        var sub = player.transform.position - bullet.transform.position;
        sub.Normalize();
        return sub;
    }

    static public bool DamageToTarget(GameObject from, GameObject to)
    {
        if (from == null) return false;
        if (to == null) return false;

        Player player = to.GetComponent<Player>();
        if (player == null) return false;

        player.Damage(1);
        GameObject.Destroy(from);
        return true;
    }
}
