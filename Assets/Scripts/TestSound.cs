using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSound : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public AudioClip audioClip; // Unity에서 AudioClip 을 입력받는다
    public AudioClip audioClip2;

    int i = 0;
    private void OnTriggerEnter(Collider other)
    {
        //AudioSource audio = GetComponent<AudioSource>();
        //audio.PlayClipAtPoint(); // 특정 월드좌표 위치에 AudioClip을 재생한다

        /*AudioSource audio = GetComponent<AudioSource>(); // AudioSource 컴포넌트를 불러온다
        audio.PlayOneShot(audioClip); // 한번 Play
        audio.PlayOneShot(audioClip2); // 먼저 실행한 PlayOneShot의 종료여부에 상관없이 재생된다
        float lifeTime = Mathf.Max(audioClip.length, audioClip2.length); // 더 긴 AudioClip이 끝날시간 구하기
        GameObject.Destroy(gameObject, lifeTime); // 객체 제거*/


        i++;
        if (i % 2 == 0) // 트리거 실행마다 다른 음원을 재생한다
            Managers.Sound.Play(audioClip, Define.Sound.Bgm); // 효과음, univ0001 음원파일 실행
        else
            Managers.Sound.Play(audioClip2, Define.Sound.Bgm); // 효과음, univ0002 음원파일 실행
    }
}
