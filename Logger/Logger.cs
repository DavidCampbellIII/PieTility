using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Logger : Singleton<Logger>
{
    [Header("==GENERAL==")]
    [SerializeField]
    private GameObject console;
    [SerializeField]
    private Text statusText;

    [Header("==CONFIGURATION==")]
    [SerializeField]
    [Tooltip("Can the player view the console? (NOTE: Probably want to disable this upon shipping of game)")]
    private bool canViewConsole = false;
    [DisplayInspector]
    [SerializeField]
    private LoggerConfigurationProfile configurationProfile;

    private LinkedList<string> messages;
    private Color[] messageTypeColors;

    protected override void Awake()
    {
        if(!InitializeSingleton(this))
        {
            return;
        }

        //always start with console turned off
        console.SetActive(false);
        //always start with empty console
        statusText.text = string.Empty;

        messages = new LinkedList<string>();

        int numMessageTypes = Enum.GetValues(typeof(LogMessageType)).Length;
        messageTypeColors = new Color[numMessageTypes];

        //fill array with default message color so that all unassigned message types
        //in the next loop will remain at this default color instead of being overridden
        for(int i = 0; i < messageTypeColors.Length; i++)
        {
            messageTypeColors[i] = configurationProfile.defaultMessageColor;
        }

        //organize colors for message types via index of enum value for fastest lookup
        LogMessageTypeProperties[] messageTypeProperties = configurationProfile.messageTypeProperties;
        foreach(LogMessageTypeProperties property in messageTypeProperties)
        {
            Color messageTypeColor = property.messageColor;
            int messageTypeColorIndex = (int)property.type;
            messageTypeColors[messageTypeColorIndex] = messageTypeColor;
        }
    }

    private void Update()
    {
        //toggle console with `
        if(canViewConsole && Input.GetKeyDown(KeyCode.BackQuote))
        {
            console.SetActive(!console.activeInHierarchy);
        }
    }

    /// <summary>
    /// Logs a message to the in game console.
    /// </summary>
    /// <param name="message">Message to log</param>
    /// <param name="type">Type of message</param>
    /// <param name="debugToConsole">Should the message also be logged to the Unity console using Debug.Log()?</param>
    public static void Log(string message, LogMessageType type = LogMessageType.DEFAULT, bool debugToConsole = false)
    {
        string colorCodedMessage = instance.ColorCodeMessage(message, type);
        if(debugToConsole)
        {
            instance.HandleDebugLog(message, type);
        }
        instance.UpdateConsoleDisplay(colorCodedMessage);
    }

    private string ColorCodeMessage(string message, LogMessageType type)
    {
        Color color = messageTypeColors[(int)type];
        string colorHex = Utilities.ColorToHexString(color);

        StringBuilder builder = new StringBuilder("<color=#");
        builder.Append(colorHex).Append(">").Append(message).Append("</color>");
        return builder.ToString();
    }

    private void HandleDebugLog(string message, LogMessageType type)
    {
        switch (type)
        {
            case LogMessageType.WARNING:
                Debug.LogWarning(message);
                break;
            case LogMessageType.ERROR:
                Debug.LogError(message);
                break;
            case LogMessageType.DEFAULT:
            default:
                Debug.Log(message);
                break;
        }
    }

    private void UpdateConsoleDisplay(string message)
    {
        messages.AddLast(message + "\n");
        if (messages.Count > configurationProfile.maxMessages)
        {
            messages.RemoveFirst();
        }

        StringBuilder builder = new StringBuilder();
        foreach (string s in messages)
        {
            builder.Append(s);
        }
        statusText.text = builder.ToString();
    }
}
