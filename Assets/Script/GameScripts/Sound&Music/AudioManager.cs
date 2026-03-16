using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Background clip ----------")]
    public AudioClip background;

    [Header("---------- Player clip ----------")]
    public AudioClip PlayerAttack1;
    public AudioClip PlayerAttack2;
    public AudioClip PlayerAttack3;
    public AudioClip PlayerDash;
    public AudioClip PlayerDead;
    public AudioClip PlayerHit;
    public AudioClip PlayerJump;

    [Header("---------- Boss clip ----------")]
    public AudioClip BossAttack1;
    public AudioClip BossAttack2;
    public AudioClip BossAttack3;
    public AudioClip BossJumpAttack;
    public AudioClip BossAttack4;
    public AudioClip BossDead;
    public AudioClip BossTeleport;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlayerSFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
    public void BossSFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
