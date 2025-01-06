using Godot;
using System;
using System.Runtime.Intrinsics.Arm;

public partial class Dummy : CharacterBody3D
{
	int health = 100;
	Timer gotHit;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gotHit = GetNode<Timer>("GotHit");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void GetHit(float force)
	{
		if(gotHit.IsStopped())
		{
			gotHit.Start();
			health -= (int)force;

			if(health <= 0)
			{
				//GetNode<GpuParticles3D>("GPUParticles3D").Emitting = true;
				QueueFree();
			}
			else
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
	}
}