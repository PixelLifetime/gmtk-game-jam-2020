using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteView : View
{
	[SerializeField] private TextMeshProUGUI _textField;
	public TextMeshProUGUI _TextField => this._textField;

	public void Display(Note note)
	{
		this._textField.text = note._Text;

		//TODO: pause the game.

		this.Open();
		this.Show();
	}

	public override void Hide()
	{
		base.Hide();

		//TODO: unpause the game.
	}
}