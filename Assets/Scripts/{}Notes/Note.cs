using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "[Note]", menuName = "[Narrative]/[Note]")]
public class Note : ScriptableObject
{
	[TextArea]
	[SerializeField] private string _text;
	public string _Text => this._text;
}
