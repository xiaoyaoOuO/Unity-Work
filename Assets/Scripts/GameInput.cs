using UnityEngine;

public class Command{
    private KeyCode key;
    private bool isPressed{get{ return Input.GetKeyDown(key); }}
    public Command(KeyCode key){ this.key = key; }
    public virtual void Execute(){}
    public virtual bool IsPressed(){ return isPressed; }
    public virtual void Setkey(KeyCode key){ this.key = key; }
}

public static class GameInput{
    public static Command Jump = new Command(KeyCode.Space);
    public static Command Attack = new Command(KeyCode.Mouse0);
    public static Command Dash = new Command(KeyCode.LeftShift);
    public static bool IsJumpPressed(){ return Jump.IsPressed(); }
    public static bool IsAttackPressed(){ return Attack.IsPressed(); }
    public static bool IsDashPressed(){ return Dash.IsPressed(); }   
}