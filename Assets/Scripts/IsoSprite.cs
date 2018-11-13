using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoSprite : MonoBehaviour {

    IsoGrid currentGrid;
    SpriteRenderer renderer;
    public CapsuleCollider collider;

    public bool automaticSorting = true;
    public int sortingOffset = 0;
    public SpriteRenderer alwaysOnBehindOf;

    public event Action OnAutoSort;

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

            if (automaticSorting) {
                if (collider != null) {
                    renderer.sortingOrder = currentGrid.CalculateSortingOrder(new Vector3(collider.bounds.min.x, transform.position.y, collider.bounds.min.z)) + sortingOffset;
                } else {
                    renderer.sortingOrder = currentGrid.CalculateSortingOrder(new Vector3(transform.position.x, transform.position.y, transform.position.z)) + sortingOffset;
                }

                if(OnAutoSort != null) {
                    OnAutoSort();
                }
            }
        }
	}

    private void LateUpdate() {
        if (automaticSorting && alwaysOnBehindOf != null) {
            if (renderer.sortingOrder >= alwaysOnBehindOf.sortingOrder) {
                renderer.sortingOrder = alwaysOnBehindOf.sortingOrder - 1;
            }
        }
    }
}
