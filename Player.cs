using Godot;
//using System;

public partial class Player : Area2D {

  //Defaults and definitions (Export is a Godot macro)
  [Export]
  public int Speed { get; set; } = 400;

  //define handler for hit (Signal is a Godot macro)
  [Signal]
  public delegate void HitEventHandler();
  
  public Vector2 ScreenSize;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready() {
    //hide on game start
    Hide();
    ScreenSize = GetViewportRect().Size;
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta) {
    //each frame it's zero plus the pressed key velocity, normalized.
    var velocity = Vector2.Zero;

    //checking for movement
    if (Input.IsActionPressed("move_right")) {
      velocity.X += 1;
    }
    if (Input.IsActionPressed("move_left")) {
      velocity.X -= 1;
    }
    if (Input.IsActionPressed("move_down")) {
      velocity.Y += 1;
    }
    if (Input.IsActionPressed("move_up")) {
      velocity.Y -= 1;
    }

    //grab this guy's child animated sprite, make it swap animations
    var animateSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

    if (velocity.Length() > 0) {
      velocity = velocity.Normalized() * Speed;
      animateSprite2D.Play();
    } else {
      animateSprite2D.Stop();
    }

    //velocity effect animation:
    if (velocity.X != 0) {
      animateSprite2D.Animation = "walk";
      animateSprite2D.FlipV = false;
      //flip to look left if moving left
      animateSprite2D.FlipH = velocity.X < 0;
    } else if (velocity.Y != 0) {
      animateSprite2D.Animation = "up";
      //flip to look down if moving down
      animateSprite2D.FlipH = velocity.Y > 0;
    }

    //compute new position based on velocity..
    Position += velocity * (float)delta;
    
    //clamp to screen so we don't run away;
    Position = new Vector2(
        x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
        y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
    );
  }

  //set player to a position, ready them for collision and rendering
  public void Start(Vector2 position) {
    Position = position;
    Show();
    GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
  }

  //body entered signal handler? I hope it works..
  private void OnBodyEntered(Node2D body) {
      Hide(); // Player disappears after being hit.
      EmitSignal(SignalName.Hit);

      // Must be deferred, can't change physics properties on a physics callback.
      GetNode<CollisionShape2D>("CollisionShape2D")
        .SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
  }

}
