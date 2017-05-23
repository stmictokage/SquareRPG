using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public static Player instance = null;

	Vector2 end;
	public Vector2 player;
    GameObject[] Enemies,blocks;
    private GameObject Attack;
    bool goal; //プレイヤーがゴールしているかを判定する
    private int HP, ATK, DEF, SPD, LUK, SUM, MAX;


    // Use this for initialization
    void Start () {
		if (instance == null)
			instance = this;
		//トランスフォームポジションに１−１の座標を登録。トランスフォームポジションは座標を保持する変数
		transform.position = GameObject.Find("1-1").transform.position;
		end = GameObject.Find("8-8").transform.position;

        Attack = GameObject.Find("PlayerAttack");

        goal = false;


        //タグを元に、プレイヤーがぶつかる可能性のあるゲームオブジェクトを取得。
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        blocks = GameObject.FindGameObjectsWithTag("block");


        setStatus();



    }
    void setStatus()
    {
        HP = 0;
        ATK = 0;
        DEF = 0;
        SPD = 0;
        LUK = 0;
        MAX = 100;
        SUM = MAX;
    }
    // Update is called once per frame
    void Update() {
		//プレイヤーターンがfalseならリターン
		if (!GameManager.instance.playerTurn || goal) return;

		//ループ変数
		int i;
		//シーン内のゲームオブジェクトの数.GameObjectNumber
		int gonum;
		//上下左右に移動していいかの許可。trueならOK、falseならダメ
		bool acceptUp = true, acceptDown = true, acceptLeft = true, acceptRight = true;

        //攻撃したかどうか
        bool DoAtk=false;

		//各ベクトルにplayerの上、左、右、下の座標を登録
		Vector2 up = new Vector2 (transform.position.x, transform.position.y + 0.32f);
		Vector2 down = new Vector2 (transform.position.x, transform.position.y - 0.32f);
		Vector2 left = new Vector2 (transform.position.x - 0.32f, transform.position.y);
		Vector2 right = new Vector2 (transform.position.x + 0.32f, transform.position.y);


		//エネミーの位置を更新
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag ("Enemy");

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


		//上下左右に障害物がないかを確認。障害物があるとaccept方向にfalseを代入
		for (i = 0; i < blocks.GetLength(0); i++) {
			if (up == (Vector2)blocks [i].transform.position) {
				acceptUp = false;
			} else if (down == (Vector2)blocks[i].transform.position) {
				acceptDown = false;
			} else if (left == (Vector2)blocks[i].transform.position) {
				acceptLeft = false;
			} else if (right == (Vector2)blocks[i].transform.position) {
				acceptRight = false;
			}
		}

        //移動前の攻撃
        //上下左右に敵がいないかを確認。敵がいると攻撃。
        //敵を倒しても移動しないようにaccept方向にfalseを代入

        for (i = 0; i < Enemies.GetLength(0); i++)
        {
            if (up == (Vector2)Enemies[i].transform.position)
            {
                attack(up);
                DoAtk = true;
                acceptUp = false;
            }
            else if (down == (Vector2)Enemies[i].transform.position)
            {
                attack(down);
                DoAtk = true;
                acceptDown = false;
            }
            else if (left == (Vector2)Enemies[i].transform.position)
            {
                attack(left);
                DoAtk = true;
                acceptLeft = false;
            }
            else if (right == (Vector2)Enemies[i].transform.position)
            {
                attack(right);
                DoAtk = true;
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


        if (DoAtk == false)
        {
            //移動後のplayerの上、左、右、下の座標を登録
            up = new Vector2(transform.position.x, transform.position.y + 0.32f);
            down = new Vector2(transform.position.x, transform.position.y - 0.32f);
            left = new Vector2(transform.position.x - 0.32f, transform.position.y);
            right = new Vector2(transform.position.x + 0.32f, transform.position.y);

            //移動後の攻撃
            for (i = 0; i < Enemies.GetLength(0); i++)
            {
                if (up == (Vector2)Enemies[i].transform.position)
                {
                    attack(up);
                }
                else if (down == (Vector2)Enemies[i].transform.position)
                {
                    attack(down);
                }
                else if (left == (Vector2)Enemies[i].transform.position)
                {
                    attack(left);
                }
                else if (right == (Vector2)Enemies[i].transform.position)
                {
                    attack(right);
                }
            }
        }


        //プレイヤーの座標とゴールの座標が重なったら
        if (player == end)
		{
			GameManager.instance.StageComp();
            goal = true;
		}
		//Debug.Log("R"+GameManager.instance.R[0]+"  "+GameManager.instance.R[1]+"   "+GameManager.instance.R[2]+"   "+GameManager.instance.R[3]);
		//上のif文二つでx軸に一回、y軸に一回動いたからプレイヤーターン終了
		GameManager.instance.playerTurn = false;
	}



    public void attack(Vector2 vec)
    {

        // Debug.Log("<color=red>Enemy1プレイヤーとのx距離</color>" + Mathf.Abs(Player.instance.player.x - enemy1.x));
        //Debug.Log("<color=red>Enemy1プレイヤーとのy距離</color>" + Mathf.Abs(Player.instance.player.y - enemy1.y));
        Attack.transform.position = vec;
        //攻撃エフェクトを描画
        Attack.GetComponent<Animator>().Play("playerAttack", 0, 0.0f);

        EnemyManager.instance.Damege(vec);
        
    }


    public void Damege(int atk)
    {
        int dmg = atk - (DEF / 2);
        if(0<dmg){
            HP -= dmg;
        }
        Debug.Log("HP" + HP);
        Debug.Log("ATK" + ATK);


        if (HP <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    public int getMax()
    {
        return MAX;
    }

    public int getSum()
    {
        return SUM;
    }
    public int getHP()
    {
        return HP;
    }
    public int getATK()
    {
        return ATK;
    }
    public int getDEF()
    {
        return DEF;
    }
    public int getSPD()
    {
        return SPD;
    }
    public int getLUK()
    {
        return LUK;
    }

    public void setHP(int hp)
    {
        HP = hp;
    }
    public void setATK(int atk)
    {
        ATK = atk;
    }

    public void setDEF(int def)
    {
        DEF = def;
    }
    public void setSPD(int spd)
    {
        SPD = spd;
    }
    public void setLUK(int luk)
    {
        LUK = luk;
    }
    public void setSUM(int sum)
    {
        SUM = sum;
    }
}
