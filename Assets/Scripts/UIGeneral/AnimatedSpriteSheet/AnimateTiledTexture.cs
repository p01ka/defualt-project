using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UIGeneral.AnimatedSpriteSheet
{
    class AnimateTiledTexture : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        private float framesPerSecond = 1f;

        [SerializeField]
        private Image image;

        [SerializeField]
        private Sprite[] _preloaderSprites;

        [SerializeField]
        private float _delayBeforeAnimation;

        [SerializeField]
        private float _delayBetweenAnimations;
#pragma warning restore 0649

        private float elapsedTime;
        private float timePerFrame;
        private int currentFrame;

        private float _timeAfterEnabled;
        private float _delayCounter;

        private void Start()
        {
            timePerFrame = 1f / framesPerSecond;
        }

        private void OnEnable()
        {
            _timeAfterEnabled = 0;
            _delayCounter = _delayBeforeAnimation;
            currentFrame = 0;
            elapsedTime = 0;
            SetSprite();
        }

        private void Update()
        {
            if (_timeAfterEnabled <= _delayBeforeAnimation)
            {
                _timeAfterEnabled += Time.unscaledDeltaTime;
                return;
            }

            if (_delayCounter < _delayBetweenAnimations)
            {
                _delayCounter += Time.unscaledDeltaTime;
            }

            timePerFrame = 1f / framesPerSecond;
            elapsedTime += Time.unscaledDeltaTime;

            if (elapsedTime >= timePerFrame)
            {
                ++currentFrame;
                SetSprite();

                if (currentFrame >= _preloaderSprites.Length)
                {
                    currentFrame = 0;
                    _delayCounter = 0;
                }
                elapsedTime = 0;
            }
        }

        private void SetSprite()
        {
            int spriteIndex = Mathf.Clamp(currentFrame, 0, _preloaderSprites.Length - 1);
            image.sprite = _preloaderSprites[spriteIndex];
        }
    }
}