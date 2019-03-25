using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class MainUI : MonoBehaviour {
    public static MainUI Instance;

    public Button button;
    public GameObject O, X;

    public GameObject pos;

    public Action onbutton;
    public Action<int> onpos;

    public List<GameObject> gos = new List<GameObject>();

    public int offX = 258;
    public int offY = 200;
    // Use this for initialization
    void Awake () {
        Instance = this;

        button.onClick.AddListener(OnButton);

        var initPos = pos.transform.localPosition;
        for(var i = 0; i < 9; i++) {
            var col = i % 3;
            var row = i / 3;
            var np = (GameObject)GameObject.Instantiate(pos);
            np.transform.parent = pos.transform.parent;
            np.transform.localScale = Vector3.one;
            np.transform.localPosition = Vector3.zero;
            var newPos = initPos + new Vector3(offX*col, -offY*row ,0);
            np.transform.localPosition = newPos;
            var id = i;
            np.GetComponent<Button>().onClick.AddListener(()=>{
                OnPos(id);
            });

            gos.Add(np);
        }

        pos.gameObject.SetActive(false);
	}


    //根据棋盘位置返回实际的棋子坐标
    public Vector2 GetPos(int px, int py)
    {
        var initPos = pos.transform.localPosition;
        var ip = new Vector2(initPos.x, initPos.y);
        var newPos = ip + new Vector2(offX * px, -offY * py);
        return newPos;
    }

    private void OnButton() {
        if(onbutton != null) {
            onbutton();
        }
    }
	
    private void OnPos(int pos) {
        if(onpos != null) {
            onpos(pos);
        }
    }
}
