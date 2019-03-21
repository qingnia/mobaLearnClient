
/*
Author: liyonghelpme
Email: 233242872@qq.com
*/

/*
Author: liyonghelpme
Email: 233242872@qq.com
*/

using MyLib;
using UnityEngine;
using System.Collections;
using KBEngine;
using System;

public class ClientApp : UnityEngine.MonoBehaviour
{
    public static GameObject client;
    public static ClientApp Instance;
    public int updateInterval;

    /*
     * Player Position Update Frequency
     */ 
    public int updateIntervalOnSerialize;
    int nextSendTickCount = Environment.TickCount;
    //public string url = "10.1.2.223";
    //public int port = 17000;
    //public string testUrl = "192.168.2.5";
    public int testPort = 20000;
    //public bool debug = false;
    public string remoteServerIP;
    public int remotePort = 10001;
    public float syncFreq = 0.1f;

    public int remoteUDPPort = 10001;
    public int UDPListenPort = 10002;

    public int ServerHttpPort = 12002;
    public int remoteKCPPort = 6060;

    public bool testAI = false;

    public string QueryServerIP;
    void Awake()
    {
        client = gameObject;
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        UnityEngine.MonoBehaviour.print("client app start");
    }


    void OnDestroy()
    {
        UnityEngine.MonoBehaviour.print("clientapp destroy");
    }

    void Update()
    {
    }

    public bool IsPause = false;
    public void OnApplicationPause(bool pauseStatus) {
    }

    [ButtonCallFunc()]
    public bool PauseTest;

    public void PauseTestMethod()
    {
        OnApplicationPause(true);
    }

}
