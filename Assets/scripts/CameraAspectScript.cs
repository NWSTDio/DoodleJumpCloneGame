using UnityEngine;

public class CameraAspectScript : MonoBehaviour {
	[SerializeField] Vector2 DefaultResolution = new Vector2(720, 1280);
	[SerializeField, Range(0f, 1f)] float WidthOrHeight = 0;

	private Camera cam;

	private float initialSize, targetAspect;
	private float initialFov, horizontalFov = 120f;

	void Start() {
		cam = GetComponent<Camera>();

		initialSize = cam.orthographicSize;
		targetAspect = DefaultResolution.x / DefaultResolution.y;

		initialFov = cam.fieldOfView;
		horizontalFov = CalcVerticalFov(initialFov, 1 / targetAspect);
		}
	void Update() {
		if (cam.orthographic)
			cam.orthographicSize = Mathf.Lerp(initialSize * (targetAspect / cam.aspect), initialSize, WidthOrHeight);
		else
			cam.fieldOfView = Mathf.Lerp(CalcVerticalFov(horizontalFov, cam.aspect), initialFov, WidthOrHeight);
		}
	float CalcVerticalFov(float hFovInDeg, float aspectRatio) {
		return 2 * Mathf.Atan(Mathf.Tan(hFovInDeg * Mathf.Deg2Rad / 2) / aspectRatio) * Mathf.Rad2Deg;
		}
	}