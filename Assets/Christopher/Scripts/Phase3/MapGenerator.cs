using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour {
    
    public TileBase blackTile;
    public Vector2Int matrixSize;
    public float perlinScale;
    public float noiseValueToPutBlackBlock = 0.5f;

    private Tilemap _mainTilemap;
    private bool[,] _matrix;
    private bool[,] _tmpMatrix;
    private void Awake() {
        _mainTilemap = GetComponent<Tilemap>();
    }

    [ExecuteAlways]
    private void Start() {
        InitFromPerlinNoise();
    }
    private void InitFromPerlinNoise() {
        _matrix = new bool[matrixSize.x, matrixSize.y];
        for (int x = 0; x < matrixSize.x; x++) {
            for (int y = 0; y < matrixSize.y; y++) {
                float noiseValue = Mathf.PerlinNoise(x * perlinScale, y * perlinScale);
                _matrix[x, y] = noiseValue > noiseValueToPutBlackBlock;
            }
        }
        DisplayMatrix(_matrix);
    }
    private void DisplayMatrix(bool[,] matrix) {
        for (int x = 0; x < matrix.GetLength(0); x++) {
            for (int y = 0; y < matrix.GetLength(1); y++) {
                Vector3Int tilePosition = new Vector3Int(x, y, 0); // Assuming z-axis is 0 for 2D tilemap
                _mainTilemap.SetTile(tilePosition, matrix[x, y] ? blackTile : null);
            }
        }
        _mainTilemap.RefreshAllTiles();
    }

}