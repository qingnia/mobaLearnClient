using UnityEngine;
using System.Collections;
using MyLib;
using KBEngine;

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

        rc.Connect("127.0.0.1", 12031);
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
        Debug.Log("ClientEvent:"+evt);
    }

    public void MsgHandler(KBEngine.Packet packet)
    {
    }
    public void SendPacket(CGPlayerCmd.Builder cg) {
        Log.Net("BroadcastMsg: " + cg);
        if (rc != null)
        {
            Bundle bundle;
            var data = KBEngine.Bundle.GetPacket(cg, out  bundle);
            rc.Send(data, bundle);
        }
    }
}
