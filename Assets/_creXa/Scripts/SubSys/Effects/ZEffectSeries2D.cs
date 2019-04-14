using UnityEngine;
using System.Collections;

namespace creXa.GameBase
{
    [System.Serializable]
    public class ZEffectSeries2D : ZAnimated2D
    {
        public Vector3[] moveRefPoints;
        public Vector3[] effectRefPoints;

        public ZMovement2D[] movements;
        public ZEffect2D[] effects;
        public ZSFx2D[] sfxs;

        public float lifeTime = 1;
        public float refTime = 1;
        public float scale = 1;
        public int layerIdx = 0;

        bool[] effectTriggered;
        bool[] moveTriggered;
        bool[] sfxTriggered;

        public IEnumerator Play()
        {
            if (effectRefPoints == null)
            {
                effectRefPoints = new Vector3[1];
                effectRefPoints[0] = Vector3.zero;
            }

            effectTriggered = new bool[effects.Length];
            moveTriggered = new bool[movements.Length];
            sfxTriggered = new bool[sfxs.Length];

            float timer = 0.0f;
            while (timer < lifeTime)
            {
                for (int i = 0; i < effectTriggered.Length; i++)
                    if (!effectTriggered[i] && effects[i].triggerTime < timer)
                    {
                        effectTriggered[i] = true;
                        PlayEffect(effects[i]);
                    }

                for (int i = 0; i < moveTriggered.Length; i++)
                    if (!moveTriggered[i] && movements[i].triggerTime < timer)
                    {
                        moveTriggered[i] = true;
                        PlayMovement(movements[i]);
                    }

                for (int i = 0; i < sfxs.Length; i++)
                    if (!sfxTriggered[i] && sfxs[i].triggerTime < timer)
                    {
                        sfxTriggered[i] = true;
                        PlaySFX(sfxs[i]);
                    }

                timer += Time.deltaTime;
                yield return null;
            }
        }

        #region Effect

        void PlayEffect(ZEffect2D effect)
        {
            switch (effect.type)
            {
                case ZEffectType2D.ZEffect2D:
                    PlayZEffect2D(effect);
                    return;
                case ZEffectType2D.Image:
                    PlayImage(effect);
                    return;
                case ZEffectType2D.ParticleSystem:
                    PlayParticleSystem(effect);
                    return;
                case ZEffectType2D.Animator:
                    PlayAnimator(effect);
                    return;
                case ZEffectType2D.Animation:
                    PlayAnimation(effect);
                    return;
            }
        }

        void PlayZEffect2D(ZEffect2D effect)
        {
            ZEffectSys2D.It.Play(layerIdx, effect.targetObject, moveRefPoints, effectRefPoints, scale);
        }

        void PlayImage(ZEffect2D effect)
        {
            GameObject tmp = Instantiate(effect.targetObject);
            ZAnimated2D animated = tmp.GetComponent<ZAnimated2D>();
            if (animated == null) return;

            animated.SetParent(transform);
            animated.SetRectScale(Vector3.one);
            animated.SetRectRot(Vector3.zero);
            animated.SetRectPos(effectRefPoints[effect.refPointsIdx]);
            Destroy(tmp, effect.lifeTime);
        }

        void PlayParticleSystem(ZEffect2D effect)
        {
            GameObject tmp = Instantiate(effect.targetObject);
            ZAnimated2D animated = tmp.GetComponent<ZAnimated2D>();
            ParticleSystem sys = tmp.GetComponent<ParticleSystem>();
            if (animated == null || sys == null) return;

            animated.SetParent(transform);
            //animated.SetRectScale(Vector3.one);
            animated.SetRectRot(Vector3.zero);
            animated.SetRectPos(effectRefPoints[effect.refPointsIdx]);

            sys.Play(true);

            Destroy(tmp, effect.lifeTime);
        }

        void PlayAnimator(ZEffect2D effect)
        {
            GameObject tmp = Instantiate(effect.targetObject);
            ZAnimated2D animated = tmp.GetComponent<ZAnimated2D>();
            Animator sys = tmp.GetComponent<Animator>();
            if (animated == null || sys == null) return;

            animated.SetParent(transform);
            animated.SetRectScale(Vector3.one);
            animated.SetRectRot(Vector3.zero);
            animated.SetRectPos(effectRefPoints[effect.refPointsIdx]);

            sys.enabled = true;

            Destroy(tmp, effect.lifeTime);
        }

        void PlayAnimation(ZEffect2D effect)
        {
            GameObject tmp = Instantiate(effect.targetObject);
            ZAnimated2D animated = tmp.GetComponent<ZAnimated2D>();
            Animation sys = tmp.GetComponent<Animation>();
            if (animated == null || sys == null) return;

            animated.SetParent(transform);
            animated.SetRectScale(Vector3.one);
            animated.SetRectRot(Vector3.zero);
            animated.SetRectPos(effectRefPoints[effect.refPointsIdx]);

            sys.Play();

            Destroy(tmp, effect.lifeTime);
        }

        #endregion

        #region Movement

        void PlayMovement(ZMovement2D movement)
        {
            Vector3[] points = new Vector3[movement.refPointsIdx.Length];
            for (int i = 0; i < points.Length; i++)
                points[i] = (movement.refPointsIdx[i] < moveRefPoints.Length ? moveRefPoints[movement.refPointsIdx[i]] : Vector3.zero)
                               + (i < movement.refPointsOffsets.Length ? movement.refPointsOffsets[i] : Vector3.zero);

            switch (movement.type)
            {
                case ZMovementType2D.DiscreteMovement:
                    StartCoroutine(DiscreteMovement(points, movement.lifeTime));
                    return;

                case ZMovementType2D.LinearMovement:
                    StartCoroutine(LinearMovement(points, movement.lifeTime));
                    return;
                case ZMovementType2D.BezierMovement:
                    StartCoroutine(BezierMovement(points, movement.lifeTime));
                    return;
            }
        }

        IEnumerator DiscreteMovement(Vector3[] points, float duration)
        {
            for (int i = 0; i < points.Length; i++)
            {
                SetWorldPos(points[i]);
                yield return new WaitForSeconds(duration / points.Length);
            }

        }

        IEnumerator LinearMovement(Vector3[] points, float duration)
        {
            for (int i = 0; i < points.Length - 1; i++)
                yield return StartCoroutine(WorldPositionTween(points[i], points[i + 1], duration / (points.Length - 1)));
        }

        IEnumerator BezierMovement(Vector3[] points, float duration)
        {
            float timer = 0.0f;
            SetWorldPos(points[0]);
            while (timer < duration)
            {
                timer += Time.deltaTime;
                SetWorldPos(ZGeo.CalculateBezierPoint(timer / duration, points));
                yield return null;
            }
            SetWorldPos(points[points.Length - 1]);
        }

        #endregion

        #region SFX

        void PlaySFX(ZSFx2D sfx)
        {
            ZAudio2D.It.PlaySFX(sfx.clip);
        }

        #endregion

    }
}
