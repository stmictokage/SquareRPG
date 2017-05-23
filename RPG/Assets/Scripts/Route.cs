using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour {

	public List<Vector2> R = new List<Vector2>();


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		if (!GameManager.instance.routeTurn) return;

		//１フレームごとに呼び出される。
		//このセットアップは下のセットアップ？
		StartCoroutine(SetUp());

	}

	IEnumerator SetUp()
	{
		GameObject obj = getClickobject();
		while (true)
		{
			if (obj != null)
			{
				Vector2 sitei = obj.transform.position;
				//きちんとルート作成できているかどうかのチェック
				if (R.LastIndexOf(sitei) == -1)
				{
					R.Add(sitei);
				}
				obj.GetComponent<Renderer>().material.color = Color.red;
			}

			yield return 0;

		}
	}

	private GameObject getClickobject()
	{
		GameObject result = null;
		if (Input.GetMouseButtonDown(0))
		{
			int enablelayer = 1 << LayerMask.NameToLayer("map");
			Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D co2d = Physics2D.OverlapPoint(tapPoint, enablelayer);
			if (co2d)
			{
				result = co2d.transform.gameObject;
			}
		}

		return result;
	}
}
