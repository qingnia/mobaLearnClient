using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyLib;

public class MobaState
{
    public Vector2 pos;
    public int playerID;
    public GameObject qizi;
}

public class MobaLogic : MonoBehaviour
{
    public int myId;
    public Dictionary<int, MobaState> allPlayer;
 
    public static MobaLogic Instance;

    public enum State
    {
        Wait,
        Matching,
        InGame,
        GameOver,
    }

    public Text uiText;
    private State state = State.Wait;
    private void Awake()
    {
        allPlayer = new Dictionary<int, MobaState>();
    }

    public void Init()
    {
        MainUI.Instance.onbutton = MatchMethod;
    }

    private void MatchMethod()
    {
        StartMatch();
    }
    #region 客户端到服务器
    public void StartMatch()
    {
        if (state == State.Wait)
        {
            state = State.Matching;
            var cmd = CGPlayerCmd.CreateBuilder();
            cmd.Cmd = "Match";
            NetworkScene.Instance.SendPacket(cmd);

            uiText.text = "Matching";
            MainUI.Instance.onbutton = null;
        }
    }

    public void MoveTo(int dir)
    {
        var cmd = CGPlayerCmd.CreateBuilder();
        cmd.Cmd = "Move " + dir;
        NetworkScene.Instance.SendPacket(cmd);
    }
    #endregion
    #region 服务器到客户端
    public void MatchSuc()
    {
        if (state == State.Matching)
        {
            state = State.InGame;
        }
    }

    public void SyncState(GCPlayerCmd cmd)
    {
        if (state == State.InGame)
        {
            var cmds = cmd.Result.Split(' ');
            var c0 = cmds[0];
            if (c0 == "InitID")
            {
                myId = System.Convert.ToInt32(cmds[1]);
            }
            else if (c0== "AddPlayer")
            {
                AddPlayer(cmds);
            }
            else if (c0 == "Move")//id 0 1 2 3
            {
                SyncMove(cmds);
            }
            else if (c0 == "NewTurn")
            {
                SyncPos(cmds);
            }
        }
    }

    public void SyncPos(string[] cmds)
    {
        for(var i = 1; i < cmds.Length; i+=3)
        {
            var id = System.Convert.ToInt32(cmds[i]);
            var px = System.Convert.ToInt32(cmds[i + 1]);
            var py = System.Convert.ToInt32(cmds[i + 2]);

            if (allPlayer.ContainsKey(id))
            {

            }
            else
            {
                var mobaState = new MobaState();
                mobaState.playerID = id;

                GameObject qizi;
                if (id == NetworkScene.Instance.myId)
                {
                    qizi = MainUI.Instance.O;
                }
                else
                {
                    qizi = MainUI.Instance.X;
                }

                var copyQizi = (GameObject)GameObject.Instantiate(qizi);
                copyQizi.transform.parent = qizi.transform.parent;
                copyQizi.transform.localScale = Vector3.one;
                copyQizi.transform.localPosition = Vector3.zero;
                mobaState.qizi = qizi;
                allPlayer.Add(id, mobaState);
            }

            var qizi = allPlayer[id].qizi;
            var p = allPlayer[id];
            p.pos = new Vector2(px, py);
            var curPos = MainUI.Instance.GetPos(px, py);
            qizi.transform.localPosition = new Vector3(curPos.x, curPos.y, 0);
        }
    }

    public void AddPlayer(string[] cmds)
    {

    }
    public void SyncMove(string[] cmds)
    {

    }
    #endregion
}
