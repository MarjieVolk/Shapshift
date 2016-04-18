﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileItem : MonoBehaviour
{
    public static float TILE_SIZE = 0.32f;
    private static Dictionary<long, List<GameObject>> _tileMap = new Dictionary<long, List<GameObject>>();

    public int tileX { get; private set; }
    public int tileY { get; private set; }

    public int startingTileWidth = 1;
    public int startingTileHeight = 1;

    private int _tileW;
    public int tileW {
        get { return _tileW; }
        set { _SetSize(value, _tileH); }
    }
    private int _tileH;
    public int tileH {
        get { return _tileH; }
        set { _SetSize (_tileW, value); }
    }

    public void Awake () {
        _tileW = startingTileWidth;
        _tileH = startingTileHeight;
        transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.y);
        SnapToGrid ();
        AddToTileMap ();
    }

    void OnDestroy() {
        RemoveFromTileMap();
    }

    public void SnapToGrid () {
        // Set tile position based off of starting transform.
        tileX = GlobalToTilePosition (transform.position.x);
        tileY = GlobalToTilePosition (transform.position.y);

        // Snap transform to grid.
        transform.position = new Vector3(tileX * TILE_SIZE, tileY * TILE_SIZE, transform.position.z);
    }

    private static long ToKey(int x, int y) {
        return (((long)x) << 32) + y;
    }

    public static int GlobalToTilePosition(float p) {
        return Mathf.RoundToInt (p / TILE_SIZE);
    }

    public static float TileToGlobalPosition(int p) {
        return p * TILE_SIZE;
    }

    /**
     * Call anytime you want to change position in order to update tile positions.
     * Overwrites z.
     */
    public void SetGlobalPosition(Vector3 newPos) {
        _SetPosition (GlobalToTilePosition (newPos.x), GlobalToTilePosition (newPos.y), newPos);
    }

    /**
     * Set the tile position and underlying transform; update the tile map.
     */
    public void SetTilePosition(int newTileX, int newTileY) {
        _SetPosition (newTileX, newTileY, new Vector3(newTileX * TILE_SIZE, newTileY * TILE_SIZE));
    }

    private void _SetPosition(int newTileX, int newTileY, Vector3 newPos) {
        bool tilePosMoved = newTileX != tileX || newTileY != tileY;
        if (tilePosMoved) {
            RemoveFromTileMap ();
        }
        tileX = newTileX;
        tileY = newTileY;
        transform.position = new Vector3(newPos.x, newPos.y, newPos.y);
        if (tilePosMoved) {
            AddToTileMap ();
        }
    }

    private void _SetSize(int newWidth, int newHeight) {
        bool tileResized = newWidth != _tileW || newHeight != _tileH;
        if (tileResized) {
            RemoveFromTileMap ();
        }
        _tileW = newWidth;
        _tileH = newHeight;
        if (tileResized) {
            AddToTileMap ();
        }
    }

    private void RemoveFromTileMap() {
        List<GameObject> ents;
        ForEachOfMyTiles ((int x, int y) => {
            if (_tileMap.TryGetValue (ToKey (x, y), out ents)) {
                ents.Remove (gameObject);
            }
        });
    }

    private void AddToTileMap() {
<<<<<<< HEAD
      //Debug.Log("AddToTileMap " + this + " at " + new Tile(tileX, tileY));
      if (tileX == 0 && tileY == 0) {
        Debug.Log(Environment.StackTrace);
      }
=======
>>>>>>> c4a5b112a46a3180d7f0c853ebcda55cdbf5c525
        List<GameObject> ents;
        ForEachOfMyTiles ((int x, int y) => {
            long key = ToKey (x, y);
            if (_tileMap.TryGetValue (key, out ents)) {
                ents.Add (gameObject);
            } else {
                ents = new List<GameObject>();
                ents.Add(gameObject);
                _tileMap.Add(key, ents);
            }
        });
    }

    private void ForEachOfMyTiles(Action<int, int> func) {
        for (int ix = 0; ix < tileW; ix++) {
            for (int iy = 0; iy < tileH; iy++) {
                func (tileX + ix, tileY + iy);
            }
        }
    }

    public static bool DoesPlacementCollideWithThings(int tileX, int tileY, int tileW, int tileH) {
<<<<<<< HEAD
      //Debug.Log("DoesPlacementCollideWithThings " + tileX + ", " + tileY + ", " + tileW + ", " + tileH);
      for (int ix = 0; ix < tileW; ix++) {
        for (int iy = 0; iy < tileH; iy++) {
          if (GetObjectsAtPosition<Wall>(tileX + ix, tileY + iy).Count > 0) {
            //Debug.Log("Placement collides with wall");
            return true;
          } else if (GetObjectsAtPosition<FurnitureItem>(tileX + ix, tileY + iy).Count > 0) {
            //Debug.Log("Placement collides with furniture");
            return true;
          } else if (GetObjectsAtPosition<Guard>(tileX + ix, tileY + iy).Count > 0) {
            //Debug.Log("Placement collides with guard");
=======
      // Debug.Log("DoesPlacementCollideWithThings " + tileX + ", " + tileY + ", " + tileW + ", " + tileH);
      for (int ix = 0; ix < tileW; ix++) {
        for (int iy = 0; iy < tileH; iy++) {
          int x = tileX + ix;
          int y = tileY + iy;
          if (GetObjectsAtPosition<Wall>(x, y).Count > 0) {
            return true;
          } else if (GetObjectsAtPosition<FurnitureItem>(x, y).Count > 0) {
            return true;
          } else if (GetObjectsAtPosition<Guard>(x, y).Count > 0) {
>>>>>>> c4a5b112a46a3180d7f0c853ebcda55cdbf5c525
            return true;
          }
        }
      }
      return false;
    }

    public static List<T> GetObjectsAtPosition<T>(int tileX, int tileY) where T: Component {
        List<T> matchingEntities = new List<T>();
        List<GameObject> allEntities;
        if (_tileMap.TryGetValue (ToKey (tileX, tileY), out allEntities)) {
            foreach (GameObject go in allEntities) {
                T nullableT = go.GetComponent<T> ();
                if (nullableT != null) {
                    matchingEntities.Add (nullableT);
                }
            }
        }
        return matchingEntities;
    }
}

