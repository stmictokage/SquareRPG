using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
	public static EnemyManager instance = null;
	//各エネミーの攻撃エフェクトを宣言
	private GameObject enemy1Attack;
	private GameObject enemy2Attack;
	//攻撃し終わったエネミーの数
	public int finishEnemyNumber = 0;

	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		enemy1Attack= GameObject.Find("Enemy1Attack");
		enemy2Attack= GameObject.Find("Enemy2Attack");
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
				Enemy1.instance.attack ();
			}
		}

		if(GameManager.instance.enemiesTurn == true){
			if(finishEnemyNumber==1){
				//行動し終わったエネミー数を１増加
				finishEnemyNumber += 1;
				Enemy2.instance.move ();
				Enemy2.instance.attack ();
				//最後のエネミーのif文にエネミーターン終了を書く
				GameManager.instance.enemiesTurn = false;
			}
		}
		//ここにどんどんエネミーを追加
//		if(GameManager.instance.enemiesTurn == true){
//			Enemy3.instance.move ();
//		}

		
	}

	//各エネミースクリプトのエフェクトの描画の表示をonにする関数
	public void DrawEffectOn(){
		enemy1Attack.SetActive (true);
		enemy2Attack.SetActive (true);
		//enemy1Attack.GetComponent<Animator> ().Play ("Enemy1Attack", 0, 0.0f);

	}

	//各エネミースクリプトのエフェクトの描画の表示をonにする関数
	public void DrawEffectOff(){
		enemy1Attack.SetActive (false);
		enemy2Attack.SetActive (false);
	}
}
 