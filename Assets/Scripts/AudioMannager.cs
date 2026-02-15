using System.Collections.Generic;
using UnityEngine;
namespace TwinTracks
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [Header("Audio Sources")]
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource bgmSource;

        [Header("Sound Effects")]
        public AudioClip coinSfx;
        public AudioClip jumpSfx;
        public AudioClip deathSfx;
        public AudioClip levelCompleteSfx;

        [Header("Background Music")]
        public List<AudioClip> bgmClips;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // ---------- SFX ----------
        public void PlaySfx(AudioClip clip)
        {
            if (clip == null || sfxSource == null)
                return;

            sfxSource.PlayOneShot(clip);
        }

        // ---------- BGM ----------
        public void PlayBgm(int levelId)
        {
            if (bgmSource == null || bgmClips == null)
                return;

            Debug.Log($"THE_AUDIO: we are currently got the level id as {levelId}");

            if (!bgmSource.isPlaying && levelId < bgmClips.Count)
            {
                bgmSource.clip = bgmClips[levelId];
                bgmSource.loop = true;
                bgmSource.Play();
            }
        }

        public void StopBgm()
        {
            if (bgmSource != null && bgmSource.isPlaying)
            {
                bgmSource.Stop();
            }
        }

        public void SetBgmVolume(float volume)
        {
            if (bgmSource != null)
            {
                bgmSource.volume = volume;
            }
        }

        public void SetSfxVolume(float volume)
        {
            if (sfxSource != null)
            {
                sfxSource.volume = volume;
            }
        }
    }
}
