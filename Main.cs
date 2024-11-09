using Godot;

public partial class Main : Node
{
  [Export]
  public PackedScene MobScene { get; set; }

  private int _score;

  public void GameOver() {
    GetNode<Timer>("MobTimer").Stop();
    GetNode<Timer>("ScoreTimer").Stop();

    GetNode<Hud>("HUD").ShowGameOver();
  }

  public void NewGame() {

    //free the remaining mobs and zero score
    GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
    _score = 0;

    //update hud
    var hud = GetNode<Hud>("HUD");
    hud.UpdateScore(_score);
    hud.ShowMessageFading("Get Ready!");

    //set player
    var player = GetNode<Player>("Player");
    var startPosition = GetNode<Marker2D>("StartPosition");
    player.Start(startPosition.Position);

    //do timer for game start
    GetNode<Timer>("StartTimer").Start();
  }

  private void OnScoreTimerTimeout() {
    _score++;

    //show new score on hud
    GetNode<Hud>("HUD").UpdateScore(_score);
  }

  private void OnStartTimerTimeout() {
    GetNode<Timer>("MobTimer").Start();
    GetNode<Timer>("ScoreTimer").Start();
  }

  private void OnMobTimerTimeout() {
    Mob mob = MobScene.Instantiate<Mob>();

    //random location along spawn path follow (edge of screen)
    PathFollow2D mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
    mobSpawnLocation.ProgressRatio = GD.Randf();

    //set mob position
    mob.Position = mobSpawnLocation.Position;

    //choose direction to send mob, start perpendicular to path (inward?)
    float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;
    //vary from directly perpendicular by given range:
    direction+= (float) GD.RandRange(-Mathf.Pi/4, Mathf.Pi/4);

    //make a velocity on that direction vector, move mob that velocity;
    var velocity = new Vector2((float) GD.RandRange(150.0, 250.0), 0);
    mob.LinearVelocity = velocity.Rotated(direction);

    //put the mob into the main scene
    AddChild(mob);
  }
}
