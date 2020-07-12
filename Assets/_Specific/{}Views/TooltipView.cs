using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipView : View
{
	public static TooltipView Instance { get; private set; }

	[SerializeField] private TextMeshProUGUI _textField;
	public TextMeshProUGUI _TextField => this._textField;

	[SerializeField] private Vector3 _offset;
	public Vector3 _Offset => this._offset;

	private Camera _camera;

	public void Display(Vector3 position, string text)
	{
		this.transform.position = this._camera.WorldToScreenPoint(position: position) + this._offset;

		this._textField.text = text;

		this.Open();
	}

	protected override void Awake()
	{
		base.Awake();

		if (Instance == null)
		{
			Instance = this;
		}

		this._camera = Camera.main;
	}
}
