using Godot;
using System;
using System.Runtime.Intrinsics.Arm;

public partial class Dummy : Enemy
{
	public override void _Ready()
	{
		gotHit = GetNode<Timer>("GotHit");
		fireHit = GetNode<Timer>("FireHit");
		health = 100;
		flamability = 10;
	}
}