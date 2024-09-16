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
            // ��������ꍇ��0�ɖ߂�
            if (nextPlace >= this.Field.MassGameObjects.Length)
            {
                nextPlace -= this.Field.MassGameObjects.Length;
            }

            Debug.Log("enemyPlace:" + nextPlace);

            // �����낭�̃}�X�I�u�W�F�N�g�̍��W�����ɁA�L�����N�^�[�̎��̈ړ�������߂�
            Vector3 nextPosition = Field.MassGameObjects[nextPlace].transform.localPosition;

            nextPosition.y += 0.5f;

            //Debug.Log("nextPlace:" + nextPlace);
            this.AppendMove(moveSequence, nextPosition);
        }

    }

    // �L�����N�^�[�̈ړ�������ǉ�
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
                    // �ړ���̏���
                    // �L�����N�^�[�̈ʒu�̍X�V
                    this.Charactor.Place = this.GetNewPlace(this.Charactor.Place);
                    // �ʒu�ɂ���ăL�����N�^�[�̌�����ς��鏈��
                    this.ChangeDirection(this.Charactor.Place);
                })
        );
    }

    // �ʒu����i�߂��V�����ʒu��Ԃ�
    private int GetNewPlace(int place)
    {
        int _place = place;
        if (Field.MassGameObjects.Length - 1 != place)
        {
            _place++;
        }
        else // ��������ꍇ
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

    // �L�����N�^�[�̌�����ς���
    private void ChangeDirection(int place)
    {
        //�S���̏������I�������{�^���ĕ\��
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
