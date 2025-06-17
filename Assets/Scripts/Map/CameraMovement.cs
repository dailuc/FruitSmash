using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System.Collections.Generic;
using System;
public class CameraMovement : MonoBehaviour
{
  

	List<string> perms = new List<string>(){"public_profile", "email", "user_friends"};

	public GameObject fbLoginButton,fbLogoutButton,userAvatarGroup;

	public GameObject avatarPrefab;

	public Texture2D khungavatar;
    public static CameraMovement mcamera;       // camera movement

    public static int StarPointMoveIndex;       // position index

    public RectTransform container;             // container of scroll view

    public GameObject PopUp;                    // popup show when click to item button level
	public GameObject recoverPopUp;    
    public GameObject StarPoint;                // position start

    public Sprite[] star;                       // arrays star of item level

    public GameObject fade;                     // fade animation

    float distance = 90.8f / 8680f;

    public static bool movement;

    public static bool setstate;

    public bool isPopup;

    public GameObject RateObj;


    Player map;


    void Awake()
    {
        mcamera = this;
    }

    void Start()
    {
        setLastpos();
        SetPoint();
	
		//if (FB.IsLoggedIn)
		//	querryScore();
    }
	public void setFacebookButtonStatus(){
		fbLoginButton.SetActive(false);
		//if (FB.IsLoggedIn) {
		//	fbLogoutButton.SetActive(true);
		//	fbLoginButton.SetActive(false);
		//} else {
		//	fbLogoutButton.SetActive(false);
		//	fbLoginButton.SetActive(true);
		//}
	}
    void Update()
    {
	//	Debug.Log(""+ DataLoader.enableclick);
        if (Input.GetKeyDown(KeyCode.Escape) && isPopup)
        {
            UnfreezeMap();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ButtonActionController.Click.HomeScene();
        }
    }

    /// <summary>
    /// set last position of container
    /// </summary>
    void setLastpos()
    {
        float lastp = PlayerPrefs.GetFloat("LASTPOS", 0);
        if (lastp < 0) lastp = 0;
        else if (lastp > 90.8000f) lastp = 90.8f;
        transform.position += new Vector3(0, lastp);
        container.anchoredPosition = new Vector2(container.anchoredPosition.x, -lastp / distance + 4740f);
    }

    void SetPoint()
    {
        float x = PlayerPrefs.GetFloat("LASTPOSX", -0.0045f);
        float y = PlayerPrefs.GetFloat("LASTPOS", -3.587f);
        StarPoint.transform.position = new Vector3(x, y, StarPoint.transform.position.z);

    }

    /// <summary>
    /// Update positio camera when scroller
    /// </summary>
    public void CameraPosUpdate()
    {
        transform.position = new Vector3(transform.position.x, -(container.anchoredPosition.y - 4740f) * distance, transform.position.z);
        if (setstate)
            movement = true;
    }


    /// <summary>
    /// show infomation of level player
    /// </summary>
    /// <param name="_map"></param>
    public void PopUpShow(Player _map)
    {
        isPopup = true;
        CameraMovement.mcamera.FreezeMap();
        map = _map;
        Image[] stars = new Image[3];
        stars[0] = PopUp.transform.GetChild(1).GetComponent<Image>();
        stars[1] = PopUp.transform.GetChild(2).GetComponent<Image>();
        stars[2] = PopUp.transform.GetChild(3).GetComponent<Image>();


        for (int i = 0; i < 3; i++)
        {
            if (i < _map.Stars)
                stars[i].sprite = star[0];
            else
                stars[i].sprite = star[1];
        }
        PopUp.transform.GetChild(4).GetComponent<Text>().text = _map.HightScore.ToString();
        PopUp.transform.GetChild(6).GetComponent<Text>().text = _map.Level.ToString("00");
        Animation am = PopUp.GetComponent<Animation>();
        am.enabled = true;
        PopUp.SetActive(true);
    }

