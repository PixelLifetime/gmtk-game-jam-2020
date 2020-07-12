using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripIndicatorController : MonoBehaviourSingleton<GripIndicatorController>
{
	[SerializeField] private DiscreteIndicator _discreteIndicator;
	public DiscreteIndicator _DiscreteIndicator => this._discreteIndicator;

	public int Value
	{
		get => this._discreteIndicator.Value;
		set => this._discreteIndicator.Value = value;
	}
}
