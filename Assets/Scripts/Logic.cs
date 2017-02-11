using UnityEngine;
using System.Collections;
using MyLib;

public class Logic : MonoBehaviour {

    [ButtonCallFunc()]
    public bool Match;

    [ButtonCallFunc()]
    public bool MakeMove;

    [ButtonCallFunc()]
    public bool Leave;

    public void MatchMethod() {
        var cmd = CGPlayerCmd.CreateBuilder();
        cmd.Cmd = "Match";
        NetworkScene.Instance.SendPacket(cmd);
    }
}
