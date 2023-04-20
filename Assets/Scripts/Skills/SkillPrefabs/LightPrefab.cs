using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RythmGame
{
    public class LightPrefab : MonoBehaviour
    {
        public float _startupTime;
        public float _windDownTime;
        public float _skillDuration;
        public float _easingFactor;

        private float _remainingDuration;

        Material _material;

        private void Start()
        {
            Renderer renderer = GetComponent<Renderer>();
            _material = renderer.material;
            StartCoroutine(LightTimer());
        }

        private IEnumerator LightTimer()
        {
            // Increase alpha from 0 to 1 over startupTime seconds
            float elapsedTime = 0f;
            while (elapsedTime < _startupTime)
            {
                float alpha = EaseInOut(elapsedTime / _startupTime);
                alpha = Mathf.Clamp01(alpha);
                _material.color = new Color(_material.color.r, _material.color.g, _material.color.b, alpha);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Keep alpha at 1 for skillDuration seconds, but allow it to be extended
            _remainingDuration = _skillDuration;
            while (_remainingDuration > 0f)
            {
                float alpha = 1f;
                _material.color = new Color(_material.color.r, _material.color.g, _material.color.b, alpha);

                // Update the remaining duration and wait for the next frame
                _remainingDuration -= Time.deltaTime;
                yield return null;
            }


            // Decrease alpha from 1 to 0 over skillDuration seconds
            elapsedTime = 0f;
            while (elapsedTime < _windDownTime)
            {
                float alpha = EaseInOut(1f - (elapsedTime / _windDownTime));
                alpha = Mathf.Clamp01(alpha);
                _material.color = new Color(_material.color.r, _material.color.g, _material.color.b, alpha);
                elapsedTime += Time.deltaTime;

                // Check if we need to ramp back up to 1 alpha and keep it there for a new duration
                while (_remainingDuration > 0f)
                {
                    // Increase alpha from current value to 1 over the remaining windDownTime
                    float remainingWindDownTime = _windDownTime - elapsedTime;
                    while (elapsedTime < _windDownTime && _remainingDuration > 0f)
                    {
                        alpha = EaseInOut((elapsedTime - _windDownTime) / remainingWindDownTime + 1f);
                        alpha = Mathf.Clamp01(alpha);
                        _material.color = new Color(_material.color.r, _material.color.g, _material.color.b, alpha);
                        elapsedTime += Time.deltaTime;

                        yield return null;
                    }
                }

                yield return null;
            }

            // Set alpha to 0
            _material.color = new Color(_material.color.r, _material.color.g, _material.color.b, 0f);

            Destroy(this.gameObject);
        }

        public void SetRemainingDuration(float newDuration)
        {
            _remainingDuration = newDuration;
        }

        private float EaseInOut(float t)
        {
            // Apply ease-in-out function to t
            return (3f - 2f * Mathf.Pow(1f - t, _easingFactor)) * t * t;
        }
    }
}
