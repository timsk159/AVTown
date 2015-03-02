using UnityEngine;
using System.Collections;
using ExitGames;

public class Popup : MonoBehaviour 
{
	protected Animator animator;
	void Start()
	{
		animator = GetComponent<Animator>();
		animator.SetTrigger("Show");
		InternalStart();
	}

	void OnDestroy()
	{
		InternalOnDestroy();
	}
	protected virtual void InternalStart() { }
	protected virtual void InternalOnDestroy() { }
	public virtual void Close() { }
	public virtual void Open() { }
}
