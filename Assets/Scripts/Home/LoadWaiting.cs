using UnityEngine;
using System.Collections;

public class LoadWaiting : MonoBehaviour
{
    public static string Id1 = "";
    public static string Id2 = "";
    public static string Id3 = "";
    public static string Id4 = "";

    public UnityEngine.UI.Image loadbar;    // Image loading fake

    /// <summary>
    /// fill image by second and go to Home scene
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        //	PlayerPrefs.DeleteAll ();
        GoogleAdmob.init();
        for (int i = 0; i < 100; i++)
        {
            loadbar.fillAmount += 1 / 100f;
            yield return new WaitForEndOfFrame();
        }
       
        Application.LoadLevel("HomeScene");
    }
}
