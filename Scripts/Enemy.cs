using Godot;
using System;

public abstract partial class Enemy : CharacterBody3D
{
	protected int health;
	protected Timer gotHit, fireHit;
	protected bool onFire = false;
	protected float flamability;
	protected bool isGettingBlocked = false;

	public override void _PhysicsProcess(double delta)
	{
		if (onFire && fireHit.IsStopped() && health > 0)
		{
			health -= 10;
			if (health <= 0)
				Death();
			else
			{
				fireHit.Start();
				GetNode<AudioStreamPlayer3D>("FlamePlayer3D").Play();
				Random rnd = new();
				Tween tween = GetTree().CreateTween();
				tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "offset", new Vector2(rnd.NextSingle() - 0.5f, -rnd.NextSingle()), 0.02f);
				tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "offset", new Vector2(-rnd.NextSingle() + 0.5f, rnd.NextSingle()), 0.02f);
				tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "offset", Vector2.Zero, 0.02f);
			}
		}
	}
	public void GetHit(float force, String type)
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
				GetNode<AudioStreamPlayer3D>("HitPlayer3D").Play();
				
				Random rnd = new();
				Tween tween = GetTree().CreateTween();
				tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "offset", new Vector2(rnd.NextSingle()-0.5f,-rnd.NextSingle()), 0.02f);
				tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "offset", new Vector2(-rnd.NextSingle()+0.5f,rnd.NextSingle()), 0.02f);
				tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "offset", new Vector2(rnd.NextSingle()-0.5f,-rnd.NextSingle()), 0.02f);
				tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "offset", new Vector2(-rnd.NextSingle()+0.5f,-rnd.NextSingle()), 0.02f);
				tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "offset", Vector2.Zero, 0.02f);
				if (type == "fire")
				{
					SetOnFire();
				}
			}
		}
	}
	public void GetBlocked()
	{
		isGettingBlocked = true;
	}

	async protected void Death()
	{
		GetNode<AudioStreamPlayer3D>("DeathPlayer3D").Play();
		GetNode<GpuParticles3D>("OnFire").Emitting = false;
		GetNode<GpuParticles3D>("GPUParticles3D").Emitting = true;
		GetNode<OmniLight3D>("OmniLight3D").Visible = false;
		GetNode<CollisionShape3D>("CollisionShape3D").QueueFree();
		GetNode<Sprite3D>("Sprite3D").QueueFree();
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		QueueFree();
	}
	async protected void SetOnFire()
	{
		onFire = true;
		GetNode<GpuParticles3D>("OnFire").Emitting = true;
		GetNode<OmniLight3D>("OmniLight3D").Visible = true;
		await ToSignal(GetTree().CreateTimer(flamability), "timeout");
		GetNode<OmniLight3D>("OmniLight3D").Visible = false;
		onFire = false;
		GetNode<GpuParticles3D>("OnFire").Emitting = false;
	}
	
}