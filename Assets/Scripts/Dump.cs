using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dump : MonoBehaviour
{
	[Header("Input Action")]
	public InputAction InputAction;

	[Header("Input Action Asset")]
	public InputActionAsset InputActions;

	[Header("Input Action Reference")]
	public InputActionReference InputActionReference;

	private void Start()
	{
		TooltipView.Instance.Display(position: this.transform.position, text: "Press\nE");
	}
}
