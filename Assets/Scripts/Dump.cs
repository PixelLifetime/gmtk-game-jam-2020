using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dump : MonoBehaviour
{
	private void Start()
	{
		TooltipView.Instance.Display(position: this.transform.position, text: "Press\nE");
	}
}
