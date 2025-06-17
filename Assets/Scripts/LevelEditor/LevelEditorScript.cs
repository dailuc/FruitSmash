using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
public class LevelEditorScript : MonoBehaviour {
	public GameObject parentTable,buttonCellPrefab;
	 List<GameObject> cellList;
	public static int clickedButtonID=0;
	public InputField ifLevel;
	// Use this for initialization
	void Start () {
		cellList = new List<GameObject> ();
		for (int i = 0; i < 63; i++) {
			Vector2 cellPos = Camera.main.ScreenToWorldPoint (new Vector2 ( 30+(i % 7 * 35), 400+(i / 7 * -35)));
			GameObject obj=Instantiate (buttonCellPrefab,cellPos, Quaternion.identity, parentTable.transform) as GameObject;
			obj.GetComponent<LevelEditorButton> ().id = i;
			cellList.Add (obj);
		}


	}
	public void reset(){
		cellList[clickedButtonID].GetComponent<LevelEditorButton>().reset();
	}
	public void lockEnable(){
		cellList[clickedButtonID].GetComponent<LevelEditorButton>().lockEnable();
	}
	public void iceEnable(){
		cellList[clickedButtonID].GetComponent<LevelEditorButton>().iceEnable();
	}
	// Update is called once per frame
	void Update () {
		
	}
	public void changeCellCode(){
	}
	public void saveMap(){
		string levelName = ifLevel.text+".txt";
		string contentToSave="";
	
		if (File.Exists(levelName)) 
		{
			Debug.Log("already exists. "+levelName);
			return;
		}


		string path = "Assets/Resources/"+levelName;

		//Write some text to the test.txt file
		StreamWriter writer = new StreamWriter(path, true);
		int dem =0;
		for (int i = 0; i < 63; i++) {
			contentToSave+= cellList[i].GetComponent<LevelEditorButton>().getCompleteCode();
			dem++;
			if ( dem ==7){
				contentToSave+="\n";
				dem=0;
			} else 
				contentToSave+="\t";
			
		}

		writer.WriteLine(contentToSave);
		writer.Close();
		Debug.Log("Create map success: "+levelName+" at Assets/Resources/");

	}
}
