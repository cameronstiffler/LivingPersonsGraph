using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {
	public GameObject target;
	public float xOffset = 0;
	public float yOffset = 0;
	public float zOffset = 0;

	void LateUpdate() {
		this.transform.position = new Vector3(target.transform.position.x + xOffset,
			target.transform.position.y + yOffset,
			target.transform.position.z + zOffset);
		if (this.transform.rotation.y > 0) {
			//this.transform.rotation = Quaternion.Euler (0, .01f, 0);
			//transform.Rotate(0,-Time.deltaTime*2, 0);
		}
		if (xOffset > -15) {
			//xOffset -= .05f;
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//zOffset -= .1f;

	}
}
