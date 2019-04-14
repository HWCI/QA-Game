using UnityEngine;
using System.Collections;

namespace creXa.GameBase
{
    [AddComponentMenu("creXa/Camera-Control/UICamera")]
    public class ZUICamera : MonoBehaviour
    {
        public void Tune3DCameraRatio(float resolution)
        {
            Camera Cam = gameObject.GetComponent<Camera>();

            float ratio = resolution / Cam.aspect;
            if (ratio < 1)
            {
                Cam.rect = new Rect((1 - ratio) / 2f, 0, ratio, 1);
            }
            else if (ratio > 1)
            {
                ratio = Cam.aspect / (resolution);
                Cam.rect = new Rect(0, (1 - ratio) / 2f, 1, ratio);
            }
        }
    }
}
