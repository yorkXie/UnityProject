using UnityEngine;
using System.Collections;

public class RayShooter : MonoBehaviour {
	[SerializeField] private AudioSource soundSource;	
	//引用了要播放的两个声音文件
	[SerializeField] private AudioClip hitWallSound;
	[SerializeField] private AudioClip hitEnemySound;

	private Camera _camera;

	void Start() {
		_camera = GetComponent<Camera>();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void OnGUI() {
		int size = 12;
		float posX = _camera.pixelWidth/2 - size/4;
		float posY = _camera.pixelHeight/2 - size/2;
		GUI.Label(new Rect(posX, posY, size, size), "*");
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 point = new Vector3(_camera.pixelWidth/2, _camera.pixelHeight/2, 0);
			Ray ray = _camera.ScreenPointToRay(point);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				GameObject hitObject = hit.transform.gameObject;
				ReactiveTarget target = hitObject.GetComponent<ReactiveTarget>();
				//如果目标不为空, 玩家击中敌人
				if (target != null) {
					target.ReactToHit();
					//调用PlayOneShot()播放击中敌人声音
					soundSource.PlayOneShot(hitEnemySound);
				} else {
					StartCoroutine(SphereIndicator(hit.point));
					//玩家未击中敌人, 调用PlayOneShot()播放击中墙声音
					soundSource.PlayOneShot(hitWallSound);
				}
			}
		}
	}

	private IEnumerator SphereIndicator(Vector3 pos) {
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere.transform.position = pos;

		yield return new WaitForSeconds(1);

		Destroy(sphere);
	}
}