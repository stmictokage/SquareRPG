using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager instance = null;
	public bool playerTurn = false;
	public bool routeTurn = true;
	public bool enemiesTurn = false;

	public float turnDelay = 0.5f;
	private Vector2 goal;
	//不明。Rは何？プレイヤーの移動の軌跡の配列？
	public List<Vector2> R =new List<Vector2>();
	public int position = 0;

	private GameObject comp,error,gameover;


	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
		InitGame();

	}

	//不明。一回しか実行されない？
	void InitGame()
	{
		//ステージクリアの文字を表示しないようにするため
		comp = GameObject.Find("StageComp");
		comp.SetActive(false);

		//ゴールまで道をつないでくださいを表示しないようにするため
		error = GameObject.Find("Error");
		error.SetActive(false);

        gameover = GameObject.Find("GameOver");
        gameover.SetActive(false);

		//ゴールを8−9に設定
		goal = GameObject.Find("8-8").transform.position;
		//初期地点＝firstを1-1に設定
		Vector2 first = GameObject.Find("1-1").transform.position;

		//不要。
		routeTurn = true;
		//初期地点を２Dのオブジェクトリストに追加
		R.Add(first);
		//Rのインデックス
		position += 1;
		turnDelay = 0.5f;
	}

	public void ErrorMessage()
	{
		//エラーメッセージのオブジェクトをtrueにすることで、エラーメッセージを表示。非表示→表示。
		error.SetActive(true);
		//invoke("メソッド名",xf);はx秒後にメソッドを実行という意味。fはfloat型のf。C#は数字は全てfloatでないと認識しないらしい。
		Invoke("ErrorMessageDel", 2.0f);
	}
	public void ErrorMessageDel()
	{
		error.SetActive(false);

	}


	//ルート指定するときの画面の処理
	IEnumerator SetUp()
	{
		//クリックしたかどうか？「true or false的な？」をobjに挿入
		GameObject obj = getClickobject();
		//不明。なんのベクトル？ベクトル＝座標？
		Vector2 check;

		//クリックされていたら？
		if (obj!=null)
		{
			//siteiにクリックしたポジションの座標を挿入
			Vector2 sitei = obj.transform.position;
			//きちんとルート作成できているかどうかのチェック
			if (R.LastIndexOf(sitei) == -1)//今まで一回もルートに指定していなかったら
			{
				//移動経路の一個前にクリックした座標をチェックに代入
				check = R[R.Count - 1];

				//前に押した箇所のx座標かy座標と同じならRに追加
				if (((check.x == sitei.x) && (check.y != sitei.y)) || ((check.x != sitei.x) && (check.y == sitei.y))) {
					R.Add (sitei);
					//objのコンポーネントのレンダラーの素材の色指定に赤を挿入
					obj.GetComponent<Renderer> ().material.color = Color.red;
				}
			}
		}
		//特に意味なし。returnでおk
		yield return 0;

	}
		
	private GameObject getClickobject()
	{
		GameObject result = null;
		//不明。マウスの左ボタンが押されていたらtrue.ちな右は１、真ん中は２を表す。
		if (Input.GetMouseButtonDown(0))
		{
			//不明。LayerMask.NameToLayerはmapレイヤーのインデックスを返す。
			int enablelayer = 1 << LayerMask.NameToLayer("map");
			//スクリーン座標からワールド座標へ変換したマウスの位置をtapPointに挿入
			Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//マウスでタップした座標がレイヤーの中に含まれているかどうかを判定
			Collider2D co2d = Physics2D.OverlapPoint(tapPoint,enablelayer);
			//含まれていたら
			if (co2d)
			{
				//不明。resultにタップされた座標を挿入？
				result = co2d.transform.gameObject;
			}
		}

		return result;
	}

	public bool CheckEnd()
	{
		Vector2 v = R[R.Count-1];

		//最後に指定された赤四角の座標がゴールの座標と一致していたらtrue
		if ((v.x == goal.x) && (v.y==goal.y)) {
			return true;
		}

		return false;
	}

//	// 予備
//	void Update () {
//
//		//routeTurnがtrueならif分に入る
//		if (routeTurn==true)
//		{
//			//1フレームごとにセットアップを呼び出してる
//			StartCoroutine(SetUp());
//			return;
//		}
//
//		//ルート指定が終わっていて、プレーヤーターンだと入る。
//		//二回目からはエネミーターンがtrueじゃなかったら入る。
//		if (playerTurn==true)
//		{
//			//Player.csが、プレイヤーを動かし、動き終わったら
//			//playerTurnをfalseにする
//			return;
//		}
//
//		//ルート指定とプレーヤーターンが終わるとエネミーターン
//		//EnemyManagerがenemiesTurnを監視していて
//		//エネミーが動き終わったらenemiesTurnをfalseにすることで
//		//上のif文に入り、再度プレイヤーターンとなる。
//		StartCoroutine(EnemyTime());
//	}

	void Update () {

		//routeTurnがtrueならif分に入る
		if (routeTurn==true)
		{
			//1フレームごとにセットアップを呼び出してる
			StartCoroutine(SetUp());
			return;
		}


	}

	public void ControllMove(){
		StartCoroutine (SwitchPlayerEnemy());
	}

	IEnumerator SwitchPlayerEnemy(){
		//プレイヤーが行動してから、モンスターが行動するまでturnDelay秒待機。
		playerTurn = true;
		yield return new WaitForSeconds (turnDelay);

		enemiesTurn = true;

		yield return new WaitForSeconds (turnDelay);


		//エネミーの攻撃エフェクトの描画をしたかどうかの変数を初期化
		EnemyManager.instance.finishEnemyNumber = 0;
		ControllMove ();
	}

	//ステージコンプリートを非表示から表示に切り替えるメソッド
	public void StageComp()
	{
		comp.SetActive(true);
	}

    public void GameOver()
    {
        gameover.SetActive(true);
        enabled = false;
    }


}
