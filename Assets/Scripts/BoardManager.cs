using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
	[Serializable]
	public class Count {
		public int max;
		public int min;

		public Count (int min, int max) {
			this.min = min;
			this.max = max;
		}
	}

	public int columns = 8;
	public int rows = 8;
	public Count innerWallCount = new Count(5, 9);
	public Count foodCount = new Count(1, 5);
	public GameObject exit;
	public GameObject[] floorTiles;
	public GameObject[] innerWallTiles;
	public GameObject[] outerWallTiles;
	public GameObject[] enemyTiles;
	public GameObject[] foodTiles;

	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3>();

	void InitialiseList() {
		gridPositions.Clear();

		for (int x = 1; x < columns-1; x++) {
			for (int y = 1; y < rows-1; y++) {
				gridPositions.Add(new Vector3(x, y, 0f));
			}
		}
	}

	void SetupFloorAndOuterWalls() {
		boardHolder = new GameObject("Board").transform;

		for (int x = -1; x < columns +1; x++) {
			for (int y = -1; y < rows +1; y++) {
				GameObject prefab = floorTiles[Random.Range(0, floorTiles.Length)];

				if (x == -1 || x == columns || y == -1 || y == rows) {
					prefab = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
				}

				GameObject instance = Instantiate(prefab, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

				instance.transform.SetParent(boardHolder);
			}
		}
	}

	Vector3 PopRandomPosition() {
		int randomIndex = Random.Range(0, gridPositions.Count);
		Vector3 randomPosition = gridPositions[randomIndex];
		gridPositions.RemoveAt(randomIndex);
		return randomPosition;
	}

	void LayoutObjectsAtRandom(GameObject[] tiles, Count constraints) {
		int objectCount = Random.Range(constraints.min, constraints.max + 1);

		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = PopRandomPosition();
			GameObject tileChoice = tiles[Random.Range(0, tiles.Length)];
			Instantiate(tileChoice, randomPosition, Quaternion.identity);
		}
	}

	public void SetupScene(int level) {
		SetupFloorAndOuterWalls();
		InitialiseList();
		LayoutObjectsAtRandom(innerWallTiles, innerWallCount);
		LayoutObjectsAtRandom(foodTiles, foodCount);
		int enemyCount = (int)Mathf.Log(level, 2f);
		LayoutObjectsAtRandom(enemyTiles, new Count(enemyCount, enemyCount));
		Instantiate(exit, new Vector3(columns-1, rows-1, 0f), Quaternion.identity);
	}

	void Start () {
	
	}

	void Update () {

	}
}
