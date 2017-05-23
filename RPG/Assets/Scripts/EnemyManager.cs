using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
	public static EnemyManager instance = null;

	//攻撃し終わったエネミーの数
	public int finishEnemyNumber = 0;

	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		//ゲームマネージャのエネミーターンがtrueならエネミースクリプトのmoveメソッドを発火
		if(GameManager.instance.enemiesTurn == true){
			//Enemy1を一回だけ行動
			if(finishEnemyNumber==0){
				//行動し終わったエネミー数を１増加
				finishEnemyNumber += 1;
				Enemy1.instance.move ();
			}
		}
        if (GameManager.instance.enemiesTurn == true){
			if(finishEnemyNumber==1){
				//行動し終わったエネミー数を１増加
				finishEnemyNumber += 1;
				Enemy2.instance.move ();
				//最後のエネミーのif文にエネミーターン終了を書く
				GameManager.instance.enemiesTurn = false;
			}
		}
		//ここにどんどんエネミーを追加
//		if(GameManager.instance.enemiesTurn == true){
//			Enemy3.instance.move ();
//		}

		
	}

    //エネミーに対するダメージ処理
    public void Damege(Vector2 pos)
    {
        int dmg = Player.instance.getATK();
        int index=0;
        if (pos == Enemy1.instance.getPosition())
        {
            index = 1;
        }else if(pos == Enemy2.instance.getPosition())
        {
            index = 2;
        }
        switch (index)
        {
            case 1:
                Debug.Log("enemy1Dmg" + dmg);
                Enemy1.instance.HP -= dmg;
                break;
            case 2:
                Debug.Log("enemy2Dmg" + dmg);

                Enemy2.instance.HP -= dmg;
                break;

        }
        
    }
}
 