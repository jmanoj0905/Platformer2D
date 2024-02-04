using System.Collections;
using UnityEngine;

public class TimeManipulation : MonoBehaviour
{
	private bool isTimeSlowed = false;
	private float originalTimeScale;

	private void Start()
	{
		originalTimeScale = Time.timeScale;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			StartCoroutine(SlowTime());
		}

		if (Input.GetMouseButtonUp(1))
		{
			ResetTimeScale();
		}
	}

	private IEnumerator SlowTime()
	{
		isTimeSlowed = true;
		Time.timeScale = 0.5f; // You can adjust the slowdown factor

		while (isTimeSlowed && Input.GetMouseButton(1))
		{
			yield return null;
		}

		ResetTimeScale();
	}

	private void ResetTimeScale()
	{
		isTimeSlowed = false;
		Time.timeScale = originalTimeScale;
	}
}
