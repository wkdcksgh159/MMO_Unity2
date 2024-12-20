using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    enum GameObjects
    {
        HPBar
    }

    Stat _stat;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        _stat = transform.parent.GetComponent<Stat>();
    }

    private void Update()
    {
        Debug.Log("UI_HPBar Test");
        Transform parent = gameObject.transform.parent; // 부모 클래스의 transform을 받는다
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y); // 부모 클래스의 Collider 위로 위치를 이동한다
        transform.rotation = Camera.main.transform.rotation; // 체력바의 rotation이 카메라의 rotation과 동일하도록(회전하지 않게)

        float ratio = _stat.Hp / (float)_stat.MaxHp; // 현재체력/최대체력 비율(ratio)를 구한다
        SetHpRatio(ratio); // 체력바 조절 실행
    }

    public void SetHpRatio(float ratio)
    {
        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio; // Slider 의 ratio 를 조절하여 체력바의 증감을 구현한다
    }
}
