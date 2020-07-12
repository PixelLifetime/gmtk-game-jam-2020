using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousIndicator : Indicator<float, UnityEventFloat>
{
	protected override float Clamp(float value, float min, float max) => Mathf.Clamp(value: value, min: min, max: max);
}
