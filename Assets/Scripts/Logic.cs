using UnityEngine;
using System.Collections;
using MyLib;
using UnityEngine.UI;
using System;

public class Logic : MonoBehaviour {
    public static Logic Instance;
    void Awake() {
        Instance = this;

    }
    private Text uiText;

    public void Init() {
        isMe = false;
        isMeFirst = false;

        MainUI.Instance.onbutton = MatchMethod;
        var text = MainUI.Instance.button.transform.Find("Text").GetComponent<Text>();
        text.text = "Match";
        uiText = text;
    }

    [ButtonCallFunc()]
    public bool Match;

    [ButtonCallFunc()]
    public bool MakeMove;

    public int pos;


    [ButtonCallFunc()]
    public bool Leave;

    public void MatchMethod() {
        var cmd = CGPlayerCmd.CreateBuilder();
        cmd.Cmd = "Match";
        NetworkScene.Instance.SendPacket(cmd);

        uiText.text = "Matching";
        MainUI.Instance.onbutton = null;
    }

    public void MakeMoveMethod() {
        var cmd = CGPlayerCmd.CreateBuilder();
        cmd.Cmd = string.Format("MakeMove {0}", pos);
        NetworkScene.Instance.SendPacket(cmd);
    }


    public void GameStart() {
        uiText.text = "InGaming";
    }

    private bool isMe = false;
    private bool isMeFirst = false;

    public void NewTurn(string[] cmds) {
        var curTurn  = Convert.ToInt32(cmds[1]);
        var playerId = Convert.ToInt32(cmds[2]);
        var who = "me";
        if(playerId != NetworkScene.Instance.myId) {
            who = "other";
        }

        uiText.text = string.Format("Turn {0} {1}", curTurn, who);
        isMe = who == "me";
        if(isMe && curTurn == 0) {
            isMeFirst = true;
        }else {
            isMeFirst = false;
        }

        MainUI.Instance.onpos = OnPos;

    }

    void OnPos(int p) {
        if(isMe) {
            pos = p;
            MakeMoveMethod();
        }
    }

    public void UpdateMove(string[] cmds) {
        var playerId = Convert.ToInt32(cmds[1]);
        var pos = Convert.ToInt32(cmds[2]);
        var meToDo = playerId == NetworkScene.Instance.myId;
        GameObject qizi;
        if(meToDo) {
            qizi = MainUI.Instance.O;
        }else {
            qizi = MainUI.Instance.X;
        }

        var copyQizi = (GameObject)GameObject.Instantiate(qizi);
        copyQizi.transform.parent = qizi.transform.parent;
        copyQizi.transform.localScale = Vector3.one;
        copyQizi.transform.localPosition = Vector3.zero;

        copyQizi.transform.localPosition = MainUI.Instance.gos[pos].transform.localPosition;
    }
}

