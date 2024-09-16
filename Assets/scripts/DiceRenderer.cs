// ダイスの数字を表示する

using UnityEngine;
using UnityEngine.UI;
using UniRx;
using static System.Net.Mime.MediaTypeNames;
using Text = UnityEngine.UI.Text;
using System.Collections.Specialized;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Collections;
using System.Threading;
using System;
//using UnityEngine.Random;
//using System.Diagnostics.Eventing.Reader;

[RequireComponent(typeof(DiceRollObserver))]
[RequireComponent(typeof(Text))]
public class DiceRenderer : MonoBehaviour
{
    public GameObject dice;
    private Transform dicetra;
    void Start()
    {
        // ダイスを振るイベントを受けて、テキストの描画
        this.GetComponent<DiceRollObserver>()
            .OnDiceRolledObservable
            .Subscribe(value => this.Render(value))
            .AddTo(this);
        dicetra = dice.transform;
    }

    // テキストの描画
    private void Render(int value)
    {
        this.GetComponent<Text>().text = value.ToString();
        StartCoroutine(diceRoll(value));
    }

    IEnumerator diceRoll(int value)
    {
        var i = 0;
        //さいころを転がす（演出）
        do
        {
            dicetra.Rotate(new Vector3(0, 0, 10f));
            dicetra.localPosition += new Vector3(0.0f, 0.03f, 0.0f);
            i += 10;
            yield return new WaitForSeconds(0.01f);
            //Debug.Log("i=" + i);
        } while (i < 360);
        //さいころを転がす（演出２）
        var j = 0;
        do
        {
            dicetra.Rotate(new Vector3(0, 10f, 0));
            dicetra.localPosition += new Vector3(0, -0.03f, 0);
            j += 10;
            yield return new WaitForSeconds(0.01f);
            //Debug.Log("j=" + j);
        } while (j < 360);

        //本番
        dicetra.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;

        int rndDice = UnityEngine.Random.Range(1, 2);
        int max = 0;

        switch (value)
        {
            case 1:
                if (rndDice == 1)
                {
                    y = 10.0f;
                }
                else
                {
                    x = 10.0f;
                }
                max = 180;
                break;
            case 2:
                x = 10.0f;
                max = 270;
                break;
            case 3:
                y = 10.0f;
                max = 270;
                break;
            case 4:
                y = 10.0f;
                max = 90;
                break;
            case 5:
                x = 10.0f;
                max = 90;
                break;
            case 6:
                if (rndDice == 1)
                {
                    y = 10.0f;
                }
                else
                {
                    x = 10.0f;
                }
                max = 360;
                break;
        }

        var k = 0;
        Debug.Log(value + "    " + x + ":" + y + ":" + z);

        do
        {
            dicetra.Rotate(new Vector3(x, y, z));
            k += 10;
            yield return new WaitForSeconds(0.01f);

        } while (k < max);
    }

    /*
    IEnumerator diceRoll(int value) {
        var i = 0;
        //さいころを転がす（演出）
        do {
            dicetra.Rotate(new Vector3(0, 0, 10f));
            i += 10;
            yield return new WaitForSeconds(0.01f);
            Debug.Log("i=" + i);
        } while (i < 360);

        //さいころを転がす（演出２）
        var j = 0;
        do {
            dicetra.Rotate(new Vector3(0, 10f, 0));
            j += 10;
            yield return new WaitForSeconds(0.01f);
            Debug.Log("j=" + j);
        } while (j < 360);

        //本番
        private float x = 0.0f;
        private float y = 0.0f;
        private float z = 0.0f;

        int rndDice = Random.Range(1, 2);
        int max = 0;
        
        switch(value){
            case 1:
                if (rndDice == 1){
                    y = 10.0f;
                }else{
                    x = 10.0f;
                }
                max = 180;
                break;
            case 2:
                x = 10.0f;
                max = 270;
                break;
            case 3:
                y = 10.0f;
                max = 270;
                break;
            case 4:
                y = 10.0f;
                max = 90;
                break;
            case 5:
                x = 10.0f;
                max = 90;
                break;
            case 6:
                if (rndDice == 1){
                    y = 10.0f;
                } else{
                    x = 10.0f;
                }
                max = 360;
                break;
       }
    }
    */
}