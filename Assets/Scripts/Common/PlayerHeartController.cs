using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHeartController : MonoBehaviour {
	public Text timerText;
	public Text heartCountText;
	int playerHeartCount;
	string lastTimeLoseHeart;
	static System.DateTime dateTimeLast,dateTimeNow,lastMinimize;
	static int totalPassedSeconds;
	// Use this for initialization
	public static int TIME_PER_HEART= 900;
	private int  focusCounter, pauseCounter;

	public  static double minimizedSeconds;
	void Start () {


		playerHeartCount = PlayerPrefs.GetInt(Constant.HEART,Constant.MAX_HEART);
		if (playerHeartCount ==Constant.MAX_HEART){
			timerText.text=Constant.MAX_HEART+"/"+Constant.MAX_HEART;
			heartCountText.text= "";
			PlayerPrefs.SetString("lasttimeloseheart","");
			PlayerPrefs.Save();
		} else {
			checkHeart();
			if (playerHeartCount < Constant.MAX_HEART) 
				InvokeRepeating ("UpdateTime", 0f, 1f);
			else {
				timerText.text=Constant.MAX_HEART+"/"+Constant.MAX_HEART;
				heartCountText.text= "";
			}
		}
	
	}
	void checkHeart(){
		lastTimeLoseHeart= PlayerPrefs.GetString("lasttimeloseheart","");
		if (!lastTimeLoseHeart.Equals("")){
			dateTimeLast = System.DateTime.Parse (lastTimeLoseHeart);

			totalPassedSeconds = (int)(System.DateTime.Now - dateTimeLast).TotalSeconds;
			if (totalPassedSeconds >= TIME_PER_HEART) {
				//	print ("check energy totalSeconds >SECOND_PER_10GEM " + totalSeconds);
				int ratio = totalPassedSeconds / TIME_PER_HEART;
				playerHeartCount +=ratio;
				dateTimeLast = dateTimeLast.AddMinutes (TIME_PER_HEART/60 * ratio);
				PlayerPrefs.SetString ("lasttimeloseheart", dateTimeLast.ToString ());
				if (playerHeartCount >=Constant.MAX_HEART) {
					playerHeartCount = Constant.MAX_HEART;
					PlayerPrefs.SetString ("lasttimeloseheart", "");
				}
				PlayerPrefs.SetInt (Constant.HEART, playerHeartCount);
				totalPassedSeconds = totalPassedSeconds % TIME_PER_HEART;
				print ("check energy dateString" + lastTimeLoseHeart);
				print ("check energy System.DataeTime.No" + System.DateTime.Now.ToString ());
			}
		} 

	}

	void checkTotalSeconds(){
		if (totalPassedSeconds>=TIME_PER_HEART){
			//	print ("check energy totalSeconds >SECOND_PER_10GEM " +totalSeconds);
			int ratio = totalPassedSeconds/TIME_PER_HEART;
			playerHeartCount +=ratio;
			dateTimeLast = dateTimeLast.AddMinutes (TIME_PER_HEART/60 * ratio);
			PlayerPrefs.SetString("lasttimeloseheart",dateTimeLast.ToString());
			if (playerHeartCount >=Constant.MAX_HEART) {
				playerHeartCount = Constant.MAX_HEART;
				PlayerPrefs.SetString ("lasttimeloseheart", "");
			}
			PlayerPrefs.SetInt (Constant.HEART, playerHeartCount);

			totalPassedSeconds = totalPassedSeconds % TIME_PER_HEART;
		}
	}

	void UpdateTime(){
		//	print ("update time "+ totalSeconds);
		playerHeartCount = PlayerPrefs.GetInt (Constant.HEART, Constant.MAX_HEART);
		heartCountText.text = "" + playerHeartCount;
		totalPassedSeconds= totalPassedSeconds+1;
		if (playerHeartCount >= Constant.MAX_HEART) {
			heartCountText.text = "";
			timerText.text = Constant.MAX_HEART+"/"+Constant.MAX_HEART;
		} else {

			int neededSecond= totalPassedSeconds%TIME_PER_HEART;
			//	print ("neededSeconde "+ neededSecond);

			int int_second=(TIME_PER_HEART - neededSecond)%60;
			int int_minute = (TIME_PER_HEART - neededSecond)/60;
			string string_second=""+int_second;
			string string_minute=""+int_minute;
			if (int_second==0) string_second="00"; 
			else if (int_second<10) string_second="0"+int_second;
			if (int_minute==0) string_minute="00"; 
			else if (int_minute<10) string_minute="0"+int_minute;
			timerText.text = string_minute+":"+string_second;
			checkTotalSeconds();
		}

	
	}

	void OnApplicationPause (bool isGamePause)
	{
		//	print ("pause "+isGamePause);
		if (isGamePause) {
			GoToMinimize ();
		} else {
			//	focusCounter++;
			GoToMaximize();

		}
	}

	void OnApplicationFocus (bool isGameFocus)
	{
		//print("OnApplicationFocus "+ isGameFocus);
		if (isGameFocus) {

		} 
	}
	public void GoToMinimize ()
	{
		//print("lastMinimize");
		lastMinimize = System.DateTime.Now;
		//	PlayerPrefs.SetString("lastTime",lastMinimize.ToString());
	}

	public static void GoToMaximize ()
	{

		//	print("gomaximize");
		minimizedSeconds = (System.DateTime.Now - lastMinimize).TotalSeconds;
		totalPassedSeconds += (int)minimizedSeconds;
		//	print ("total seconds " + totalSeconds);


	}

	// Update is called once per frame
	void Update () {
		
	}

}
