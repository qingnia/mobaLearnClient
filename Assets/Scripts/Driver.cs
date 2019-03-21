using UnityEngine;
using System.Collections;
using MyLib;

public class Driver : MonoBehaviour {

    void Awake() {
        gameObject.AddComponent<SaveGame>();
        gameObject.AddComponent<NetworkScene>();

    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
