using UnityEngine;

/// <summary>
/// 課題用の弾処理
/// NOTE: 唯一編集するコード
/// </summary>
[DefaultExecutionOrder(-1)]
public class Bullet_3d98b3aee99f4e745c93f3a3cc4897e0 : BulletBase
{
    static GameObject _parent;
    static GameObject _destroyer;

    public override void Bang()
    {
        if (_parent == null)
        {
            _parent = new GameObject();
            _parent.transform.position = Vector3.one * 100000;
            _parent.name = "pAr3n7";

            _destroyer = new GameObject();
            _destroyer.name = "D3s7r0y3r";
        }

        Collider[] results = Physics.OverlapSphere(transform.position, float.MaxValue);
        foreach (var v in results)
        {
            if (v.TryGetComponent(out Bullet _) && v.TryGetComponent(out Rigidbody __))
            {
                v.transform.localScale = Vector3.zero;
                v.transform.SetParent(_parent.transform);
            }
            else if (v.TryGetComponent(out Rigidbody _))
            {
                v.transform.SetParent(_destroyer.transform);
            }
        }
       
        _parent.transform.GetChild(0).localPosition = Vector3.zero;
        
        if (_destroyer != null)
        {
            _destroyer.transform.Translate(100, 0, 0);
        }

        // Rigidbodyを無効化
        Physics.autoSimulation = false;
        Physics.queriesHitTriggers = false;
        Physics.sleepThreshold = float.MaxValue;
    }

    private void Update()
    {
        if (_destroyer != null)
        {
            _destroyer.transform.Translate(100, 0, 0);
        }

        Physics.autoSimulation = false;
        Physics.queriesHitTriggers = false;
        Physics.sleepThreshold = float.MaxValue;
    }
}
