using Godot;
using System;

public abstract partial class Enemy : CharacterBody3D
{
	protected int health;
	protected Timer gotHit;
	public void GetHit(float force)
	{
		if(gotHit.IsStopped())
		{
			gotHit.Start();
			health -= (int)force;

			if(health <= 0)
			{
				Death();
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

	async void Death()
	{
		GetNode<GpuParticles3D>("GPUParticles3D").Emitting = true;
		GetNode<CollisionShape3D>("CollisionShape3D").QueueFree();
		GetNode<Sprite3D>("Sprite3D").QueueFree();
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		QueueFree();
	}
}