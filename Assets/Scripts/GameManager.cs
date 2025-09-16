using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Используйте, если у вас есть UI элементы
using TMPro; // Используйте, если у вас есть TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Character playerCharacter;
    private Monster currentMonster;
    private int turn;
    private bool isPlayerTurn;

    // Ссылки на UI (необходимо настроить в инспекторе)
    public GameObject classSelectionPanel;
    public GameObject combatPanel;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public GameObject weaponChoicePanel;

    public TextMeshProUGUI combatLogText;
    public TextMeshProUGUI playerStatsText;
    public TextMeshProUGUI monsterStatsText;
    public TextMeshProUGUI weaponChoiceText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ShowClassSelection();
    }

    public void SelectClass(int classIndex)
    {
        Classes.CharacterClass selectedClass = (Classes.CharacterClass)classIndex;
        playerCharacter = new Character(selectedClass);
        classSelectionPanel.SetActive(false);
        StartNextFight();
    }

    private void StartNextFight()
    {
        if (playerCharacter.monstersDefeated >= 5)
        {
            GameWon();
            return;
        }

        currentMonster = new Monster(playerCharacter.level);
        UpdateStatsUI();
        combatPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        weaponChoicePanel.SetActive(false);
        combatLogText.text = $"На вас напал {currentMonster.type}!\n";
        StartCoroutine(CombatLoop());
    }

    private void UpdateStatsUI()
    {
        playerStatsText.text = $"Игрок ({playerCharacter.characterClass})\n" +
                               $"Уровень: {playerCharacter.level}\n" +
                               $"HP: {playerCharacter.currentHp}/{playerCharacter.maxHp}\n" +
                               $"Сила: {playerCharacter.strength}\n" +
                               $"Ловкость: {playerCharacter.agility}\n" +
                               $"Выносливость: {playerCharacter.stamina}\n" +
                               $"Оружие: {playerCharacter.weapon.name} ({playerCharacter.weapon.minDamage}-{playerCharacter.weapon.maxDamage})";

        monsterStatsText.text = $"Монстр ({currentMonster.type})\n" +
                                $"HP: {currentMonster.currentHp}/{currentMonster.maxHp}\n" +
                                $"Сила: {currentMonster.strength}\n" +
                                $"Ловкость: {currentMonster.agility}\n" +
                                $"Выносливость: {currentMonster.stamina}";
    }

    private IEnumerator CombatLoop()
    {
        turn = 1;
        isPlayerTurn = true;

        while (playerCharacter.currentHp > 0 && currentMonster.currentHp > 0)
        {
            yield return new WaitForSeconds(1.5f); // Пауза между ходами

            if (isPlayerTurn)
            {
                PlayerAttack();
            }
            else
            {
                MonsterAttack();
            }

            UpdateStatsUI();
            isPlayerTurn = !isPlayerTurn;
            if (isPlayerTurn)
            {
                turn++;
            }
        }

        yield return new WaitForSeconds(1.5f);

        if (playerCharacter.currentHp > 0)
        {
            PlayerWon();
        }
        else
        {
            GameOver();
        }
    }

    private void PlayerAttack()
    {
        int damage = CalculatePlayerDamage();
        currentMonster.currentHp -= damage;
        combatLogText.text += $"Ход {turn}: Игрок атакует и наносит {damage} урона.\n";
    }

    private void MonsterAttack()
    {
        int damage = CalculateMonsterDamage();
        playerCharacter.currentHp -= damage;
        combatLogText.text += $"Ход {turn}: {currentMonster.type} атакует и наносит {damage} урона.\n";
    }

    private int CalculatePlayerDamage()
    {
        int baseDamage = playerCharacter.weapon.GetDamage() + playerCharacter.strength;
        int finalDamage = baseDamage;

        // Бонусы классов
        switch (playerCharacter.characterClass)
        {
            case Classes.CharacterClass.Rogue:
                if (playerCharacter.level >= 1 && playerCharacter.agility > currentMonster.agility)
                    finalDamage++; // Скрытая атака
                if (playerCharacter.level >= 3 && turn > 1)
                    finalDamage += (turn - 1); // Яд
                break;
            case Classes.CharacterClass.Warrior:
                if (playerCharacter.level >= 1 && turn == 1)
                    finalDamage *= 2; // Порыв к действию
                break;
            case Classes.CharacterClass.Barbarian:
                if (playerCharacter.level >= 1)
                {
                    if (turn <= 3) finalDamage += 2; // Ярость
                    else finalDamage--;
                }
                break;
        }
        return Mathf.Max(0, finalDamage);
    }

    private int CalculateMonsterDamage()
    {
        int baseDamage = currentMonster.weapon.GetDamage() + currentMonster.strength;
        int finalDamage = baseDamage;

        // Бонусы защиты игрока
        switch (playerCharacter.characterClass)
        {
            case Classes.CharacterClass.Warrior:
                if (playerCharacter.level >= 2 && playerCharacter.strength > currentMonster.strength)
                    finalDamage -= 3; // Щит
                break;
            case Classes.CharacterClass.Barbarian:
                if (playerCharacter.level >= 2)
                    finalDamage -= playerCharacter.stamina; // Каменная кожа
                break;
        }
        return Mathf.Max(0, finalDamage);
    }

    private void PlayerWon()
    {
        combatLogText.text += "Вы победили!\n";
        playerCharacter.monstersDefeated++;
        playerCharacter.LevelUp();
        
        combatPanel.SetActive(false);
        weaponChoicePanel.SetActive(true);
        weaponChoiceText.text = $"Вы нашли {currentMonster.weapon.name} ({currentMonster.weapon.minDamage}-{currentMonster.weapon.maxDamage}).\n" +
                                $"Ваше текущее оружие: {playerCharacter.weapon.name} ({playerCharacter.weapon.minDamage}-{playerCharacter.weapon.maxDamage}).\n" +
                                "Хотите заменить?";
    }

    public void ChooseWeapon(bool replace)
    {
        if (replace)
        {
            playerCharacter.weapon = currentMonster.weapon;
        }
        StartNextFight();
    }

    private void GameOver()
    {
        combatPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    private void GameWon()
    {
        combatPanel.SetActive(false);
        victoryPanel.SetActive(true);
    }

    public void RestartGame()
    {
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        ShowClassSelection();
    }

    private void ShowClassSelection()
    {
        classSelectionPanel.SetActive(true);
        combatPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        weaponChoicePanel.SetActive(false);
    }
}
