using MyTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemCallBack : MonoBehaviour
{
    void Start()
    {
        var main = GetComponent<ParticleSystem>().main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    void OnParticleSystemStopped()
    {
        ReturnToPoolAction();
    }
    public void ReturnToPoolAction()
    {
        ObjectPoolHandler.instance.ReturnObjectToPool(this.gameObject);
    }
}
