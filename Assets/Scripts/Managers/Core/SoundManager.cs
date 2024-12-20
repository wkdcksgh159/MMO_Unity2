using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount]; // 타입별 오디오 소스를 받을 배열 선언

    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>(); // 오디오 소스를 캐싱하기 위한 Dictionary

    // MP3 Player   -> AudioSource
    // MP3 음원?    -> AudioClip
    // 관객(귀)     -> AudioListener

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound"); // @Sound 오브젝트를 찾는다
        if (root == null) // @Sound 오브젝트가 없으면
        {
            root = new GameObject { name = "@Sound" }; // @Sound 오브젝트를 생성한다
            Object.DontDestroyOnLoad(root); // @Sound 오브젝트가 씬이 바뀌어도 삭제되지 않도록 한다

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound)); // 사운드 타입을 불러온다
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] }; // 각 타입의 오브젝트를 생성하고
                _audioSources[i] = go.AddComponent<AudioSource>(); // 컴포넌트를 추가한다
                go.transform.parent = root.transform; // 해당 컴포넌트의 부모로 @Sound 지정
            }

            _audioSources[(int)Define.Sound.Bgm].loop = true; // Bgm 은 Loop 지정
        }
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear();
    }

    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f) // AudioClip, 음원타입, 재생속도
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type); // path 경로의 AudioClip을 가져오거나 생성
        Play(audioClip, type, pitch); // AudioClip 을 인자로 받는 Play 호출
    }

    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f) // 경로, 음원타입, 재생속도
    {
        if (audioClip == null) // audioClip 이 null 이면 종료
            return;
        if (type == Define.Sound.Bgm) // Bgm 인 경우
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm]; // Bgm 파일을 불러온다
            if (audioSource.isPlaying) // 객체를 새로 생성해서 해당 코드는 없어도 되지만 명확하게 하기 위해
                audioSource.Stop();

            audioSource.pitch = pitch; // pitch 지정
            audioSource.clip = audioClip; // AudioClip 지정
            audioSource.Play(); // 재생
        }
        else // Bgm 이 아닌 경우
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect]; // Effect 파일을 불러온다
            audioSource.pitch = pitch; // pitch 지정
            audioSource.PlayOneShot(audioClip); // 한 번 재생한다
        }
    }

    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect) // AudioClip 을 가져오거나 생성한다
    {
        if (path.Contains("Sounds/") == false) // 음원 경로 지정
            path = $"Sounds/{path}";

        AudioClip audioClip = null; // 리턴할 AudioClip 

        if (type == Define.Sound.Bgm) // Bgm 인 경우
        {
            audioClip = Managers.Resource.Load<AudioClip>(path); // 해당 리소스를 불러온다
        }
        else // Bgm 이 아닌 경우
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false) // Dictionary 에 path 키값을 가진 audioClip 이 없다면
            {
                audioClip = Managers.Resource.Load<AudioClip>(path); // audioClip 을 가져온다
                _audioClips.Add(path, audioClip); // Dic에 추가
            }
        }

        if (audioClip == null) // 음원이 없는 경우 종료
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip; // AudioClip 리턴 
    }
}
