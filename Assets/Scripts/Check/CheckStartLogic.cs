using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class CheckStartLogic : MonoBehaviour
{
    [SerializeField] CheckLogic _checker;

    //priorotyが高い
    private void Update()
    {
        GameObject[] list = GameObject.FindObjectsOfType<GameObject>();
        _checker.RelayList(list);
    }
}