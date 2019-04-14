using UnityEngine;
using System.Collections;

namespace creXa.GameBase
{
    [AddComponentMenu("creXa/Effect/Effect System 2D")]
    public class ZEffectSys2D : ZSingleton<ZEffectSys2D>
    {
        const float SAFEINTERVAL = 0.1f;

        public Transform[] EffectLayers;
        
        public float Play(int layer, GameObject obj, Vector3[] moveRefPoints, Vector3[] effectRefPoints = null, float scale = 1.0f, float duration = -1.0f)
        {
            if (layer < 0 || !obj || moveRefPoints.Length <= 0) { return 0; }

            GameObject tmp = Instantiate(obj);
            ZEffectSeries2D series = tmp.GetComponent<ZEffectSeries2D>();
            if (!series) { DestroyImmediate(tmp); return 0; }
            
            series.SetParent(EffectLayers[layer]);
            series.SetRectScale(Vector3.one * scale);
            series.SetRectRot(Vector3.zero);
            series.SetWorldPos(moveRefPoints[0]);

            series.moveRefPoints = moveRefPoints;
            series.scale = scale;
            series.layerIdx = layer;

            if (effectRefPoints != null) series.effectRefPoints = effectRefPoints;
            if (duration > 0) series.lifeTime = duration;

            StartCoroutine(series.Play());
            Destroy(tmp, series.lifeTime + SAFEINTERVAL);
            return series.refTime;
        }

    }

}
