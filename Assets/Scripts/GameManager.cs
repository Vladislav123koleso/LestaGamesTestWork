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
        combatLogText.text = $"�� ��� ����� {currentMonster.type}!\n";
        StartCoroutine(CombatLoop());
    }

    private void UpdateStatsUI()
    {
        playerStatsText.text = $"����� ({playerCharacter.GetClassLevelSummary()})\n" +
                               $"����� �������: {playerCharacter.TotalLevel}\n" +
                               $"HP: {playerCharacter.currentHp}/{playerCharacter.maxHp}\n" +
                               $"����: {playerCharacter.strength}\n" +
                               $"��������: {playerCharacter.agility}\n" +
                               $"������������: {playerCharacter.stamina}\n" +
                               $"������: {playerCharacter.weapon.name} ({playerCharacter.weapon.minDamage}-{playerCharacter.weapon.maxDamage})";

        monsterStatsText.text = $"������ ({currentMonster.type})\n" +
                                $"HP: {currentMonster.currentHp}/{currentMonster.maxHp}\n" +
                                $"����: {currentMonster.strength}\n" +
                                $"��������: {currentMonster.agility}\n" +
                                $"������������: {currentMonster.stamina}";
    }

    private IEnumerator CombatLoop()
    {
        turn = 1;
        isPlayerTurn = true;

        while (playerCharacter.currentHp > 0 && currentMonster.currentHp > 0)
        {
            yield return new WaitForSeconds(1.5f); // ����� ����� ������

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
        combatLogText.text += $"��� {turn}: ����� ������� � ������� {damage} �����.\n";
    }

    private void MonsterAttack()
    {
        int damage = CalculateMonsterDamage();
        playerCharacter.currentHp -= damage;
        combatLogText.text += $"��� {turn}: {currentMonster.type} ������� � ������� {damage} �����.\n";
    }

    private int CalculatePlayerDamage()
    {
        int baseDamage = playerCharacter.weapon.GetDamage() + playerCharacter.strength;
        int finalDamage = baseDamage;

        // ��������� ������ �� ���� �������
        foreach (var classLevelPair in playerCharacter.classLevels)
        {
            var charClass = classLevelPair.Key;
            var level = classLevelPair.Value;

            switch (charClass)
            {
                case Classes.CharacterClass.Rogue:
                    if (level >= 1 && playerCharacter.agility > currentMonster.agility)
                        finalDamage++; // ������� �����
                    if (level >= 3 && turn > 1)
                        finalDamage += (turn - 1); // ��
                    break;
                case Classes.CharacterClass.Warrior:
                    if (level >= 1 && turn == 1)
                        finalDamage *= 2; // ����� � ��������
                    break;
                case Classes.CharacterClass.Barbarian:
                    if (level >= 1)
                    {
                        if (turn <= 3) finalDamage += 2; // ������
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

        // ��������� ������ ������ �� ���� �������
        foreach (var classLevelPair in playerCharacter.classLevels)
        {
            var charClass = classLevelPair.Key;
            var level = classLevelPair.Value;

            switch (charClass)
            {
                case Classes.CharacterClass.Warrior:
                    if (level >= 2 && playerCharacter.strength > currentMonster.strength)
                        finalDamage -= 3; // ���
                    break;
                case Classes.CharacterClass.Barbarian:
                    if (level >= 2)
                        finalDamage -= playerCharacter.stamina; // �������� ����
                    break;
            }
        }
        return Mathf.Max(0, finalDamage);
    }

    private void PlayerWon()
    {
        combatLogText.text += "�� ��������!\n";
        playerCharacter.monstersDefeated++;

        if (playerCharacter.TotalLevel < 3)
        {
            ShowLevelUpChoice();
        }
        else
        {
            playerCharacter.RecalculateStats(); // ��������������� HP �� ����. ������
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
        weaponChoiceText.text = $"�� �����: <b>{newWeapon.name}</b> (����: {newWeapon.minDamage}-{newWeapon.maxDamage})\n" +
                                $"���� ������� ������: {playerCharacter.weapon.name} (����: {playerCharacter.weapon.minDamage}-{playerCharacter.weapon.maxDamage})\n\n" +
                                "������ ��������?";
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
        // ���������� GameManager, ����� ��� ������������ ����� �������� �����
        Destroy(gameObject);
        // ������������� ������� �����
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
