using UnityEngine;

public class Command{
    private KeyCode key;
    private bool isPressed{get{ return Input.GetKeyDown(key); }}
    public float consumeBuffer;
    public float bufferTime;
    public Command(KeyCode key,float bufferTime = 0f){
        this.bufferTime = bufferTime;
        this.key = key;
    }
    public virtual void Execute(){}

    public virtual bool IsPressed(){ return isPressed || consumeBuffer > 0; }

    public virtual bool IsDown(){ return Input.GetKey(key); }

    public virtual void Setkey(KeyCode key){ this.key = key; }

    public void Update(){
        consumeBuffer -= Time.deltaTime;
        bool flag = false;
        if(isPressed){
            consumeBuffer = bufferTime;
            flag = true;
        }else if(IsDown()){
            flag = true;
        }

        if(!flag){
           consumeBuffer = 0; 
        }
    }
}


public static class GameInput{
    public static Command Jump = new Command(KeyCode.Space,0.4f);
    public static Command Attack = new Command(KeyCode.Mouse0);
    public static Command Dash = new Command(KeyCode.LeftShift,0.1f);
    public static Command BulletTime = new Command(KeyCode.Mouse1);
    public static Command Roll = new Command(KeyCode.S,0.4f);
    public static Vector2 LastDir {get => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));}
    public static bool IsJumpPressed(){ return Jump.IsPressed(); }
    public static bool IsAttackPressed(){ return Attack.IsPressed(); }
    public static bool IsDashPressed(){ return Dash.IsPressed(); }  
    public static bool IsBulletTimePressed(){ return BulletTime.IsDown(); } 
    public static bool IsRollPressed(){ return Roll.IsPressed(); }
}