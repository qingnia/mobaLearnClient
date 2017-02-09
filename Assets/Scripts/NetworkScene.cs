using UnityEngine;
using System.Collections;

public class NetworkScene : MonoBehaviour {
    public int myId;
    public static NetworkScene Instance;
    private void Awake() {
        Instance = this;
    }

    public void MsgHandler(KBEngine.Packet packet)
    {
    }
}
