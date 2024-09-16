// ダイスを振るボタンを押した時の処理

using UnityEngine;
using UniRx;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RollDiceButtonDispatcher : MonoBehaviour
{
    public DiceRollObserver DiceRollObserver;

    void Start()
    {
        this.GetComponent<Button>().OnClickAsObservable()
            .Subscribe(_ => this.DiceRollObserver.RollDice())
            .AddTo(this);
    }
}