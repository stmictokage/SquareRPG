using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour {
	public  static Enemy2 instance = null;
	private GameObject enemy2Attack;
	Vector2 enemy2;
    public int HP;


    // Use this for initialization
    void Awake () {
		if (instance == null)
			instance = this;
		enemy2Attack = GameObject.Find("Enemy2Attack");
        HP = 10;

    }

    // Update is called once per frame
    void Update()
    {
        //HPがゼロの場合エネミーを非アクティブ状態にする
        if (HP <= 0) Delete();

    }

    //非アクティブ状態にする
    private void Delete()
    {
        gameObject.SetActive(false);

    }

    public void move(){
        if (HP <= 0) return;

		//ループ変数
		int i, j;
		//シーン内のゲームオブジェクトの数GameObjectNumber
		int gonum;
		//上下左右に移動していいかの許可。trueならOK、falseならダメ
		bool acceptUp = true, acceptDown = true, acceptLeft = true, acceptRight = true;
		//プレイヤーに近く方向に１を格納。上を０番目、下を１、左を２、右を３番目に格納。
		int[] approachPlayerWays = {0,0,0,0};

		//自分のtranform.positonが入ってる変数
		enemy2 = transform.position;

		//各ベクトルにエネミーの上、左、右、下の座標を登録
		Vector2 up = new Vector2 (transform.position.x, transform.position.y + 0.32f);
		Vector2 down = new Vector2 (transform.position.x, transform.position.y - 0.32f);
		Vector2 left = new Vector2 (transform.position.x - 0.32f, transform.position.y);
		Vector2 right = new Vector2 (transform.position.x + 0.32f, transform.position.y);


		//タグを元に、エネミーがぶつかる可能性のあるゲームオブジェクトを取得。
		//エネミーとブロックは複数個ある可能性があるので、配列で格納。
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		GameObject[] otherEnemies = GameObject.FindGameObjectsWithTag ("Enemy");
		GameObject[] blocks = GameObject.FindGameObjectsWithTag ("block");

		//プレイヤー＋他のエネミー数（全敵ー自分）＋ブロックの数
		//引数の０は配列の1次元目。今回は一次元配列なので０.２次元なら１。
		gonum = 1 + otherEnemies.GetLength (0) - 1 + blocks.GetLength (0);

		//エネミーがぶつかる可能性のある全てのゲームオブジェクトをまとめた配列
		Vector2[] all = new Vector2[gonum];

		//allにゲームオブジェクトを代入
		all [0] = player.transform.position;
		for (i = 1; i < otherEnemies.GetLength (0); i++) {
			//修正要
			all [i] = otherEnemies [1].transform.position;
		}
		j = 0;
		for (; i < gonum; i++) {
			all [i] = blocks [j].transform.position;
			j++;
		}

		//上下左右に障害物がないかを確認。障害物があるとaccept方向にfalseを代入
		for (i = 0; i < gonum; i++) {
			if (up == all [i]) {
				acceptUp = false;
			} else if (down == all [i]) {
				acceptDown = false;
			} else if (left == all [i]) {
				acceptLeft = false;
			} else if (right == all [i]) {
				acceptRight = false;
			}
		}

		//横移動
		if ((Mathf.Abs (Player.instance.player.y - enemy2.y)) < 0.31f) { 
			//プレイヤーと目標地点のx座標の差が32pixel以下なら何もしない
		} else if (Player.instance.player.y > enemy2.y) {
			approachPlayerWays [0] = 1; 
		} else if (Player.instance.player.y < enemy2.y) {
			approachPlayerWays[1] = 1;
		}
		if ((Mathf.Abs (Player.instance.player.x - enemy2.x)) < 0.31f) {
			//プレイヤーと目標地点のy座標の差が32pixel以下なら何もしない
		} else if (Player.instance.player.x < enemy2.x) {
			approachPlayerWays[2] = 1;
		} else if (Player.instance.player.x > enemy2.x) {
			approachPlayerWays[3] = 1;
		}

		//横移動を優先するために、左右上下の順で記述
		if(approachPlayerWays[2]==1&&acceptLeft==true){
			enemy2.x -= 0.32f;
			transform.position = enemy2;
		}else if(approachPlayerWays[3]==1&&acceptRight==true){
			enemy2.x += 0.32f;
			transform.position = enemy2;
		}else if(approachPlayerWays[0]==1&&acceptUp==true){
			enemy2.y += 0.32f;
			transform.position = enemy2;
		}else if(approachPlayerWays[1]==1&&acceptDown==true){
			enemy2.y -= 0.32f;
			transform.position = enemy2;
		}

        //攻撃判定
        attack();
	}

	public void attack(){
        //Debug.Log ("<color=green>Enemy2プレイヤーとのx距離</color>"+Mathf.Abs(Player.instance.player.x - enemy2.x));
        //Debug.Log ("<color=green>Enemy2プレイヤーとのy距離</color>"+Mathf.Abs(Player.instance.player.y - enemy2.y));
        if ((Mathf.Abs (Player.instance.player.x - enemy2.x) + Mathf.Abs (Player.instance.player.y - enemy2.y)) <= 0.32f){

            enemy2Attack.transform.position = Player.instance.player;
			//攻撃エフェクトを描画
			enemy2Attack.GetComponent<Animator> ().Play ("Enemy2Attack", 0, 0.0f);

            Player.instance.Damege(10);


        }
    }

    //エネミーのポジションを返す
    public Vector2 getPosition()
    {
        return transform.position;
    }
}
