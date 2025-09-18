using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Character playerCharacter;
    private Monster currentMonster;
    private int turn;
    private bool isPlayerTurn;

    private enum ClassChoiceMode { Initial, LevelUp };
    private ClassChoiceMode currentClassChoiceMode;

    
    public GameObject classChoicePanel; 
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
        ShowInitialClassChoice();
    }

    private void StartNextFight()
    {
        if (playerCharacter.monstersDefeated >= 5)
        {
            GameWon();
            return;
        }

        currentMonster = new Monster(playerCharacter.TotalLevel);
        UpdateStatsUI();
        combatPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        weaponChoicePanel.SetActive(false);
        classChoicePanel.SetActive(false);
        combatLogText.text = $"На вас напал {currentMonster.type}!\n";
        StartCoroutine(CombatLoop());
    }

    private void UpdateStatsUI()
    {
        playerStatsText.text = $"Игрок ({playerCharacter.GetClassLevelSummary()})\n" +
                               $"Общий уровень: {playerCharacter.TotalLevel}\n" +
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

        // Применяем бонусы от всех классов
        foreach (var classLevelPair in playerCharacter.classLevels)
        {
            var charClass = classLevelPair.Key;
            var level = classLevelPair.Value;

            switch (charClass)
            {
                case Classes.CharacterClass.Rogue:
                    if (level >= 1 && playerCharacter.agility > currentMonster.agility)
                        finalDamage++; // Скрытая атака
                    if (level >= 3 && turn > 1)
                        finalDamage += (turn - 1); // Яд
                    break;
                case Classes.CharacterClass.Warrior:
                    if (level >= 1 && turn == 1)
                        finalDamage *= 2; // Порыв к действию
                    break;
                case Classes.CharacterClass.Barbarian:
                    if (level >= 1)
                    {
                        if (turn <= 3) finalDamage += 2; // Ярость
                        else finalDamage--;
                    }
                    break;
            }
        }
        return Mathf.Max(0, finalDamage);
    }

    private int CalculateMonsterDamage()
    {
        int baseDamage = currentMonster.weapon.GetDamage() + currentMonster.strength;
        int finalDamage = baseDamage;

        // Применяем бонусы защиты от всех классов
        foreach (var classLevelPair in playerCharacter.classLevels)
        {
            var charClass = classLevelPair.Key;
            var level = classLevelPair.Value;

            switch (charClass)
            {
                case Classes.CharacterClass.Warrior:
                    if (level >= 2 && playerCharacter.strength > currentMonster.strength)
                        finalDamage -= 3; // Щит
                    break;
                case Classes.CharacterClass.Barbarian:
                    if (level >= 2)
                        finalDamage -= playerCharacter.stamina; // Каменная кожа
                    break;
            }
        }
        return Mathf.Max(0, finalDamage);
    }

    private void PlayerWon()
    {
        combatLogText.text += "Вы победили!\n";
        playerCharacter.monstersDefeated++;

        if (playerCharacter.TotalLevel < 3)
        {
            ShowLevelUpChoice();
        }
        else
        {
            playerCharacter.RecalculateStats(); // Восстанавливаем HP на макс. уровне
            ShowWeaponChoice();
        }
    }

    private void ShowLevelUpChoice()
    {
        currentClassChoiceMode = ClassChoiceMode.LevelUp;
        classChoicePanel.SetActive(true);
    }

    private void ShowWeaponChoice()
    {
        weaponChoicePanel.SetActive(true);
        
        Weapon newWeapon = currentMonster.weapon;
        weaponChoiceText.text = $"Вы нашли: <b>{newWeapon.name}</b> (Урон: {newWeapon.minDamage}-{newWeapon.maxDamage})\n" +
                                $"Ваше текущее оружие: {playerCharacter.weapon.name} (Урон: {playerCharacter.weapon.minDamage}-{playerCharacter.weapon.maxDamage})\n\n" +
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
        
        gameOverPanel.SetActive(true);
    }

    private void GameWon()
    {
        
        victoryPanel.SetActive(true);
    }

    public void RestartGame()
    {
        // Уничтожаем GameManager, чтобы при перезагрузке сцены создался новый
        Destroy(gameObject);
        // Перезагружаем текущую сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ShowInitialClassChoice()
    {
        currentClassChoiceMode = ClassChoiceMode.Initial;
        classChoicePanel.SetActive(true);

        combatPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        weaponChoicePanel.SetActive(false);
    }

    public void OnClassChoice(int classIndex)
    {
        classChoicePanel.SetActive(false);
        Classes.CharacterClass selectedClass = (Classes.CharacterClass)classIndex;

        if (currentClassChoiceMode == ClassChoiceMode.Initial)
        {
            playerCharacter = new Character(selectedClass);
            StartNextFight();
        }
        else // LevelUp
        {
            playerCharacter.ApplyLevelUp(selectedClass);
            ShowWeaponChoice();
        }
    }
}
