using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using TMPro;

public class PixelPos {

	public PixelPos(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public int x = 0;
	public int y = 0;
}

public class LevelGenerator : MonoBehaviour {

	private Dictionary<PixelPos, string> map = new Dictionary<PixelPos, string>();

	public Grid grid;
	public Transform spawnPoint;
	public Texture2D[] levels;

	public Tilemap floorMap;
	public Tilemap wallMap;
	public Tilemap propsMap;

	public GameObject doorPrefab;
	public GameObject enemyPrefab;
	public GameObject healthPrefab;
	public GameObject endComputerPrefab;

	public TileBase baseFloor;
	public TileBase topRightFloor;
	public TileBase topLeftFloor;
	public TileBase bottomRightFloor;
	public TileBase bottomLeftFloor;
	public TileBase topFloor;
	public TileBase bottomFloor;
	public TileBase rightFloor;
	public TileBase leftFloor;
	public TileBase leftRightFloor;
	public TileBase topBottomFloor;
	public TileBase leftWall;
	public TileBase rightWall;
	public TileBase topWall;
	public TileBase bottomWall;
	public TileBase topLeftCorner;
	public TileBase topRightCorner;
	public TileBase bottomLeftCorner;
	public TileBase bottomRightCorner;
	public TileBase innerBottomRightCorner;
	public TileBase innerBottomLeftCorner;
	public TileBase innerTopLeftCorner;
	public TileBase innerTopRightCorner;
	public TileBase pillar;
	public TileBase table;

	public int baseLevel = 0;
	public bool testLevel = false;

	void Start() {
		GenerateMap();
	}

