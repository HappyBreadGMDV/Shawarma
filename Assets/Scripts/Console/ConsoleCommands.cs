using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleCommands : MonoBehaviour
{
    public TMP_InputField InputCommand;
    private List<CommandInfo> commands = new List<CommandInfo>();

    private void Awake()
    {
        // Находим все MonoBehaviour в сцене (включая выключенные не обязательно, достаточно активных)
        var allBehaviours = FindObjectsOfType<MonoBehaviour>(true);
        foreach (var behaviour in allBehaviours)
        {
            var type = behaviour.GetType();
            // Получаем все методы, помеченные нашим атрибутом Command
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => Attribute.IsDefined(m, typeof(CommandAttribute)));

            foreach (var method in methods)
            {
                var attr = (CommandAttribute)Attribute.GetCustomAttribute(method, typeof(CommandAttribute));
                commands.Add(new CommandInfo(behaviour, method, attr.ParamTypes.ToList()));
            }
        }
    }

    public void Send()
    {
        if (InputCommand == null) return;
        string input = InputCommand.text.Trim();
        if (string.IsNullOrEmpty(input)) return;

        // Парсим имя команды и аргументы
        string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0) return;

        string commandName = parts[0];
        string[] args = parts.Skip(1).ToArray();

        // Ищем подходящую команду
        var candidates = commands.Where(c => c.Method.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));
        foreach (var cmd in candidates)
        {
            if (TryExecuteCommand(cmd, args))
            {
                Debug.Log($"Command '{commandName}' executed.");
                InputCommand.text = "";   // очистка поля после успешного выполнения
                return;
            }
        }

        Debug.LogWarning($"Command '{commandName}' not found or invalid parameters.");
        InputCommand.text = "";
    }

    private bool TryExecuteCommand(CommandInfo cmd, string[] args)
    {
        var parameters = cmd.Method.GetParameters();

        // Количество переданных аргументов не должно превышать количество параметров
        if (args.Length > parameters.Length)
            return false;

        // Собираем финальные аргументы для вызова
        object[] parsedArgs = new object[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            if (i < args.Length)
            {
                // Пытаемся преобразовать строку в нужный тип
                try
                {
                    parsedArgs[i] = Convert.ChangeType(args[i], parameters[i].ParameterType);
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                // Для оставшихся параметров проверяем, есть ли у них значение по умолчанию
                if (parameters[i].HasDefaultValue)
                {
                    parsedArgs[i] = parameters[i].DefaultValue;
                }
                else
                {
                    return false; // обязательный параметр не передан
                }
            }
        }

        cmd.Method.Invoke(cmd.Target, parsedArgs);
        return true;
    }
}