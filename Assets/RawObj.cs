using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Article", menuName="raw")]
public class RawObj : ScriptableObject {
	public string Title;
	public string Summary;
	public float posID; //where are you in the raw list
	public float uniqueID;

}
