using UnityEngine;
using System.Collections;

public class glass : MonoBehaviour
{

    public Animator amin;
    public RectTransform curTransform;
    /// <summary>
    /// disable animator
    /// </summary>
    public void DisableAnimator()
    {
        curTransform.localRotation = new Quaternion(0f,0f,0f,0f);
    }
}
