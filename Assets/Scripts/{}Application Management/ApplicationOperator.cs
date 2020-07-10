using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationOperator : MonoBehaviour
{
	public void Quit() => Application.Quit();
	public void Quit(int exitCode) => Application.Quit(exitCode: exitCode);
}
