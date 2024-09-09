using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace FGSBlackJack
{
    public class CallBreakSoundManager : MonoBehaviour
    {
        public static System.Action<string> PlaySoundEvent;
        public static System.Action PlayVibrationEvent;

        public AudioClip[] allAudioClips;


        [Space(10)]
        public AudioSource audioSourceBackGround;
        public AudioSource soundSource;
        [Space(5)]

        public Image soundImage;
        public Image musicImage;
        public Image vibrationImage;
        public Sprite onBtnSprite, offBtnSprite;

        public List<AudioClip> audioClips; // List of AudioClips

        public AudioClip ReturnAudioClip(string audioClipName)
        {
            // Iterate through the list of AudioClips
            foreach (AudioClip clip in audioClips)
            {
                // Check if the name of the AudioClip matches the requested name
                if (clip.name == audioClipName)
                {
                    return clip; // Return the found AudioClip
                }
            }

            // If no matching AudioClip is found, return null or handle it as needed
            Debug.LogWarning($"AudioClip with name '{audioClipName}' not found.");
            return null;
        }

        private void OnEnable()
        {
            PlaySoundEvent += PlaySoundEffect;
            PlayVibrationEvent += PlayVibration;
        }
        private void OnDisable()
        {
            PlaySoundEvent -= PlaySoundEffect;
            PlayVibrationEvent -= PlayVibration;
        }
        private void Start() => ChangeSprite();

        public void PlaySoundEffect(string soundEffects)
        {
            soundSource.PlayOneShot(ReturnAudioClip(soundEffects));
        }

        public void PlayBGSoundEffect()
        {
            audioSourceBackGround.Play();
        }

        public void SoundBtnClick()
        {
            BtnClickSound();

            CallBreakConstants.IsSound = !CallBreakConstants.IsSound;
            if (CallBreakConstants.IsSound)
            {
                soundImage.sprite = onBtnSprite;
                soundSource.mute = false;
                soundSource.enabled = false;
            }
            else
            {
                soundImage.sprite = offBtnSprite;
                soundSource.enabled = true;
            }

            ChangeSprite();
        }

        public void PlayVibration()
        {
#if !UNITY_WEBGL
            if (CallBreakConstants.IsVibration)
            {
                Handheld.Vibrate();
            }
#endif


        }

        public void MusicBtnClick()
        {
            BtnClickSound();
            CallBreakConstants.IsMusic = !CallBreakConstants.IsMusic;

            if (CallBreakConstants.IsMusic)
            {
                musicImage.sprite = onBtnSprite;
                audioSourceBackGround.mute = false;
                audioSourceBackGround.enabled = false;
                audioSourceBackGround.enabled = true;
            }
            else
            {
                musicImage.sprite = offBtnSprite;
                audioSourceBackGround.mute = true;
                audioSourceBackGround.enabled = false;
            }

            Debug.Log("MusicBtnClick " + audioSourceBackGround.mute);
            ChangeSprite();
        }

        public void VibrationBtnClick()
        {
            BtnClickSound();

            CallBreakConstants.IsVibration = !CallBreakConstants.IsVibration;

            if (CallBreakConstants.IsVibration)
                vibrationImage.sprite = onBtnSprite;
            else
                vibrationImage.sprite = offBtnSprite;

            ChangeSprite();
        }

        internal void ChangeSprite()
        {
            if (CallBreakConstants.IsSound)
            {
                soundImage.sprite = onBtnSprite;
                soundSource.mute = false;
                soundSource.enabled = true;
            }
            else
            {
                soundImage.sprite = offBtnSprite;
                soundSource.mute = true;
                soundSource.enabled = false;
            }

            if (CallBreakConstants.IsMusic)
            {
                musicImage.sprite = onBtnSprite;
                audioSourceBackGround.mute = false;
                audioSourceBackGround.enabled = true;
            }
            else
            {
                musicImage.sprite = offBtnSprite;
                audioSourceBackGround.mute = false;
                audioSourceBackGround.enabled = false;
            }

            if (CallBreakConstants.IsVibration)
                vibrationImage.sprite = onBtnSprite;
            else
                vibrationImage.sprite = offBtnSprite;


        }

        public void BtnClickSound()
        {
            PlaySoundEvent("Click");
        }
    }
}
