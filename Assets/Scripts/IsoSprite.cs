using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoSprite : MonoBehaviour {

    IsoGrid currentGrid;
    SpriteRenderer renderer;

	void Start () {
        currentGrid = FindObjectOfType<IsoGrid>();
        renderer = GetComponent<SpriteRenderer>();
    }
	
	void Update () {
        transform.rotation = Camera.main.transform.rotation;
        if(currentGrid != null) {
            renderer.sortingOrder = currentGrid.CalculateSortingOrder(transform.position) + 2;
        }
	}
}
