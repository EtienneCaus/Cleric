using Godot;
using System;
using System.Runtime.Intrinsics.Arm;

public partial class Dummy : StaticBody3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void GetHit()
	{
		Random rnd = new();
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "offset", new Vector2(rnd.NextSingle()-0.5f,-rnd.NextSingle()), 0.02f);
		tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "offset", new Vector2(-rnd.NextSingle()+0.5f,rnd.NextSingle()), 0.02f);
		tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "offset", new Vector2(rnd.NextSingle()-0.5f,-rnd.NextSingle()), 0.02f);
		tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "offset", new Vector2(-rnd.NextSingle()+0.5f,-rnd.NextSingle()), 0.02f);
		tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "offset", Vector2.Zero, 0.02f);
	}
}