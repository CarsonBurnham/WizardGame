using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{

	public GameObject playerPrefab;
	public GameObject enemyPrefab;

	public Transform playerBattleStation;
	public Transform enemyBattleStation;

	Unit playerUnit;
	Unit enemyUnit;

	public GameObject dialoguePanel;
	public Text dialogueText;
	public GameObject spellPanel;

	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

	public UnityEngine.UI.Button attackButton;
	public UnityEngine.UI.Button healButton;

	public BattleState state;

	// Start is called before the first frame update
	void Start()
	{
		state = BattleState.START;
		StartCoroutine(SetupBattle());
	}

	IEnumerator SetupBattle()
	{
		GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
		playerUnit = playerGO.GetComponent<Unit>();

		GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
		enemyUnit = enemyGO.GetComponent<Unit>();

		dialogueText.text = enemyUnit.unitName + " draws near...";

		playerHUD.SetHUD(playerUnit);
		enemyHUD.SetHUD(enemyUnit);

		yield return new WaitForSeconds(2f);

		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}

	IEnumerator PlayerAttack()
	{
		bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

		enemyHUD.SetHP(enemyUnit.currentHP);
		dialogueText.text = "The attack is successful!";



		if (isDead)
		{
			state = BattleState.WON;
			EndBattle();
		}
		else
		{
			state = BattleState.ENEMYTURN;
			yield return new WaitForSeconds(2f);
			StartCoroutine(EnemyTurn());
		}
	}

	IEnumerator EnemyTurn()
	{
		dialogueText.text = enemyUnit.unitName + " attacks!";

		yield return new WaitForSeconds(1f);

		bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

		playerHUD.SetHP(playerUnit.currentHP);

		yield return new WaitForSeconds(1f);

		if (isDead)
		{
			state = BattleState.LOST;
			EndBattle();
		}
		else
		{
			state = BattleState.PLAYERTURN;
			PlayerTurn();
		}

	}

	void EndBattle()
	{
		if (state == BattleState.WON)
		{
			dialogueText.text = "You won the battle!";
		}
		else if (state == BattleState.LOST)
		{
			dialogueText.text = "You were defeated.";
		}
	}

	void PlayerTurn()
	{
		dialoguePanel.SetActive(true);
		spellPanel.SetActive(false);
		dialogueText.text = "Choose an action:";
	}

	IEnumerator PlayerHeal()
	{
		playerUnit.Heal(5);

		playerHUD.SetHP(playerUnit.currentHP);
		dialogueText.text = "Your strength returns.";

		state = BattleState.ENEMYTURN;

		yield return new WaitForSeconds(2f);


		StartCoroutine(EnemyTurn());
	}
	IEnumerator PlayerFireball()
	{
		if (enemyUnit.tag == "Oiled Up")
		{
			bool isDead = enemyUnit.TakeDamage((playerUnit.damage) * 3);
			enemyUnit.tag = "Slimy";
		}
		else
		{
			bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
		}
		enemyHUD.SetHP(enemyUnit.currentHP);
		dialogueText.text = "You cast Fireball!";
		if (enemyUnit.currentHP <= 0)
		{
			state = BattleState.WON;
			EndBattle();
		}
		else
		{
			state = BattleState.ENEMYTURN;
			yield return new WaitForSeconds(2f);
			StartCoroutine(EnemyTurn());
		}
	}
	IEnumerator PlayerOilSplash()
	{
		bool isDead = enemyUnit.TakeDamage((playerUnit.damage)/2);
		enemyUnit.tag = "Oiled Up";

		enemyHUD.SetHP(enemyUnit.currentHP);
		dialogueText.text = "The enemy is Oiled Up!";



		if (isDead)
		{
			state = BattleState.WON;
			EndBattle();
		}
		else
		{
			state = BattleState.ENEMYTURN;
			yield return new WaitForSeconds(2f);
			StartCoroutine(EnemyTurn());
		}
	}
	IEnumerator PlayerMagicMissile()
	{
		bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

		enemyHUD.SetHP(enemyUnit.currentHP);
		dialogueText.text = "The attack is successful!";



		if (isDead)
		{
			state = BattleState.WON;
			EndBattle();
		}
		else
		{
			state = BattleState.ENEMYTURN;
			yield return new WaitForSeconds(2f);
			StartCoroutine(EnemyTurn());
		}
	}


    public void Update()
    {
		playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
    }
    public void OnAttackButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;
		dialoguePanel.SetActive(true);
		spellPanel.SetActive(false);
		StartCoroutine(PlayerAttack());
		Debug.Log("cringe");
	}

	public void OnHealButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;

		dialoguePanel.SetActive(true);
		spellPanel.SetActive(false);
		StartCoroutine(PlayerHeal());
	}
	public void OnFireballButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;
		dialoguePanel.SetActive(true);
		spellPanel.SetActive(false);
		StartCoroutine(PlayerFireball());
		Debug.Log("kaboom");
	}
	public void OnOilSplashButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;
		dialoguePanel.SetActive(true);
		spellPanel.SetActive(false);
		StartCoroutine(PlayerOilSplash());
		Debug.Log("squelch");

	}
	public void OnMagicMissileButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;
		dialoguePanel.SetActive(true);
		spellPanel.SetActive(false);
		StartCoroutine(PlayerMagicMissile());
		Debug.Log("Shazam");
	}
}
