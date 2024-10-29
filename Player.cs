using Godot;
//using System;

public partial class Player : Area2D
{
  //Defaults and definitions
  [Export]
  public int Speed { get; set; } = 400;

  public Vector2 ScreenSize;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
		ScreenSize = GetViewportRect().Size;
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
		//each frame it's zero plus the pressed key velocity, normalized.
		var velocity = Vector2.Zero;

		//checking for movement
		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}
		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}
		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}
		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}

		//grab this guy's child animated sprite, make it swap animations
		var animateSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animateSprite2D.Play();
		}
		else
		{
			animateSprite2D.Stop();
		}

		//compute new position based on velocity..
		Position += velocity * (float)delta;
		//clamp to screen so we don't run away;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
				);
  }
}
