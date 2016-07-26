using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Linq;

public class BoardManager : MonoBehaviour {
	public int columns = 8;
	public int rows = 8;
	public GameObject exit;
	public GameObject[] floorTiles;
	public GameObject[] innerWallTiles;
	public GameObject[] outerWallTiles;
	public GameObject[] enemyTiles;
	public GameObject[] foodTiles;

	private Transform boardHolder;

	public void SetupScene(int level) {
		boardHolder = new GameObject("Board").transform;
		SetupFloorAndOuterWalls ();
		SetupLevelContents (level);
		Instantiate(exit, new Vector3(columns-1, rows-1, 0f), Quaternion.identity);
	}

	private void SetupFloorAndOuterWalls() {
		for (int x = -1; x < columns +1; x++) {
			for (int y = -1; y < rows +1; y++) {
				GameObject tile;
				if (IsPartOfOuterWalls (x, y)) {
					tile = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
				} else {
					tile = floorTiles [Random.Range (0, floorTiles.Length)];
				}
				GameObject instance = Instantiate(tile, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
				instance.transform.SetParent(boardHolder);
			}
		}
	}

	private void SetupLevelContents(int level) {
		GameObject[] innerWalls = PickRandomly (innerWallTiles, Random.Range (5, 9));
		GameObject[] foods = PickRandomly (foodTiles, Random.Range (1, 5));
		GameObject[] enemies = PickRandomly (enemyTiles, (int)Mathf.Log (level, 2f));
		GameObject[] levelContents = MergeArrays (innerWalls, foods, enemies);
		List<Vector3> availablePositions = GetPositionsForRandomlyPlacedItems ();
		LayoutObjectsAtRandom(availablePositions, levelContents);
	}

	private bool IsPartOfOuterWalls(int x, int y) {
		return x == -1 || x == columns || y == -1 || y == rows;
	}

	private List<Vector3> GetPositionsForRandomlyPlacedItems() {
		List<Vector3> positions = new List<Vector3> ();
		// Ensure there's always a route to exit by not placing any objects on outer rim
		for (int x = 1; x < columns-1; x++) {
			for (int y = 1; y < rows-1; y++) {
				positions.Add(new Vector3(x, y, 0f));
			}
		}
		return positions;
	}

	void LayoutObjectsAtRandom(List<Vector3> availablePositions, GameObject[] gameObjects) {
		foreach (GameObject gameObject in gameObjects) {
			int randomIndex = Random.Range(0, availablePositions.Count);
			Vector3 randomPosition = availablePositions[randomIndex];
			availablePositions.RemoveAt(randomIndex);
			Instantiate(gameObject, randomPosition, Quaternion.identity);
		}
	}

	private T[] PickRandomly<T>(T[] elements, int amount) {
		T[] newArray = new T[amount];
		for (int i = 0; i < newArray.Length; i++) {
			newArray [i] = elements [Random.Range (0, elements.Length)];
		}
		return newArray;
	}

	private T[] MergeArrays<T>(params T[][] arrays) {
		IEnumerable<T> mergeResult = new T[0].AsEnumerable();
		foreach (T[] array in arrays) {
			mergeResult = mergeResult.Concat (array);
		}
		return mergeResult.ToArray ();
	}
}
