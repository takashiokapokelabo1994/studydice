using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Debug = UnityEngine.Debug;
using System.Diagnostics;
using System;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    private Charactor Charactor;
    public Field Field;
    public GameObject player;
    public GameObject button;
    public CharactorRenderer chRen;
    public Boolean enemWinFlg;

    void Start()
    {
        this.Charactor = this.GetComponent<Charactor>();
        enemWinFlg = false;
        chRen = player.GetComponent<CharactorRenderer>();
        //this.Field = this.GetComponent<Field>();
    }

    public void MoveEnemy(int moveCount)
    {
        Debug.Log("MoveEnemy:" + moveCount);
        Debug.Log("Field:" + Field);

        //int diEn = Random.Range(1, 5);
        //StartCoroutine(diceRoll(diEn));

        Sequence moveSequence = DOTween.Sequence();
        for(int i = 1; i <= moveCount; i++)
        {
            //Debug.Log("count " + i);
            int nextPlace = i + this.Charactor.Place;
            Debug.Log("nextPlace:" + nextPlace);
            Debug.Log(this.Field.MassGameObjects.Length);
            // 一周した場合は0に戻す
            if (nextPlace >= this.Field.MassGameObjects.Length)
            {
                nextPlace -= this.Field.MassGameObjects.Length;
            }

            Debug.Log("enemyPlace:" + nextPlace);

            // すごろくのマスオブジェクトの座標を元に、キャラクターの次の移動先を決める
            Vector3 nextPosition = Field.MassGameObjects[nextPlace].transform.localPosition;

            nextPosition.y += 0.5f;

            //Debug.Log("nextPlace:" + nextPlace);
            this.AppendMove(moveSequence, nextPosition);
        }

    }

    // キャラクターの移動処理を追加
    private void AppendMove(Sequence sequence, Vector3 newPosition)
    {
        Debug.Log("AppendMove");
        Debug.Log(newPosition);
        //Debug.Log("aM:" + this.Charactor.Place);

        sequence.Append(
            //this.GetComponent<RectTransform>()
            this.GetComponent<Transform>()
                .DOLocalMove(newPosition, 0.5f)
                .OnComplete(() =>
                {
                    // 移動後の処理
                    // キャラクターの位置の更新
                    this.Charactor.Place = this.GetNewPlace(this.Charactor.Place);
                    // 位置によってキャラクターの向きを変える処理
                    this.ChangeDirection(this.Charactor.Place);
                })
        );
    }

    // 位置を一つ進めた新しい位置を返す
    private int GetNewPlace(int place)
    {
        int _place = place;
        if (Field.MassGameObjects.Length - 1 != place)
        {
            _place++;
        }
        else // 一周した場合
        {
            Debug.Log("finish");
            _place = 0;
            enemWinFlg = true;
            GameObject.Find("ImageGameCLEAR").GetComponent<UnityEngine.UI.Image>().enabled = true;
            if (chRen.chrWinFlg == false){
                GameObject.Find("Youlose").GetComponent<UnityEngine.UI.Image>().enabled = true;
            }
            button.SetActive(false);
        }

        return _place;
    }

    // キャラクターの向きを変える
    private void ChangeDirection(int place)
    {
        //全部の処理が終わったらボタン再表示
        if (chRen.chrWinFlg == false && enemWinFlg == false){
            button.SetActive(true);
        }
    }

    IEnumerator diceRoll(int diEn)
    {
        for (int i = 0; i < diEn; i++)
        {
            Debug.Log("Enemy cnt :" + i);
            yield return new WaitForSeconds(5.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
