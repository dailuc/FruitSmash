using UnityEngine;
using System.Collections;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Advertisements;
public class GoogleAdmob
{
    public static bool canRequestFullAds = false;
    public static bool adRequested = false;
    public static BannerView _bannerView;
    public static InterstitialAd interstitialAd;


    public delegate void OnClosetInter();
    public static event OnClosetInter onCloseInterAds;


    public static bool loadAdmobRewardFailed = false;

    public static string bannerID = "ca-app-pub-3940256099942544/6300978111";
    public static string interID = "ca-app-pub-3940256099942544/1033173712";


    public static string bannerID2 = "ca-app-pub-3940256099942544/6300978111";
    public static string interID2 = "ca-app-pub-3940256099942544/1033173712";


    public static string bannerID3 = "ca-app-pub-3940256099942544/6300978111";
    public static string interID3 = "ca-app-pub-3940256099942544/1033173712";



    public static string lastInterID = "";

    public static int bannerLoadCount = 0;
    public static int interLoadCount = 0;


    public static float lastTimeShowAd = -30;



    public static void init()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            if (initStatus == null)
            {
                Debug.LogError("Google Mobile Ads initialization failed.");
                return;
            }
            Debug.Log("Google Mobile Ads initialization complete.");
            LoadAd(bannerID);
            LoadInterstitialAd(interID);
        });
    }


    #region Banner  handlers
    public static void DestroyBannerAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner ad.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    /// <summary>
    /// Creates a 320x50 banner at top of the screen.
    /// </summary>
    public static void CreateBannerView(string _adUnitId)
    {
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            DestroyBannerAd();
        }

        // Create a 320x50 banner at top of the screen
        _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Bottom);
        ListenToAdEvents();
        Debug.Log("Banner view created.");
    }
    /// <summary>
    /// Creates the banner view and loads a banner ad.
    /// </summary>
    public static void LoadAd(string bannerAdID)
    {
        // create an instance of a banner view first.
        if (_bannerView == null)
        {
            CreateBannerView(bannerAdID);
        }
        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("bt42");

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }

    public static void showBanner(bool isShow)
    {
        //     if (MyUtil.isPurchaseRemoveAds()) return;
        if (isShow) _bannerView.Show();
        else _bannerView.Hide();
    }


    /// <summary>
    /// listen to events the banner may raise.
    /// </summary>
    private static void ListenToAdEvents()
    {
        // Raised when an ad is loaded into the banner view.
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + _bannerView.GetResponseInfo());
            bannerLoadCount = 0;
            showBanner(true);
        };
        // Raised when an ad fails to load into the banner view.
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
            bannerLoadCount++;
            if (bannerLoadCount == 1)
            {
                LoadAd(bannerID2);
            }
            else if (bannerLoadCount == 2)
            {
                LoadAd(bannerID3);
            }

        };
        // Raised when the ad is estimated to have earned money.
        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    #endregion

    #region Interstitial handlers

    /// <summary>
    /// Loads the interstitial ad.
    /// </summary>
    public static void LoadInterstitialAd(string _adUnitId)
    {
        // Clean up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }
        lastInterID = _adUnitId;
        if (lastInterID == interID) interLoadCount = 0;

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("bt42");
        GoogleAdmob.canRequestFullAds = false;
        adRequested = true;
        // send the request to load the ad.
        InterstitialAd.Load(lastInterID, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                    "with error : " + error);
                    if (lastInterID == interID || lastInterID == interID2 || lastInterID == interID3)
                    {
                        Debug.Log("Inter Ads Load Failed Count " + interLoadCount);
                        interLoadCount++;
                        if (interLoadCount == 1)
                        {
                            LoadInterstitialAd(interID2);
                        }
                        else if (interLoadCount == 2)
                        {
                            LoadInterstitialAd(interID3);
                        }
                    }

                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());
                interLoadCount = 0;

                interstitialAd = ad;
                RegisterEventHandlers(ad);
            });
    }
    /// <summary>
    /// Shows the interstitial ad.
    /// </summary>
    public static void ShowInterAd()
    {

        // test
        //if (Application.isEditor)
        //{
        //    onCloseInterAds?.Invoke();
        //    return;
        //}


        //
        if (Time.realtimeSinceStartup - lastTimeShowAd > 30)
        {
            adRequested = false;
            if (interstitialAd != null && interstitialAd.CanShowAd())
            {
                Debug.Log("Showing interstitial ad.");
                interstitialAd.Show();
                lastTimeShowAd = Time.realtimeSinceStartup;
                canRequestFullAds = true;
            }
            else
            {
                Debug.LogError("Interstitial ad is not ready yet.");
                onCloseInterAds?.Invoke();
            }

        }
        else
        {

            onCloseInterAds?.Invoke();
        }



    }

    private static void RegisterEventHandlers(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");

        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            onCloseInterAds?.Invoke();
            LoadInterstitialAd(lastInterID);
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }



    #endregion

    #region Reward Ad handlers

    #endregion
}