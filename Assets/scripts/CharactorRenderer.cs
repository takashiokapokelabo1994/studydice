// �L�����N�^�[�̕\��

using UnityEngine;
using UniRx;
using DG.Tweening;
using Debug = UnityEngine.Debug;
using System.Collections;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using Random = UnityEngine.Random;
using System;
//using Image = System.Net.Mime.MediaTypeNames.Image;

[RequireComponent(typeof(Charactor))]
public class CharactorRenderer : MonoBehaviour
{
    public DiceRollObserver DiceRollObserver;
    public Field Field;
    public GameObject enemy;
    private Charactor Charactor;
    private Charactor Enemy;
    private EnemyController enmCon;
    public GameObject button;
    public GameObject background;
    public Boolean chrWinFlg;
    private SpriteRenderer bgSprite;

    void Awake()
    {
        GameObject.Find("ImageGameCLEAR").GetComponent<UnityEngine.UI.Image>().enabled = false;
        GameObject.Find("Youwin").GetComponent<UnityEngine.UI.Image>().enabled = false;
        GameObject.Find("Youlose").GetComponent<UnityEngine.UI.Image>().enabled = false;
        chrWinFlg = false;
    }
    void Start()
    {
        this.Charactor = this.GetComponent<Charactor>();
        this.Enemy = enemy.GetComponent<Charactor>();
        this.enmCon = enemy.GetComponent<EnemyController>();
        this.bgSprite = background.GetComponent<SpriteRenderer>();

        // �_�C�X��U��C�x���g���󂯂āA�ړ����������s
        this.DiceRollObserver.OnDiceRolledObservable
            .Subscribe(value => this.Move(value))
            .AddTo(this);
    }

    // �_�C�X�̖ڂ����ړ�
    private void Move(int moveCount)
    {
        Debug.Log("mvc:" + moveCount);
        Debug.Log("place:" + this.Charactor.Place);

        button.SetActive(false);

        Sequence moveSequence = DOTween.Sequence();
        for (int i = 1; i <= moveCount; i++)
        {
            int nextPlace = i + this.Charactor.Place;
            // ��������ꍇ��0�ɖ߂�
            if (nextPlace >= this.Field.MassGameObjects.Length)
            {
                nextPlace -= this.Field.MassGameObjects.Length;
            }

            Debug.Log("nextPlace:" + nextPlace);

            // �����낭�̃}�X�I�u�W�F�N�g�̍��W�����ɁA�L�����N�^�[�̎��̈ړ�������߂�
            Vector3 nextPosition = Field.MassGameObjects[nextPlace].transform.localPosition;
            this.bgSprite.sprite = Field.background[nextPlace];

            nextPosition.y += 0.5f;

            //Debug.Log("nextPlace:" + nextPlace);
            this.AppendMove(moveSequence, nextPosition);
        }
        //Enemy
        int diEn = Random.Range(1, 5);
        StartCoroutine(diceRoll(diEn));
        //this.enmCon.MoveEnemy(diEn);
    }

    // �L�����N�^�[�̈ړ�������ǉ�
    private void AppendMove(Sequence sequence, Vector3 newPosition)
    {
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
            chrWinFlg = true;
            GameObject.Find("ImageGameCLEAR").GetComponent<UnityEngine.UI.Image>().enabled = true;
            if (this.enmCon.enemWinFlg == false){
                GameObject.Find("Youwin").GetComponent<UnityEngine.UI.Image>().enabled = true;
            }
            button.SetActive(false);
        }

        return _place;
    }

    // �L�����N�^�[�̌�����ς���
    private void ChangeDirection(int place)
    {
        //�S���̏������I�������{�^���ĕ\��
        if (chrWinFlg == false && enmCon.enemWinFlg == false)
        {
            button.SetActive(true);
        }
    }


    IEnumerator diceRoll(int diEn)
    {
        for (int i = 0;i < diEn + 1 ; i++)
        {
            Debug.Log("Enemy cnt :" + i);
            if (i == 0)
            {
                yield return new WaitForSeconds(2.0f);
            }
            this.enmCon.MoveEnemy(1);
            yield return new WaitForSeconds(0.3f);
        }
    }
}