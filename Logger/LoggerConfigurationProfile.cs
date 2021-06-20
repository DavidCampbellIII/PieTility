using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoggerConfigurationProfile", menuName = "Logger/Logger Configuration Profile")]
public class LoggerConfigurationProfile : ScriptableObject
{
    [Header("==GENERAL==")]
    [SerializeField]
    [Tooltip("Max number of messages allowed to be displayed in the console at a given time")]
    private int _maxMessages = 5;
    public int maxMessages { get => _maxMessages; }

    [Header("==MESSAGE TYPES==")]
    [SerializeField]
    [Tooltip("Default message color for all message types where a specific color is not given")]
    private Color _defaultMessageColor = Color.white;
    public Color defaultMessageColor { get => _defaultMessageColor; }

    [SerializeField]
    [Tooltip("Properties of all message types")]
    private LogMessageTypeProperties[] _messageTypeProperties;
    public LogMessageTypeProperties[] messageTypeProperties { get => _messageTypeProperties; }
}

[System.Serializable]
public class LogMessageTypeProperties
{
    [SerializeField]
    [Tooltip("Type of message")]
    private LogMessageType _type;
    public LogMessageType type { get => _type; }

    [SerializeField]
    [Tooltip("Text color of all messages with this type")]
    private Color _messageColor = Color.white;
    public Color messageColor { get => _messageColor; }
}
