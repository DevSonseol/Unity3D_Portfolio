using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : FloatingText
{
    private Color alpha;
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private float alphaSpeed = 2;

    void Awake()
    {
        alpha = text.color;
    }

    void Update()
    {
        base.Update();
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); // 텍스트 위치
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed); // 텍스트 알파값
    }

    public void InitText(float damage,float time ,Transform target ,Color textColor)
    {
        unit = target;
        text.text = damage.ToString();
        text.color = textColor;
        Invoke("TimerSetting", time);
        transform.SetParent(target);
        transform.position = unit.position + offSet;
    }
    
    void TimerSetting()
    {
        Main main = Main.Instance;
        main.Destroy(this.gameObject);
    }
}
