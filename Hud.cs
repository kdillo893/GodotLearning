using Godot;

public partial class Hud : CanvasLayer
{

  [Signal]
  public delegate void StartGameEventHandler();


  public void ShowMessageFading(string text) {
    ShowMessage(text);
    
    GetNode<Timer>("MessageTimer").Start();
  }

  private void ShowMessage(string text) {
    var message = GetNode<Label>("Message");
    message.Text = text;
    message.Show();
  }

  private void HideMessage() {
    var message = GetNode<Label>("Message");
    message.Hide();
  }

  async public void ShowGameOver() {
    ShowMessageFading("Game Over");

    //showing game over, wait 'til message fades before showing "game start" state
    var messageTimer = GetNode<Timer>("MessageTimer");
    await ToSignal(messageTimer, Timer.SignalName.Timeout);

    ShowMessage("Dodge the Creeps!");

    await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
    GetNode<Button>("StartButton").Show();
  }

  public void UpdateScore(int score) {
    GetNode<Label>("ScoreLabel").Text = score.ToString();
  }

  private void OnStartButtonPressed() {
    GetNode<Button>("StartButton").Hide();
    EmitSignal(SignalName.StartGame);
  }

  private void OnMessageTimerTimeout() {
    GetNode<Label>("Message").Hide();
  }

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
  }
}
