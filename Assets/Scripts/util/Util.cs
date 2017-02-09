using UnityEngine;
using System.Collections;
using System;

namespace MyLib {

    public partial class Util {

        public class Pair
        {
            public byte moduleId;
            public byte messageId;

            public Pair(byte a, byte b)
            {
                moduleId = a;
                messageId = b;
            }
        }


        public static Pair GetMsgID(string name)
        {
            return SaveGame.saveGame.GetMsgID(name);
        }

        public static Pair GetMsgID(string moduleName, string name)
        {
            Debug.Log("moduleName " + moduleName + " " + name);
            var mId = SaveGame.saveGame.msgNameIdMap [moduleName] ["id"].AsInt;
            var pId = SaveGame.saveGame.msgNameIdMap [moduleName] [name].AsInt;
            return new Pair((byte)mId, (byte)pId);
        }


        public static double GetTimeNow()
        {
            return DateTime.UtcNow.Ticks / 10000000.0;
        }
        public static void ShowMsg(string msg) {
        }
    }



}