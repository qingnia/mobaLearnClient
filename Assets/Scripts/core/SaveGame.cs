using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

namespace MyLib
{
    public class SaveGame : MonoBehaviour
    {
        public JSONClass msgNameIdMap;
        public static SaveGame saveGame;
	
        void Awake() {
            saveGame = this;
            LoadMsg();
        }

        void LoadMsg()
        {
            TextAsset bindata = Resources.Load("nameMap") as TextAsset;
            Debug.Log("nameMap " + bindata.text);
            msgNameIdMap = JSON.Parse(bindata.text).AsObject;
            Debug.Log("msgList " + msgNameIdMap.ToString());
        }

        public Util.Pair GetMsgID(string msgName)
        {
            foreach (KeyValuePair<string, JSONNode> m in msgNameIdMap)
            {
                if (m.Value [msgName] != null)
                {
                    int a = m.Value ["id"].AsInt;
                    int b = m.Value [msgName].AsInt;
                    return new Util.Pair((byte)a, (byte)b);     
                }
            }
            return null;
        }

        public string getMethodName(string module, int msgId)
        {
            var msgs = msgNameIdMap [module].AsObject;
            foreach (KeyValuePair<string, JSONNode> m in msgs)
            {
                if (m.Key != "id")
                {
                    if (m.Value.AsInt == msgId)
                    {
                        return m.Key;
                    }
                }
            }
            return null;

        }

        public string getMethodName(int moduleId, int msgId)
        {
            var module = SaveGame.saveGame.getModuleName(moduleId);
            return getMethodName(module, msgId);
        }

        public string getModuleName(int moduleId)
        {
            //Debug.Log("find Module Name is " + moduleId);
            foreach (KeyValuePair<string, JSONNode> m in msgNameIdMap)
            {
                var job = m.Value.AsObject;
                if (job ["id"].AsInt == moduleId)
                {
                    return m.Key;
                }
            }
            //Debug.Log("name map file not found  ");
            return null;
        }

    }

}