using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

class CheckLogic : MonoBehaviour
{
    GameObject[] _list;
    List<Vector3> _curPosition = new List<Vector3>();
    public void RelayList(GameObject[] list)
    {
        _list = list;
        _curPosition.Clear();
        foreach (var d in _list)
        {
            _curPosition.Add(d.transform.position);
        }
    }

    //priorotyが低い
    private void LateUpdate()
    {
        for(int i=0; i<_list.Length; ++i)
        {
            if (_list[i] == null) continue;

            //Playerのコンポーネントチェック
            var player = _list[i].GetComponent<Player>();
            if(player)
            {
                if(!player.GetComponent<Collider>() || !player.GetComponent<Rigidbody>())
                {
                    Debug.Log($"Player:{player.name}に足りないコンポーネントがあります。");
                }
            }

            //移動距離チェック
            if ((_curPosition[i] - _list[i].transform.position).magnitude > 20)
            {
                if (_list[i].name == "TargetGroup1") continue;
                if (_list[i].name == "cm") continue;
                if (_list[i].name == "Main Camera") continue;
                if (_list[i].name == "CM vcam1") continue;

                //システムによる破壊
                Debug.Log($"ルールにより{_list[i].name}を破壊します");
                GameObject.Destroy(_list[i]);
            }
        }

        ShotQueue.Instance.ShotAll();
    }
}

public class ShotQueue
{
    static ShotQueue _instance = new ShotQueue();
    static public ShotQueue Instance { get { return _instance; } }
    ShotQueue() { }

    class Queue
    {
        public GameObject From;
        public Type BulletType;
    }
    List<Queue> _queue = new List<Queue>();

    public void Enqueue(GameObject from, Type bulletType)
    {
        _queue.Add(new Queue { From = from, BulletType = bulletType });
    }

    public void ShotAll()
    {
        while (_queue.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, _queue.Count);

            GameObject from = _queue[index].From;
            if (from)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                BulletBase script = cube.AddComponent(_queue[index].BulletType) as BulletBase;
                cube.transform.position = from.transform.position + from.transform.forward * from.transform.localScale.z;
                script.Bang();
            }
            _queue.RemoveAt(index);
        }
    }
}