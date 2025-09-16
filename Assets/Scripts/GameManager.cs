using System.Collections;
using UnityEngine;
using UnityEngine.UI; // �����������, ���� � ��� ���� UI ��������
using TMPro; // �����������, ���� � ��� ���� TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Character playerCharacter;
    private Monster currentMonster;
    private int turn;
    private bool isPlayerTurn;

    // ������ �� UI (���������� ��������� � ����������)
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
        combatLogText.text = $"�� ��� ����� {currentMonster.type}!\n";
        StartCoroutine(CombatLoop());
    }

    private void UpdateStatsUI()
    {
        playerStatsText.text = $"����� ({playerCharacter.characterClass})\n" +
                               $"�������: {playerCharacter.level}\n" +
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

        // ������ �������
        switch (playerCharacter.characterClass)
        {
            case Classes.CharacterClass.Rogue:
                if (playerCharacter.level >= 1 && playerCharacter.agility > currentMonster.agility)
                    finalDamage++; // ������� �����
                if (playerCharacter.level >= 3 && turn > 1)
                    finalDamage += (turn - 1); // ��
                break;
            case Classes.CharacterClass.Warrior:
                if (playerCharacter.level >= 1 && turn == 1)
                    finalDamage *= 2; // ����� � ��������
                break;
            case Classes.CharacterClass.Barbarian:
                if (playerCharacter.level >= 1)
                {
                    if (turn <= 3) finalDamage += 2; // ������
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

        // ������ ������ ������
        switch (playerCharacter.characterClass)
        {
            case Classes.CharacterClass.Warrior:
                if (playerCharacter.level >= 2 && playerCharacter.strength > currentMonster.strength)
                    finalDamage -= 3; // ���
                break;
            case Classes.CharacterClass.Barbarian:
                if (playerCharacter.level >= 2)
                    finalDamage -= playerCharacter.stamina; // �������� ����
                break;
        }
        return Mathf.Max(0, finalDamage);
    }

    private void PlayerWon()
    {
        combatLogText.text += "�� ��������!\n";
        playerCharacter.monstersDefeated++;
        playerCharacter.LevelUp();
        
        combatPanel.SetActive(false);
        weaponChoicePanel.SetActive(true);
        weaponChoiceText.text = $"�� ����� {currentMonster.weapon.name} ({currentMonster.weapon.minDamage}-{currentMonster.weapon.maxDamage}).\n" +
                                $"���� ������� ������: {playerCharacter.weapon.name} ({playerCharacter.weapon.minDamage}-{playerCharacter.weapon.maxDamage}).\n" +
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
