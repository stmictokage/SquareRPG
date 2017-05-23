using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatusBtn : MonoBehaviour
{

    [SerializeField] UnityEngine.Events.UnityEvent _event;

    [SerializeField] private float _longTime = 0.1f, _interval = 0.1f;
    private float _waitTime = 0;
    private bool _isPressing = false;
    private bool _invoke = false;


    public void Awake()
    {

        //ボタンを押し時のイベント作成
        EventTrigger.Entry pressDown = new EventTrigger.Entry();
        pressDown.eventID = EventTriggerType.PointerDown;
        pressDown.callback.AddListener((data) => { PressDown(); });

        //ボタンを離した時のイベント作成
        EventTrigger.Entry pressUp = new EventTrigger.Entry();
        pressUp.eventID = EventTriggerType.PointerUp;
        pressUp.callback.AddListener((data) => { PressUp(); });

        //ボタンをクリックした時のイベント作成
        EventTrigger.Entry click = new EventTrigger.Entry();
        click.eventID = EventTriggerType.PointerClick;
        click.callback.AddListener((data) => { Click(); });

        //イベントトリガーを追加し、イベントを登録
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        trigger.triggers.Add(pressDown);
        trigger.triggers.Add(pressUp);
        trigger.triggers.Add(click);
    }

    //ボタンを押した瞬間に実行されるメソッド
    private void PressDown()
    {
        Debug.Log("test1");
        _isPressing = true;
        _invoke = false;
        _waitTime = _longTime;
    }

    //ボタンを離した瞬間に実行されるメソッド
    private void PressUp()
    {
        _isPressing = false;
    }

    //クリックした瞬間に実行されるメソッド
    private void Click()
    {
        Debug.Log("test2");
        //一度もイベントが実行されていなければ実行
        if (!_invoke)
        {
            _event.Invoke();
        }
    }

    private void Update()
    {
        //ボタンが押されていない時はスルー
        if (!_isPressing)
        {
            return;
        }

        //待ち時間を減らす
        _waitTime -= Time.deltaTime;

        //待ち時間がまだある時はスルー
        if (_waitTime > 0)
        {
            return;
        }

        //メソッド実行、待ち時間設定
        _event.Invoke();
        _waitTime = _interval;
        _invoke = true;
    }

    public void ChagenStatus(int num)
    {
        GameObject obj;
        Text text, Sum;
        int setNum;
        int max = Player.instance.getMax();
        int sum = Player.instance.getSum();
        if ((num < 0 && max == sum) || (num > 0 && sum <= 0))
        {
            Debug.Log("test5");

            return;
        }




        switch (System.Math.Abs(num))
        {
            case 1:
                Debug.Log("test3");

                obj = GameObject.Find("HPnum");
                text = obj.GetComponentInChildren<Text>();
                if (num > 0)
                {

                    setNum = Player.instance.getHP() + 1;

                }
                else
                {
                    if (Player.instance.getHP() <= 0) return;
                    setNum = Player.instance.getHP() - 1;
                }
                text.text = (setNum).ToString();
                Player.instance.setHP(setNum);
                break;
            case 2:
                obj = GameObject.Find("ATKnum");
                text = obj.GetComponentInChildren<Text>();
                if (num > 0)
                {

                    setNum = Player.instance.getATK() + 1;

                }
                else
                {
                    if (Player.instance.getATK() <= 0) return;
                    setNum = Player.instance.getATK() - 1;
                }
                text.text = (setNum).ToString();
                Player.instance.setATK(setNum);
                break;
            case 3:
                obj = GameObject.Find("DEFnum");
                text = obj.GetComponentInChildren<Text>();

                if (num > 0)
                    text = obj.GetComponentInChildren<Text>();
                if (num > 0)
                {

                    setNum = Player.instance.getDEF() + 1;

                }
                else
                {
                    if (Player.instance.getDEF() <= 0) return;
                    setNum = Player.instance.getDEF() - 1;
                }
                text.text = (setNum).ToString();
                Player.instance.setDEF(setNum);
                break;
            case 4:
                obj = GameObject.Find("SPDnum");
                text = obj.GetComponentInChildren<Text>();
                if (num > 0)
                {

                    setNum = Player.instance.getSPD() + 1;

                }
                else
                {
                    if (Player.instance.getSPD() <= 0) return;
                    setNum = Player.instance.getSPD() - 1;
                }
                text.text = (setNum).ToString();
                Player.instance.setSPD(setNum);
                break;
            case 5:
                obj = GameObject.Find("LUKnum");
                text = obj.GetComponentInChildren<Text>();
                if (num > 0)
                {

                    setNum = Player.instance.getLUK() + 1;

                }
                else
                {
                    if (Player.instance.getLUK() <= 0) return;
                    setNum = Player.instance.getLUK() - 1;
                }
                text.text = (setNum).ToString();
                Player.instance.setLUK(setNum);
                break;

        }
        Sum = GameObject.Find("SUMnum").GetComponentInChildren<Text>();
        if (num < 0)
        {
            sum += 1;
        }
        else if (num > 0)
        {
            sum -= 1;
        }
        Player.instance.setSUM(sum);
        Sum.text = (sum).ToString();
    }

}
