using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemystate1 : MonoBehaviour
{
    [SerializeField] Animator EnemyController;
    [SerializeField] CharaStatus charadata;
    [SerializeField] MonoBehaviour Enemykinsetu;

    int State;
    IEnemyAction Enemykoudou;

    void Start()
    {
        EnemyController = GetComponent<Animator>();
        //IEnemyのインターフェースを宣言したスクリプトを手に入れ、Enemykoudouへ代入。
        Enemykoudou = GetComponent<IEnemyAction>();
    }

    void Update()
    {
        //コルーチンでEnemytimeの時間だけ待機
        StartCoroutine(Enemytime());

        //Attackパラメータの値を代入し0より大きいなら攻撃中なのでreturn;して処理を終了する。
        int Attack = EnemyController.GetInteger("attack");
        if (Attack > 0)
        {
            return;
        }

        //値が入っていない場合に備えnullチェック。
        if (Enemykoudou != null)
        {
            //スクリプトのEnemyAIkoudou()を呼び出し、返ってきた値をStateに代入する。
            State = Enemykoudou.EnemyAIkoudou();

            //Switch文でStateの値に応じて条件分岐。
            switch (State)
            {
                //Stateが0なら停止。
                case 0:

                    EnemyController.SetInteger("attack", 0);
                    break;

                //Stateが1なら攻撃。
                case 1:

                    EnemyController.SetInteger("attack", 1);
                    break;
            }
        }
    }

    IEnumerator Enemytime()
    {
        yield return new WaitForSeconds(charadata.EnemyTime);
    }
}