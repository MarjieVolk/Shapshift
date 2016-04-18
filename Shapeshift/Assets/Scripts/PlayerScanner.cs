﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TileItem))]
public class PlayerScanner : MonoBehaviour {

    public float scanCompletionSeconds = 2;

    private PlayableFurnitureItem currentlyScanning;
    private float scanStartTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            PlayableFurnitureItem toScan = findItemToScan();

            if (toScan == null) {
                // TODO -- play bad sound effect?
            } else if (currentlyScanning == toScan) {
                // Continue scanning
                if (Time.time - scanStartTime >= scanCompletionSeconds) {
                    // Finish scanning
                    UnlockState.INSTANCE.completeScanOn(currentlyScanning.furnitureType);
                }
            } else {
                // Start new scan
                currentlyScanning = toScan;
                scanStartTime = Time.time;
            }
        }
    }

    private PlayableFurnitureItem findItemToScan() {
        TileItem ti = gameObject.GetComponent<TileItem>();

        List<PlayableFurnitureItem> potentialScans = new List<PlayableFurnitureItem>();
        for (int x = ti.tileX - 1; x <= ti.tileX + 1; x++) {
            for (int y = ti.tileY - 1; y <= ti.tileY + 1; y++) {
                foreach (PlayableFurnitureItem item in TileItem.GetObjectsAtPosition<PlayableFurnitureItem>(x, y)) {
                    if (!item.hasBeenScanned) {
                        potentialScans.Add(item);
                    }
                }
            }
        }

        if (potentialScans.Count == 0) {
            return null;
        } else if (potentialScans.Contains(currentlyScanning)) {
            return currentlyScanning;
        } else {
            // TODO - intelligently pick a good thing to scan
            return potentialScans[0];
        }
    }
}