	public void GenerateMap() {
		int currentLevel = baseLevel;
		if(GameManager.instance != null) {
			currentLevel = GameManager.instance.CurrentLevel;
		}

		if(testLevel)
			currentLevel = baseLevel;

		spawnPoint.GetComponentInChildren<TextMeshPro>().text = "level " + (currentLevel + 1);

		Texture2D lvl = levels[currentLevel];
		float centerX = (float)lvl.width/2;
		float centerY = (float)lvl.height/2;

		var data = lvl.GetRawTextureData<Color32>();

		floorMap.ClearAllTiles();
		wallMap.ClearAllTiles();
		propsMap.ClearAllTiles();

		for (int y = 0; y < lvl.height; y++) {
            for (int x = 0; x < lvl.width; x++) {
				string cHex = GetHex(lvl, x, y);
				PixelPos pp = new PixelPos(x, y);
				map.Add(pp, cHex);

				if(HexEquals(cHex, 'F', 'F')) {
					floorMap.SetTile(new Vector3Int(x, y, 0), baseFloor);

					if(HexEquals(cHex, 'F', 'F', 2)) {
						spawnPoint.localPosition = new Vector3(x + 0.5f, y + 0.5f, 0);
					}

					string leftHex = GetHex(lvl, x - 1, y);
					if(!HexEquals(leftHex, 'F', 'F')) {
						// No left neighbour, populate it
						wallMap.SetTile(new Vector3Int(x - 1, y, 0), leftWall);
					}

					string rightHex = GetHex(lvl, x + 1, y);
					if(!HexEquals(rightHex, 'F', 'F')) {
						// No right neighbour, populate it
						wallMap.SetTile(new Vector3Int(x + 1, y, 0), rightWall);
					}

					string topHex = GetHex(lvl, x, y + 1);
					if(!HexEquals(topHex, 'F', 'F')) {
						// No top neighbour, populate it
						wallMap.SetTile(new Vector3Int(x, y + 1, 0), topWall);
					}

					string bottomhex = GetHex(lvl, x, y - 1);
					if(!HexEquals(bottomhex, 'F', 'F')) {
						// No bottom neighbour, populate it
						wallMap.SetTile(new Vector3Int(x, y - 1, 0), bottomWall);
					}

					string topLeftHex = GetHex(lvl, x - 1, y + 1);
					if(!HexEquals(topLeftHex, 'F', 'F')) {
						// No top left neighbour, populate it
						if(!wallMap.HasTile(new Vector3Int(x - 1, y + 1, 0))) {
							wallMap.SetTile(new Vector3Int(x - 1, y + 1, 0), topLeftCorner);
						}
					}

					string topRightHex = GetHex(lvl, x + 1, y + 1);
					if(!HexEquals(topRightHex, 'F', 'F')) {
						// No top right neighbour, populate it
						if(!wallMap.HasTile(new Vector3Int(x + 1, y + 1, 0))) {
							wallMap.SetTile(new Vector3Int(x + 1, y + 1, 0), topRightCorner);
						}
					}

					string bottomLeftHex = GetHex(lvl, x - 1, y - 1);
					if(!HexEquals(bottomLeftHex, 'F', 'F')) {
						// No bottom left neighbour, populate it
						if(!wallMap.HasTile(new Vector3Int(x - 1, y - 1, 0))) {
							wallMap.SetTile(new Vector3Int(x - 1, y - 1, 0), bottomLeftCorner);
						}
					}

					string bottomRightHex = GetHex(lvl, x + 1, y - 1);
					if(!HexEquals(bottomRightHex, 'F', 'F')) {
						// No bottom right neighbour, populate it
						if(!wallMap.HasTile(new Vector3Int(x + 1, y - 1, 0))) {
							wallMap.SetTile(new Vector3Int(x + 1, y - 1, 0), bottomRightCorner);
						}
					}
				}
			}
		}

		for (int y = 0; y < lvl.height; y++) {
            for (int x = 0; x < lvl.width; x++) {
				string cHex = GetHex(lvl, x, y);

				// Corners
				if(wallMap.HasTile(new Vector3Int(x,y,0))) {
					// inner bottom right corner
					if(!wallMap.HasTile(new Vector3Int(x + 1, y, 0)) &&
						wallMap.HasTile(new Vector3Int(x, y + 1, 0)) &&
						wallMap.HasTile(new Vector3Int(x - 1, y, 0)) &&
						!wallMap.HasTile(new Vector3Int(x, y - 1, 0)) &&
						floorMap.HasTile(new Vector3Int(x + 1, y, 0))) {
						wallMap.SetTile(new Vector3Int(x, y, 0), innerBottomRightCorner);
					}

					// inner bottom left corner
					if(!wallMap.HasTile(new Vector3Int(x - 1, y, 0)) &&
						wallMap.HasTile(new Vector3Int(x, y + 1, 0)) &&
						wallMap.HasTile(new Vector3Int(x + 1, y, 0)) &&
						!wallMap.HasTile(new Vector3Int(x, y - 1, 0)) &&
						floorMap.HasTile(new Vector3Int(x - 1, y, 0))) {
						wallMap.SetTile(new Vector3Int(x, y, 0), innerBottomLeftCorner);
					}

					// inner top right corner
					if(!wallMap.HasTile(new Vector3Int(x + 1, y, 0)) &&
						wallMap.HasTile(new Vector3Int(x, y - 1, 0)) &&
						wallMap.HasTile(new Vector3Int(x - 1, y, 0)) &&
						!wallMap.HasTile(new Vector3Int(x, y + 1, 0)) &&
						floorMap.HasTile(new Vector3Int(x + 1, y, 0))) {
						wallMap.SetTile(new Vector3Int(x, y, 0), innerTopRightCorner);
					}

					// inner top left corner
					if(!wallMap.HasTile(new Vector3Int(x - 1, y, 0)) &&
						wallMap.HasTile(new Vector3Int(x, y - 1, 0)) &&
						wallMap.HasTile(new Vector3Int(x + 1, y, 0)) &&
						!wallMap.HasTile(new Vector3Int(x, y + 1, 0)) &&
						floorMap.HasTile(new Vector3Int(x - 1, y, 0))) {
						wallMap.SetTile(new Vector3Int(x, y, 0), innerTopLeftCorner);
					}
				}

				// floors
				if(floorMap.HasTile(new Vector3Int(x, y, 0))) {
					// top right
					if(!wallMap.HasTile(new Vector3Int(x - 1, y, 0)) &&
						!wallMap.HasTile(new Vector3Int(x, y - 1, 0)) &&
						wallMap.HasTile(new Vector3Int(x + 1, y, 0)) &&
						wallMap.HasTile(new Vector3Int(x, y + 1, 0))) {
						floorMap.SetTile(new Vector3Int(x, y, 0), topRightFloor);
					}

					// top left
					if(!wallMap.HasTile(new Vector3Int(x + 1, y, 0)) &&
						!wallMap.HasTile(new Vector3Int(x, y - 1, 0)) &&
						wallMap.HasTile(new Vector3Int(x - 1, y, 0)) &&
						wallMap.HasTile(new Vector3Int(x, y + 1, 0))) {
						floorMap.SetTile(new Vector3Int(x, y, 0), topLeftFloor);
					}

					// bottom right
					if(!wallMap.HasTile(new Vector3Int(x - 1, y, 0)) &&
						!wallMap.HasTile(new Vector3Int(x, y + 1, 0)) &&
						wallMap.HasTile(new Vector3Int(x + 1, y, 0)) &&
						wallMap.HasTile(new Vector3Int(x, y - 1, 0))) {
						floorMap.SetTile(new Vector3Int(x, y, 0), bottomRightFloor);
					}

					// bottom left
					if(!wallMap.HasTile(new Vector3Int(x + 1, y, 0)) &&
						!wallMap.HasTile(new Vector3Int(x, y + 1, 0)) &&
						wallMap.HasTile(new Vector3Int(x - 1, y, 0)) &&
						wallMap.HasTile(new Vector3Int(x, y - 1, 0))) {
						floorMap.SetTile(new Vector3Int(x, y, 0), bottomLeftFloor);
					}

					// top
					if(wallMap.HasTile(new Vector3Int(x, y + 1, 0)) &&
						!wallMap.HasTile(new Vector3Int(x, y - 1, 0)) &&
						!wallMap.HasTile(new Vector3Int(x - 1, y, 0)) &&
						!wallMap.HasTile(new Vector3Int(x + 1, y, 0))) {
						floorMap.SetTile(new Vector3Int(x, y, 0), topFloor);
					}

					// bottom
					if(wallMap.HasTile(new Vector3Int(x, y - 1, 0)) &&
						!wallMap.HasTile(new Vector3Int(x, y + 1, 0)) &&
						!wallMap.HasTile(new Vector3Int(x - 1, y, 0)) &&
						!wallMap.HasTile(new Vector3Int(x + 1, y, 0))) {
						floorMap.SetTile(new Vector3Int(x, y, 0), bottomFloor);
					}

					// right
					if(wallMap.HasTile(new Vector3Int(x + 1, y, 0)) &&
						!wallMap.HasTile(new Vector3Int(x, y - 1, 0)) &&
						!wallMap.HasTile(new Vector3Int(x, y + 1, 0)) &&
						!wallMap.HasTile(new Vector3Int(x - 1, y, 0))) {
						floorMap.SetTile(new Vector3Int(x, y, 0), rightFloor);
					}

					// left
					if(wallMap.HasTile(new Vector3Int(x - 1, y, 0)) &&
						!wallMap.HasTile(new Vector3Int(x, y - 1, 0)) &&
						!wallMap.HasTile(new Vector3Int(x, y + 1, 0)) &&
						!wallMap.HasTile(new Vector3Int(x + 1, y, 0))) {
						floorMap.SetTile(new Vector3Int(x, y, 0), leftFloor);
					}

					// left right
					if(wallMap.HasTile(new Vector3Int(x - 1, y, 0)) &&
						!wallMap.HasTile(new Vector3Int(x, y - 1, 0)) &&
						!wallMap.HasTile(new Vector3Int(x, y + 1, 0)) &&
						wallMap.HasTile(new Vector3Int(x + 1, y, 0))) {
						floorMap.SetTile(new Vector3Int(x, y, 0), leftRightFloor);
					}

					// top bottom
					if(wallMap.HasTile(new Vector3Int(x, y + 1, 0)) &&
						!wallMap.HasTile(new Vector3Int(x - 1, y, 0)) &&
						!wallMap.HasTile(new Vector3Int(x + 1, y, 0)) &&
						wallMap.HasTile(new Vector3Int(x, y - 1, 0))) {
						floorMap.SetTile(new Vector3Int(x, y, 0), topBottomFloor);
					}
				}

				// Doors
				if(HexEquals(cHex, 'A', 'B', 2)) {
					bool flipY = wallMap.HasTile(new Vector3Int(x, y - 1, 0));

					GameObject door = Instantiate(doorPrefab);
					door.transform.parent = floorMap.transform.parent;
					door.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);

					door.GetComponent<SpriteRenderer>().flipY = flipY;
					door.GetComponent<Door>().nextLevelDoor = flipY;

					door.SetActive(true);
				}

				// Pillar
				if(HexEquals(cHex, '8', '3', 2)) {
					wallMap.SetTile(new Vector3Int(x, y, 0), pillar);
				} else if(HexEquals(cHex, '5', '0', 2)) {
					// table
					propsMap.SetTile(new Vector3Int(x, y, 0), table);
				}

				if(HexEquals(cHex, 'F', 'F', 4)) {
					GameObject e = Instantiate(enemyPrefab);
					float xPos = (x + 0.5f) - centerX;
					float yPos = (y + 0.5f) - centerY;
					e.transform.position = new Vector3(xPos, yPos, 0);
					e.SetActive(true);
				} else if(HexEquals(cHex, 'F', 'E', 4)) {
					GameObject e = Instantiate(enemyPrefab);
					e.GetComponent<BasicEnemy>().SetBoss();
					float xPos = (x + 0.5f) - centerX;
					float yPos = (y + 0.5f) - centerY;
					e.transform.position = new Vector3(xPos, yPos, 0);
					e.SetActive(true);
				}  else if(HexEquals(cHex, '5', 'F', 4)) {
					GameObject e = Instantiate(healthPrefab);
					float xPos = (x + 0.5f) - centerX;
					float yPos = (y + 0.5f) - centerY;
					e.transform.position = new Vector3(xPos, yPos, 0);
					e.SetActive(true);
				} else if(HexEquals(cHex, '4', '0', 4)) {
					// end computer
					GameObject e = Instantiate(endComputerPrefab);
					float xPos = (x + 0.5f) - centerX;
					float yPos = (y + 0.5f) - centerY;
					e.transform.position = new Vector3(xPos, yPos, 0);
					e.SetActive(true);
				}
			}
		}

		floorMap.transform.parent.localPosition = new Vector3(-centerX, -centerY, 0);

		Player.Create(spawnPoint.position);
		CameraManager.instance.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, CameraManager.instance.transform.position.z);
		CameraManager.instance.player = Player.instance;

		floorMap.RefreshAllTiles();
		wallMap.RefreshAllTiles();

		Invoke("GenerateGrid", 0.3f);
	}

	void GenerateGrid() {
		grid.Generate();
	}

	public bool HexEquals(string hex, char c0, char c1, int i = 0, bool debug = false) {
		if(debug) Debug.Log(hex[i] + " " + c0 +  " " + hex[i + 1] +  " " + c1);
		return hex[i] == c0 && hex[i + 1] == c1;
	}

	public string GetHex(Texture2D lvl, int x, int y) {
		return ColorUtility.ToHtmlStringRGBA(lvl.GetPixel(x, y));
	}
}
