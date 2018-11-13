using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoSprite : MonoBehaviour {

    IsoGrid currentGrid;
    SpriteRenderer renderer;
    public CapsuleCollider collider;

	void Awake () {
        currentGrid = FindObjectOfType<IsoGrid>();
        renderer = GetComponent<SpriteRenderer>();
    }
	
	void Update () {
        transform.rotation = Camera.main.transform.rotation;
        if(currentGrid != null) {
            //renderer.sortingOrder = currentGrid.CalculateSortingOrder(transform.position + Vector3.up * 0f) + 1;

            // En lugar de tomar la posicion, tomar la posicion "más enfrente" (X y Y negativos)

            //Debug.Log(new Vector3(collider.bounds.min.x, transform.position.y, collider.bounds.min.z));
            renderer.sortingOrder = currentGrid.CalculateSortingOrder(new Vector3(collider.bounds.min.x, transform.position.y, collider.bounds.min.z)) + 1;
        }
	}
}
