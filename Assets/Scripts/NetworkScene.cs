using UnityEngine;
using System.Collections;
using MyLib;

public class NetworkScene : MonoBehaviour {
    public int myId;
    public static NetworkScene Instance;
    private MainThreadLoop ml;
    private void Awake() {
        Instance = this;
        ml = gameObject.AddComponent<MainThreadLoop>();

        StartCoroutine(ConnectServer());
    }
    private RemoteClient rc;
    private IEnumerator ConnectServer() {
        rc = new RemoteClient(ml);
        rc.evtHandler = EvtHandler;
        rc.msgHandler = MsgHandler;

        rc.Connect("127.0.0.1", 9091);
        while (lastEvt == RemoteClientEvent.None)
        {
            yield return null;
        }
        if(lastEvt == RemoteClientEvent.Connected) {
            gameObject.AddComponent<Logic>();
        }
    }

    private RemoteClientEvent lastEvt = RemoteClientEvent.None;

    private void EvtHandler(RemoteClientEvent evt) {
        lastEvt = evt;
    }

    public void MsgHandler(KBEngine.Packet packet)
    {
    }
}
