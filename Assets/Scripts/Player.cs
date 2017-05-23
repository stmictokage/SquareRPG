using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public static Player instance = null;

	Vector2 end;
	public Vector2 player;

	// Use this for initialization
	void Start () {
		if (instance == null)
			instance = this;
		//トランスフォームポジションに１−１の座標を登録。トランスフォームポジションは座標を保持する変数
		transform.position = GameObject.Find("1-1").transform.position;
		end = GameObject.Find("8-8").transform.position;

	}

	// Update is called once per frame
	void Update() {
		//プレイヤーターンがfalseならリターン
		if (!GameManager.instance.playerTurn) return;

		//ループ変数
		int i, j;
		//シーン内のゲームオブジェクトの数.GameObjectNumber
		int gonum;
		//上下左右に移動していいかの許可。trueならOK、falseならダメ
		bool acceptUp = true, acceptDown = true, acceptLeft = true, acceptRight = true;

		//各ベクトルにエネミーの上、左、右、下の座標を登録
		Vector2 up = new Vector2 (transform.position.x, transform.position.y + 0.32f);
		Vector2 down = new Vector2 (transform.position.x, transform.position.y - 0.32f);
		Vector2 left = new Vector2 (transform.position.x - 0.32f, transform.position.y);
		Vector2 right = new Vector2 (transform.position.x + 0.32f, transform.position.y);


		//タグを元に、プレイヤーがぶつかる可能性のあるゲームオブジェクトを取得。
		//エネミーとブロックは複数個ある可能性があるので、配列で格納。;
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		GameObject[] blocks = GameObject.FindGameObjectsWithTag ("block");

		//GameObjectNumber(gonum)=エネミー数＋ブロックの数
		//引数の０は配列の1次元目。今回は一次元配列なので０.２次元なら１。
		gonum = Enemies.GetLength (0) + blocks.GetLength (0);

		//エネミーがぶつかる可能性のある全てのゲームオブジェクトをまとめた配列
		Vector2[] all = new Vector2[gonum];


		//movepoint(次の目的地)にR(移動経路)の０番目を挿入
		Vector2 movepoint=GameManager.instance.R[GameManager.instance.position];
		//プレイヤーの座標を1-1に設定
		player = transform.position;

		if (player == movepoint)
		{
			//R(移動経路)のインデントを増やしてる
			GameManager.instance.position += 1;
			//目標地点についたから次の目標を更新している
			movepoint = GameManager.instance.R[GameManager.instance.position];
		}

		//allにゲームオブジェクトを代入
		for (i = 0; i < Enemies.GetLength (0); i++) {
			//修正要
			all [i] = Enemies [i].transform.position;
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

		if ((Mathf.Abs(player.x - movepoint.x)) < 0.31f) { 
			//プレイヤーと目標地点のx座標の差が32pixel以下なら何もしない
		} else if (player.x < movepoint.x&&acceptRight==true) {
			player.x += 0.32f;
			transform.position = player;
		} else if(player.x > movepoint.x&&acceptLeft==true){
			player.x -= 0.32f;
			transform.position = player;
		}

		if ((Mathf.Abs(player.y - movepoint.y)) < 0.31f) {
			//プレイヤーと目標地点のy座標の差が32pixel以下なら何もしない
		} else if (player.y < movepoint.y&&acceptUp==true) {
			player.y += 0.32f;
			transform.position = player;
		} else if(player.y > movepoint.y&&acceptDown==true){
			player.y -= 0.32f;
			transform.position = player;
		}

		//プレイヤーの座標とゴールの座標が重なったら
		if (player == end)
		{
			GameManager.instance.StageComp();
		}
		//Debug.Log("R"+GameManager.instance.R[0]+"  "+GameManager.instance.R[1]+"   "+GameManager.instance.R[2]+"   "+GameManager.instance.R[3]);
		//上のif文二つでx軸に一回、y軸に一回動いたからプレイヤーターン終了
		GameManager.instance.playerTurn = false;
	}
}
