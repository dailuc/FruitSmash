using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelEditorButton : MonoBehaviour {
	public int id;
	public int code;
	public int subcode;
	public string completeCode;
	public Sprite[] cellBgList;
	Image bg;
	public GameObject imgLock,imgIce;
	// Use this for initialization
	void Start () {
		GetComponent<Button>().onClick.AddListener(TaskOnClick);
		bg = GetComponent<Image> ();
		imgLock.SetActive(false);
		imgIce.SetActive(false);
		subcode=0;
		code=2;
	}
	public void reset(){
		imgLock.SetActive(false);
		imgIce.SetActive(false);
		subcode=0;
		code=2;
		if (code < 4) {
			Color color = 	bg.color;
			color.a = 255;
			bg.color = color;
		} else {
			code = 0;
			Color color = 	bg.color;
			color.a = 0;
			bg.color = color;
		}
		bg.sprite = cellBgList [code];
	}
	public void lockEnable(){
		subcode=4;
		imgLock.SetActive(true);
		imgIce.SetActive(false);

	}
	public void iceEnable(){
		subcode=5;
		imgLock.SetActive(false);
		imgIce.SetActive(true);
	}
	public string getCompleteCode(){
		completeCode=""+code;
		if (subcode!=0){
			completeCode +=""+subcode;
		}
		return completeCode;
	}
	void TaskOnClick()
	{
		LevelEditorScript.clickedButtonID=id;
		changeCellBackGround ();
		Debug.Log("You have clicked the button!   " +id);
	}

	void changeCellBackGround(){
		if (code < 4) {
			code++;
			Color color = 	bg.color;
			color.a = 255;
			bg.color = color;
		} else {
			code = 0;
			Color color = 	bg.color;
			color.a = 0;
			bg.color = color;
		}
		bg.sprite = cellBgList [code];
	}
}
