using System.Drawing;
using Godot;

namespace UIProject.Scripts;

public partial class Player : Creature
{

	public int playerPoints = 0;

	[Export]
	public int Lives = 3;
	
	private Vector2 _startPosition;
	
	private int Score = GameManager.playerScore;

	private bool IsAttacking => _sprite.Animation.ToString() == "attack" && _sprite.IsPlaying();

	private AnimatedSprite2D _sprite;
	private Area2D _hurtBox;

	public int Points = 0;


	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
		_startPosition = GlobalPosition;
		
		_sprite = GetNode<AnimatedSprite2D>("Sprite");
		_hurtBox = GetNode<Area2D>("HurtBox");



	}

	public override void _PhysicsProcess(double delta)
	{
		var direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
		UpdateVelocity(direction);
		
		UpdateDirection(direction);
		
		var attacking = Input.IsActionJustPressed("ui_accept");
		//don't let the use spam attacks
		if (attacking && !IsAttacking)
			ActivateAttack();
		UpdateSpriteAnimation(direction, attacking);
		
		MoveAndSlide();
	}

	public void TakeDamage(int damage)
	{
		CurrentHealth -= damage;
		GetNode<AnimationPlayer>("damageplayer1").Play("takeDamage");
		
		if (CurrentHealth < 1)
		{
			GetNode<AnimationPlayer>("AnimationPlayer").Play("frog3");
		}
		else if (CurrentHealth < 2)
		{
			GetNode<AnimationPlayer>("AnimationPlayer").Play("frog2");
		}
		else if (CurrentHealth < 3)
		{
			GetNode<AnimationPlayer>("AnimationPlayer").Play("frog");
		}


		if (CurrentHealth <= 0)
		{
			Lives -= 1;
			if (Lives == 2)
			{
				GD.Print("ben is beri cool");
				GetNode<AnimationPlayer>("AnimationPlayer2").Play("live1");
			}
			else if (Lives == 1)
			{
				GetNode<AnimationPlayer>("AnimationPlayer2").Play("live2");
			}
			else if (Lives == 0)
			{
				GetNode<AnimationPlayer>("AnimationPlayer2").Play("live3");
			}
			if (Lives <= 0) {
				GD.Print("Game Over");
				GetTree().Quit();
			}
			else
			{
				GetNode<AnimationPlayer>("AnimationPlayer").Play("frogfix");
				GD.Print($"Player Lives: {Lives}");
				GlobalPosition = _startPosition;
				CurrentHealth = MaxHealth;
			}
		}
		
		GD.Print($"Player Health: {CurrentHealth}");
		EmitSignal(Creature.SignalName.HealthChanged, CurrentHealth, MaxHealth);
	}
	
	private void UpdateVelocity(Vector2 direction)
	{
		Vector2 velocity = Velocity;
		if (direction != Vector2.Zero && !IsAttacking)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
			
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}
		Velocity = velocity;
	}
	
	private void UpdateDirection(Vector2 direction)
	{
		if (direction.X < 0)
		{
			_sprite.FlipH = true;
			if (_hurtBox.Position.X > 0)
				_hurtBox.Position = new Vector2(_hurtBox.Position.X * -1, _hurtBox.Position.Y);
		}
		else if (direction.X > 0)
		{
			_sprite.FlipH = false;
			if (_hurtBox.Position.X < 0)
				_hurtBox.Position = new Vector2(_hurtBox.Position.X * -1, _hurtBox.Position.Y);
		}
	}

	private void UpdateSpriteAnimation(Vector2 direction, bool attacking)
	{
		//don't interrupt the attack animation
		if (!IsAttacking)
		{
			if (direction != Vector2.Zero)
				_sprite.Play("walk");
			else
				_sprite.Play("idle");
			
			//attack needs to be checked first to get priority
			if (attacking)
			{
				_sprite.Play("attack");
				//stop moving if you're attacking
				Vector2 velocity = Vector2.Zero;
			}
		}
	}

	private void ActivateAttack()
	{
		var bodies = _hurtBox.GetOverlappingBodies();
		foreach (var body in bodies)
		{
			if (body is Enemy enemy)
			{
				//for this demo, just assume each attack does 1 damage
				enemy.TakeDamage(1);
				Score = GameManager.playerScore;
				UpdateScore();
			}
		}
	}

	private void UpdateScore()
	{
		GetNode<RichTextLabel>("RichTextLabel").Text = $"Score: {Score}";
	}
}