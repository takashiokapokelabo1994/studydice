using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Random = UnityEngine.Random;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class DiceRollObserver : MonoBehaviour
{
    public Subject<int> OnDiceRolledObservable = new Subject<int>();

    public void RollDice()
    {
        // 今回は１〜６の目が出るダイス
        Debug.Log("dice");
        this.OnDiceRolledObservable.OnNext(Random.Range(1, 7));
    }
}