    public void ArcadeScene()
    {
		int playerHeartCount = PlayerPrefs.GetInt(Constant.HEART,Constant.MAX_HEART);
		if (playerHeartCount<=0) {
			showPopupPurchaseHeart();
		} else {
			ButtonActionController.Click.ArcadeScene(map);
		}
        
    }
	public void purchaseHearts(){
		IAPManager.instance.purchaseHearts();
	}
	public void showPopupPurchaseHeart(){
		SoundController.Sound.Click();
		Animation am = recoverPopUp.GetComponent<Animation>();
		am.enabled = true;
		recoverPopUp.SetActive(true);
	}
    public void FreezeMap()
    {
        DataLoader.enableclick = false;
        fade.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void UnfreezeMap()
    {
        SoundController.Sound.Click();
        PopUp.SetActive(false);
        isPopup = false;
        DataLoader.enableclick = true;
        fade.GetComponent<CanvasGroup>().blocksRaycasts = false;

    }
	public void UnfreezeMapAndCloseRecoverDialog()
	{
		SoundController.Sound.Click();
		recoverPopUp.SetActive(false);
		isPopup = false;
		DataLoader.enableclick = true;
		fade.GetComponent<CanvasGroup>().blocksRaycasts = false;

	}
	void OnEnable(){
		IAPManager.onPurchasePack+=purchaseSuccess;


	}
	void OnDisable(){
		IAPManager.onPurchasePack-=purchaseSuccess;


	}
	void purchaseSuccess(){
		UnfreezeMapAndCloseRecoverDialog();
	}
    public void ShowRatePopup()
    {
        RateObj.SetActive(true);
        PlayerPrefs.SetInt("LevelShowRate",0);
        LeanTween.scale(RateObj.transform.GetChild(0).gameObject,new Vector3(1.3f,1.3f),0.8f);
        DataLoader.enableclick = true;
        fade.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnRateClick()
    {
#if UNITY_ANDROID
		Application.OpenURL("https://play.google.com/store/apps/details?id=" +  "");
#elif UNITY_IPHONE
        Application.OpenURL("itms-apps://itunes.apple.com/app/" + Application.bundleIdentifier + "");
#endif
    }

    public void OnCloseRate()
    {
        SoundController.Sound.Click();
        RateObj.SetActive(false);
        RateObj.transform.GetChild(0).localScale = Vector3.zero;
    }
	public void loginFacebook(){
		//FB.LogInWithReadPermissions(perms, AuthCallback);
	
	}
	public void logoutFacebook(){
		//FB.LogOut();
		setFacebookButtonStatus();
	}
	//private void AuthCallback (ILoginResult result) {
	//	setFacebookButtonStatus();
	//	if (FB.IsLoggedIn) {
	//		// AccessToken class will have session details
	//		var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
	//		// Print current access token's User ID
	//		Debug.Log(aToken.UserId);
	//		// Print current access token's granted permissions
	//		foreach (string perm in aToken.Permissions) {
	//			Debug.Log(perm);
	//		}
	//		//setScore("7");
	//		querryScore();
	//	} else {
	//		Debug.Log("User cancelled login");
	//	}
	//}
	public void inviteFacebook(){
		//if (FB.IsLoggedIn){
		//	string gameURL = "https://play.google.com/store/apps/details?id="  ;
		//	#if UNITY_ANDROID 
		//	gameURL="https://play.google.com/store/apps/details?id="  ; 
		//	#endif

		//	FB.AppRequest("Come and play "+gameURL,null, new List<object>() { "app_users" },null,null,null,null, AppInviteCallback);
		
		//} else {
		//	loginFacebook();

		//}

	}
	//private void AppInviteCallback(IAppRequestResult result){
	//	Debug.Log("AppInviteCallback "+ result);
	
	//}
	//public void querryScore(){
	
	//	FB.API("/app/scores?fields=score,user.limit(30)",HttpMethod.GET,getScoreCallBack);
	//}
	//void getScoreCallBack(IResult result){

	//	IDictionary<string,object> data = result.ResultDictionary;
	//	List<object> scorelist= (List<object>) data["data"];
	//	foreach (object obj in scorelist){
	//		var entry = (Dictionary<string, object>) obj;
	//		var user = (Dictionary<string , object>)entry ["user"];
	//		if (user["id"].ToString().Equals(""+ Facebook.Unity.AccessToken.CurrentAccessToken.UserId)) continue;
	//		Debug.Log(user["name"].ToString() + "," + entry["score"].ToString());
	//		int friendscore= int.Parse(entry["score"].ToString());
	//		GameObject userAvatar = Instantiate(avatarPrefab) as GameObject;
	//		userAvatar.transform.position = new Vector3(DataLoader.Data.mappos[friendscore-1].x+0.2f,DataLoader.Data.mappos[friendscore-1].y,0);

	//		userAvatar.transform.SetParent(userAvatarGroup.transform,false);

	//		Transform avatar= userAvatar.transform.Find("avatar");
	//		SpriteRenderer imgAvatar = avatar.GetComponent<SpriteRenderer>();
	//		SpriteRenderer imgKhung = userAvatar.transform.Find("bg").GetComponent<SpriteRenderer>();

	//		FB.API(user["id"].ToString()+"/picture?width=48&height=48",HttpMethod.GET,delegate (IGraphResult picresult){
	//			if (picresult.Error!=null){
				
	//			} else {
	//				imgAvatar.sprite = Sprite.Create(picresult.Texture,new Rect(0,0,48,48),new Vector2(0,0));
	//				imgKhung.sprite = Sprite.Create(khungavatar,new Rect(0,0,50,50),new Vector2(0,0));
	//			}
	//		} );


	//	}
	//}

	//public void setScore(string mscore){
	//	var scoredata = new Dictionary<string,string> ();
	//	scoredata["score"] = mscore;

	//	FB.API("/me/scores",HttpMethod.POST,delegate(IGraphResult result) {
	//		Debug.Log("Send score successfully "+ result.RawResult);
	//	}, scoredata);
		
	//}



}
