using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UnityDevTools.Common
{
    /// <summary>
    /// CoroutineUtility.
    /// </summary>
    public static class CoroutineUtility
    {
        public static IEnumerator LerpCoroutine(Action<float> action, float animationTime)
        {
            var startTime = Time.time;
            var endTime = startTime + animationTime;

            while (endTime >= Time.time)
            {
                yield return null;
                var currentPersent = (Time.time - startTime) / animationTime;
                action(currentPersent);
            }
        }

        public static IEnumerator WaitForClickButton(this Button button)
        {
            var isClicked = false;
            void OnButtonClicked()
            {
                isClicked = true;
                button.onClick.RemoveListener(OnButtonClicked);
            }

            button.onClick.AddListener(OnButtonClicked);

            while (!isClicked)
            {
                yield return null;
            }
        }

        public static IEnumerator PlayAnimation(Animator animator, string clipName, float time, bool forceInitialPose = true, bool isReverse = false, int layer = 0)
        {
            if (forceInitialPose)
            {
                animator.Play(clipName, layer, isReverse ? 1 : 0);
            }

            void AnimationAction(float t)
            {
                animator.Play(clipName, layer, isReverse ? 1 - t : t);
            }

            return LerpCoroutine(AnimationAction, time);
        }

        public static IEnumerator WaitForClick()
        {
            while (!Input.GetMouseButtonDown(0))
            {
                yield return null;
            }

            while (!Input.GetMouseButtonUp(0))
            {
                yield return null;
            }
        }

    }
}