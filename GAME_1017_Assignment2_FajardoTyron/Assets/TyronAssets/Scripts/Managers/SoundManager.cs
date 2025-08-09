using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Music")]
    public AudioSource titleMusic;
    public AudioSource gameMusic;
    public AudioSource gameOverMusic;

    [Header("SFX")]
    public AudioSource sfxSource;
    public AudioClip jumpSfx;
    public AudioClip rollSfx;
    public AudioClip hitSfx;

    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    private void Start()
    {
        ApplyVolumes();
    }

    public void PlayTitleMusic()
    {
        StopAllMusic();
        if (titleMusic) { titleMusic.loop = true; titleMusic.Play(); }
    }

    public void PlayGameMusic()
    {
        StopAllMusic();
        if (gameMusic) { gameMusic.loop = true; gameMusic.Play(); }
    }

    public void PlayGameOverMusic()
    {
        StopAllMusic();
        if (gameOverMusic) { gameOverMusic.loop = false; gameOverMusic.Play(); }
    }

    private void StopAllMusic()
    {
        if (titleMusic) titleMusic.Stop();
        if (gameMusic) gameMusic.Stop();
        if (gameOverMusic) gameOverMusic.Stop();
    }

    public void PlayJump() => PlaySfx(jumpSfx);
    public void PlayRoll() => PlaySfx(rollSfx);
    public void PlayHit() => PlaySfx(hitSfx);

    public void PlaySfx(AudioClip clip)
    {
        if (sfxSource == null || clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void SetMusicVolume(float v)
    {
        musicVolume = Mathf.Clamp01(v);
        ApplyVolumes();
    }

    public void SetSfxVolume(float v)
    {
        sfxVolume = Mathf.Clamp01(v);
        ApplyVolumes();
    }

    private void ApplyVolumes()
    {
        if (titleMusic) titleMusic.volume = musicVolume;
        if (gameMusic) gameMusic.volume = musicVolume;
        if (gameOverMusic) gameOverMusic.volume = musicVolume;
        if (sfxSource) sfxSource.volume = sfxVolume;
    }
}
