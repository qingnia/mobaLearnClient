using UnityEngine;
using System.Collections;
using MyLib;
using KBEngine;
using System;

public enum GameState {
    Idle,
    InGame,
    GameOver,
}

public class NetworkScene : MonoBehaviour {
    private GameState state = GameState.Idle;

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
            var logic = gameObject.AddComponent<Logic>();
            logic.Init();
        }
    }

    private RemoteClientEvent lastEvt = RemoteClientEvent.None;

    private void EvtHandler(RemoteClientEvent evt) {
        lastEvt = evt;
        Debug.Log("ClientEvent:"+evt);
    }


    public void MsgHandler(KBEngine.Packet packet)
    {
        var pb = packet.protoBody;
        var cmd = pb as GCPlayerCmd;
        var cmds = cmd.Result.Split(' ');
        switch(cmds[0]) {
            case "Init":
                Log.Net("Init:"+myId);
                myId = Convert.ToInt32(cmds[1]);
                break;
            case "GameStart":
                state = GameState.InGame;
                Logic.Instance.GameStart();
                break;
            case "NewTurn":
                Logic.Instance.NewTurn(cmds);
                break;
            case "MakeMove":
                Logic.Instance.UpdateMove(cmds);
                break;
            default:
                break;    
        }

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
