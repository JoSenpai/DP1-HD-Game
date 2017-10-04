using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioClip ArrowAudioClip, DeathSoundAudioClip, BlackCarrotAudioClip, GolemDeath, 
		BugDeath, FireBall, IcedArrow, NormalUpgrade, GlobalUpgrade, FailUpgrade, BunnySound, DragonSound, RiderSound;

    /// <summary>
    /// Basic singleton implementation
    /// </summary>
    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void PlayArrowSound()
    {
        StartCoroutine(PlaySound(ArrowAudioClip));
    }

	public void PlayFireballSound()
	{
		StartCoroutine (PlaySound(FireBall));
	}

	public void PlayIcedArrowSound()
	{
		StartCoroutine (PlaySound(IcedArrow));
	}

    public void PlayDeathSound()
    {
        StartCoroutine(PlaySound(DeathSoundAudioClip));
    }

	public void PlayBlackCarrotSound()
	{
		StartCoroutine(PlaySound(BlackCarrotAudioClip));
	}

	public void PlayDeathGolem()
	{
		StartCoroutine(PlaySound(GolemDeath));
	}

	public void PlayDeathBug()
	{
		StartCoroutine(PlaySound(BugDeath));
	}

	public void PlayNormalUpgrade()
	{
		StartCoroutine (PlaySound (NormalUpgrade));
	}

	public void PlayGlobalUpgrade()
	{
		StartCoroutine (PlaySound (GlobalUpgrade));
	}

	public void PlayFailUpgrade()
	{
		StartCoroutine (PlaySound (FailUpgrade));
	}

	public void PlayBunnyClick()
	{
		StartCoroutine (PlaySound (BunnySound));
	}

	public void PlayDragonClick()
	{
		StartCoroutine (PlaySound (DragonSound));
	}

	public void PlayRiderClick()
	{
		StartCoroutine (PlaySound (RiderSound));
	}
    //coroutine is used since we also want to deactivate it after the sound is played
    private IEnumerator PlaySound(AudioClip clip)
    {
        //get an object from the pooler, activate it, play the sound
        //wait for sound completion and then deactivate the object
        GameObject go = ObjectPoolerManager.Instance.AudioPooler.GetPooledObject();
        go.SetActive(true);
        go.GetComponent<AudioSource>().PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        go.SetActive(false);
    }
}
