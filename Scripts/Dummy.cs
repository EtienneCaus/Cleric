using Godot;
using System;
using System.Runtime.Intrinsics.Arm;

public partial class Dummy : Enemy
{
	public override void _Ready()
	{
		gotHit = GetNode<Timer>("GotHit");
		health = 100;
	}
}