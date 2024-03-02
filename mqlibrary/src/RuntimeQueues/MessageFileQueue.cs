using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FileMqBroker.MqLibrary.DAL;
using FileMqBroker.MqLibrary.Models;

namespace FileMqBroker.MqLibrary.RuntimeQueues;

/// <summary>
/// A class that allows you to manage a message queue within an application instance.
/// </summary>
public class MessageFileQueue
{
    #region Private fields
    private readonly string m_directoryName;
    private readonly FieldInfo[] m_privateFields;
    private ConcurrentDictionary<string, MessageFileState> m_messageFiles;
    private IReadOnlyDictionary<string, MessageFileState>? m_cachedMessageFiles;
    private IReadOnlyList<string>? m_cachedMessageFilesReadyToRead;
    private IReadOnlyList<string>? m_cachedMessageFilesReadyToWrite;
    private IReadOnlyList<string>? m_cachedMessageFilesFailed;
    #endregion  // Private fields
    
    #region Constructors
    /// <summary>
    /// Default constructor.
    /// </summary>
    public MessageFileQueue(string directoryName)
    {
        m_directoryName = directoryName;
        m_privateFields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        m_messageFiles = new ConcurrentDictionary<string, MessageFileState>();
    }
    #endregion  // Constructors

    #region Public properties
    /// <summary>
    /// A cached, immutable copy of the message queue.
    /// </summary>
    public IReadOnlyDictionary<string, MessageFileState> MessageFiles
    {
        get
        {
            if (m_cachedMessageFiles == null)
            {
                m_cachedMessageFiles = new Dictionary<string, MessageFileState>(m_messageFiles);
            }
            return m_cachedMessageFiles;
        }
    }

    /// <summary>
    /// List of queued files that are ready to be read.
    /// </summary>
    public IReadOnlyList<string> MessageFilesReadyToRead
    {
        get
        {
            if (m_cachedMessageFilesReadyToRead == null)
            {
                m_cachedMessageFilesReadyToRead = GetMessageFileList(x => x.Value == MessageFileState.ReadyToRead);
            }
            return m_cachedMessageFilesReadyToRead;
        }
    }

    /// <summary>
    /// List of queued files that are ready to be written.
    /// </summary>
    public IReadOnlyList<string> MessageFilesReadyToWrite
    {
        get
        {
            if (m_cachedMessageFilesReadyToWrite == null)
            {
                m_cachedMessageFilesReadyToWrite = GetMessageFileList(x => x.Value == MessageFileState.ReadyToWrite);
            }
            return m_cachedMessageFilesReadyToWrite;
        }
    }

    /// <summary>
    /// List of files from the queue whose processing is in an error state.
    /// </summary>
    public IReadOnlyList<string> MessageFilesFailed
    {
        get
        {
            if (m_cachedMessageFilesFailed == null)
            {
                m_cachedMessageFilesFailed = GetMessageFileList(x => x.Value == MessageFileState.FailedToWrite
                    || x.Value == MessageFileState.FailedToRead
                    || x.Value == MessageFileState.FailedToDelete);
            }
            return m_cachedMessageFilesFailed;
        }
    }
    #endregion  // Public properties

    #region Private methods
    /// <summary>
    /// Gets a list of files from the queue based on a given condition.
    /// </summary>
    private IReadOnlyList<string> GetMessageFileList(Func<KeyValuePair<string, MessageFileState>, bool> whereClause)
    {
        return MessageFiles.Where(whereClause).Select(x => x.Key).ToList();
    }

    /// <summary>
    /// Clears fields that contain cached values.
    /// </summary>
    private void ClearCacheFields()
    {
        foreach (var field in m_privateFields)
        {
            if (field.Name.StartsWith("m_cachedMessageFiles"))
            {
                field.SetValue(this, null);
            }
        }
    }
    #endregion  // Private methods
}