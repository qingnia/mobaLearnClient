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
    public Text text;
    public int HP;
}

public class BulletState
{
    public int id;
    public GameObject go;
}

public class MobaLogic : MonoBehaviour
{
    public int myId;
    public Dictionary<int, MobaState> allPlayer;
    public Dictionary<int, BulletState> allBullet = new Dictionary<int, BulletState>();

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
        Instance = this;
        allPlayer = new Dictionary<int, MobaState>();
    }

    public void Init()
    {
        MainUI.Instance.onbutton = MatchMethod;
        uiText = MainUI.Instance.button.GetComponentInChildren<Text>();
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

    public void Update()
    {
        if (state == State.InGame)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    Skill(1);
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    Skill(3);
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    Skill(2);
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    Skill(0);
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    MoveTo(1);
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    MoveTo(3);
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    MoveTo(2);
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    MoveTo(0);
                }
            }
        }
    }

    public void Skill(int dir)
    {
        var cmd = CGPlayerCmd.CreateBuilder();
        cmd.Cmd = "Skill " + dir;
        NetworkScene.Instance.SendPacket(cmd);
    }

    public void MoveTo(int dir)
    {
        var cmd = CGPlayerCmd.CreateBuilder();
        cmd.Cmd = "MoveTo " + dir;
        NetworkScene.Instance.SendPacket(cmd);
    }

    #endregion
    #region 服务器到客户端
    public void MatchSuc()
    {
        Debug.LogWarning("match success");
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
            Debug.Log("sync state " + c0);
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

    public void SyncBullet(string[] cmds)
    {
        for (var i = 1; (i + 2) < cmds.Length; i += 2)
        {
            var id = System.Convert.ToInt32(cmds[i]);
            var px = System.Convert.ToInt32(cmds[i + 1]);
            var py = System.Convert.ToInt32(cmds[i + 2]);
            if (allBullet.ContainsKey(id))
            {

            }
            else
            {
                var BulletS = new BulletState();
                BulletS.id = id;

                GameObject go = MainUI.Instance.bullet;

                var copyQizi = (GameObject)GameObject.Instantiate(go);
                copyQizi.transform.parent = go.transform.parent;
                copyQizi.transform.localScale = Vector3.one;
                copyQizi.transform.localPosition = Vector3.zero;
                BulletS.go = copyQizi;
                allBullet.Add(id, BulletS);
            }
            var qizi2 = allBullet[id].go;
            var curPos = MainUI.Instance.GetPos(px, py);
            qizi2.transform.localPosition = new Vector3(curPos.x, curPos.y, 0);
            qizi2.SetActive(true);
        }
        //TODO : 删除死亡子弹
    }

    public void SyncPos(string[] cmds)
    {
        //Debug.Log("length:" + cmds.Length + " msg:" + cmds.ToString());
        for(var i = 1; (i+2) < cmds.Length; i+=4)
        {
            var id = System.Convert.ToInt32(cmds[i]);
            var px = System.Convert.ToInt32(cmds[i + 1]);
            var py = System.Convert.ToInt32(cmds[i + 2]);
            var hp = System.Convert.ToInt32(cmds[i + 3]);

            //Debug.Log("id:" + id + " px:" + px + " py:" + py);
            if (allPlayer.ContainsKey(id))
            {

            }
            else
            {
                var mobaState = new MobaState();
                mobaState.playerID = id;

                GameObject qizi = new GameObject();
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
                mobaState.qizi = copyQizi;
                mobaState.text = copyQizi.transform.Find("HP").GetComponent<Text>();
                allPlayer.Add(id, mobaState);
            }

            var qizi2 = allPlayer[id].qizi;
            var p = allPlayer[id];
            p.pos = new Vector2(px, py);
            var curPos = MainUI.Instance.GetPos(px, py);
            qizi2.transform.localPosition = new Vector3(curPos.x, curPos.y, 0);
            allPlayer[id].text.text = hp.ToString();
            qizi2.SetActive(true);
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
