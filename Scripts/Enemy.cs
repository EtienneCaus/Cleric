using Godot;
using System;

public abstract partial class Enemy : CharacterBody3D
{
	protected int health;
	protected Timer gotHit, fireHit;
	protected bool onFire;

	public override void _PhysicsProcess(double delta)
	{
		if(onFire && fireHit.IsStopped() && health > 0)
		{
			health -= 10;
			if(health <= 0)
				Death();
			else
			{
				fireHit.Start();

				Random rnd = new();
				Tween tween = GetTree().CreateTween();
				tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "offset", new Vector2(rnd.NextSingle()-0.5f,-rnd.NextSingle()), 0.02f);
				tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "offset", new Vector2(-rnd.NextSingle()+0.5f,rnd.NextSingle()), 0.02f);
				tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "offset", Vector2.Zero, 0.02f);
			}
		}
	}
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
				SetOnFire();
			}
		}
	}

	async protected void Death()
	{
		GetNode<GpuParticles3D>("OnFire").Emitting = false;
		GetNode<GpuParticles3D>("GPUParticles3D").Emitting = true;
		GetNode<CollisionShape3D>("CollisionShape3D").QueueFree();
		GetNode<Sprite3D>("Sprite3D").QueueFree();
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		QueueFree();
	}
	async protected void SetOnFire()
	{
		onFire = true;
		GetNode<GpuParticles3D>("OnFire").Emitting = true;
		await ToSignal(GetTree().CreateTimer(6), "timeout");
		onFire = false;
		GetNode<GpuParticles3D>("OnFire").Emitting = false;
	}
}