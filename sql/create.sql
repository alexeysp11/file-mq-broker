CREATE TABLE MessageFiles (
    MessageFileId INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Size INTEGER,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    MessageFileState TEXT NOT NULL
);

CREATE TABLE FileStateHistory (
    FileStateHistoryId INTEGER PRIMARY KEY AUTOINCREMENT,
    MessageFileId INTEGER NOT NULL,
    NewState TEXT NOT NULL,
    Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (MessageFileId) REFERENCES MessageFiles(MessageFileId)
);

CREATE TABLE ExceptionLog (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ExceptionMessage TEXT NOT NULL,
    StackTrace TEXT,
    Timestamp DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);
