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
        // Определение очередности хода
        isPlayerTurn = playerCharacter.agility >= currentMonster.agility;
        combatLogText.text += isPlayerTurn ? "Вы ходите первым!\n" : $"{currentMonster.type} ходит первым!\n";

        while (playerCharacter.currentHp > 0 && currentMonster.currentHp > 0)
        {
            yield return new WaitForSeconds(1.5f); // Пауза между ходами

            if (isPlayerTurn)
            {
                PerformPlayerAttack();
            }
            else
            {
                PerformMonsterAttack();
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

    private void PerformPlayerAttack()
    {
        // 1. Шанс попадания
        int hitRoll = Random.Range(1, playerCharacter.agility + currentMonster.agility + 1);
        if (hitRoll <= currentMonster.agility)
        {
            combatLogText.text += $"Ход {turn}: Игрок атакует, но промахивается!\n";
            return;
        }

        // 2. Изначальный урон
        int weaponDamage = playerCharacter.weapon.GetDamage();
        int damage = playerCharacter.strength;
        string vulnerabilityMessage = "";

        if (currentMonster.type == Classes.MobType.Slime && 
            (playerCharacter.weapon.type == Classes.WeaponType.Sword || playerCharacter.weapon.type == Classes.WeaponType.Axe))
        {
            weaponDamage = 0;
            vulnerabilityMessage = " Слизь поглощает рубящий удар!";
        }
        
        damage += weaponDamage;

        // 3. Эффекты атакующего (игрока)
        if (playerCharacter.classLevels.TryGetValue(Classes.CharacterClass.Warrior, out int warriorLvl) && warriorLvl >= 1 && turn == 1)
        {
            damage += weaponDamage;
        }
        if (playerCharacter.classLevels.TryGetValue(Classes.CharacterClass.Barbarian, out int barbarianLvl) && barbarianLvl >= 1)
        {
            if (turn <= 3) damage += 2;
            else damage--;
        }
        if (playerCharacter.classLevels.TryGetValue(Classes.CharacterClass.Rogue, out int rogueLvl))
        {
            if (rogueLvl >= 1 && playerCharacter.agility > currentMonster.agility) damage++;
            if (rogueLvl >= 3 && turn > 1) damage += (turn - 1);
        }
        
        // 4. Эффекты цели (монстра)
        if (currentMonster.type == Classes.MobType.Golem)
        {
            damage -= currentMonster.stamina; // Каменная кожа Голема
            vulnerabilityMessage += " Каменная кожа Голема поглощает часть урона.";
        }
        if (currentMonster.type == Classes.MobType.Skeleton && playerCharacter.weapon.type == Classes.WeaponType.Club)
        {
            damage *= 2;
            vulnerabilityMessage = " Скелет уязвим к дробящему оружию! Урон удвоен.";
        }

        // 5. Вычитание урона
        damage = Mathf.Max(0, damage);
        currentMonster.currentHp -= damage;
        combatLogText.text += $"Ход {turn}: Игрок попадает и наносит {damage} урона.{vulnerabilityMessage}\n";
    }

    private void PerformMonsterAttack()
    {
        // 1. Шанс попадания
        int hitRoll = Random.Range(1, currentMonster.agility + playerCharacter.agility + 1);
        if (hitRoll <= playerCharacter.agility)
        {
            combatLogText.text += $"Ход {turn}: {currentMonster.type} атакует, но промахивается!\n";
            return;
        }

        // 2. Изначальный урон
        int damage = currentMonster.damage;
        string specialAttackMessage = "";

        // 3. Эффекты атакующего (монстра)
        if (currentMonster.type == Classes.MobType.Ghost && currentMonster.agility > playerCharacter.agility)
        {
            damage++; // Скрытая атака Призрака
        }
        if (currentMonster.type == Classes.MobType.Dragon && turn % 3 == 0)
        {
            damage += 3; // Огненное дыхание Дракона
            specialAttackMessage = " Дракон дышит огнем!";
        }

        // 4. Эффекты цели (игрока)
        if (playerCharacter.classLevels.TryGetValue(Classes.CharacterClass.Barbarian, out int barbarianLvl) && barbarianLvl >= 2)
        {
            damage -= playerCharacter.stamina;
        }
        if (playerCharacter.classLevels.TryGetValue(Classes.CharacterClass.Warrior, out int warriorLvl) && warriorLvl >= 2 && playerCharacter.strength > currentMonster.strength)
        {
            damage -= 3;
        }

        // 5. Вычитание урона
        damage = Mathf.Max(0, damage);
        playerCharacter.currentHp -= damage;
        combatLogText.text += $"Ход {turn}: {currentMonster.type} попадает и наносит {damage} урона.{specialAttackMessage}\n";
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
            playerCharacter.RecalculateStats();
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
        
        Weapon newWeapon = currentMonster.droppedWeapon;
        weaponChoiceText.text = $"Вы нашли: <b>{newWeapon.name}</b> (Урон: {newWeapon.minDamage}-{newWeapon.maxDamage})\n" +
                                $"Ваше текущее оружие: {playerCharacter.weapon.name} (Урон: {playerCharacter.weapon.minDamage}-{playerCharacter.weapon.maxDamage})\n\n" +
                                "Хотите заменить?";
    }

    public void ChooseWeapon(bool replace)
    {
        if (replace)
        {
            playerCharacter.weapon = currentMonster.droppedWeapon;
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
        Destroy(gameObject);
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
