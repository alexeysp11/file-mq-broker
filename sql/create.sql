CREATE TABLE MessageFiles (
    MessageFileId INTEGER PRIMARY KEY,
    Name TEXT NOT NULL,
    Content BLOB,
    ContentText TEXT,
    Size INTEGER,
    CreatedAt DATETIME,
    MessageFileState TEXT NOT NULL
);

CREATE TABLE FileStateHistory (
    FileStateHistoryId INTEGER PRIMARY KEY,
    MessageFileId INTEGER NOT NULL,
    NewState TEXT NOT NULL,
    Timestamp DATETIME,
    FOREIGN KEY (MessageFileId) REFERENCES MessageFiles(MessageFileId)
);

CREATE TABLE ExceptionLog (
    Id INTEGER PRIMARY KEY,
    ExceptionMessage TEXT NOT NULL,
    StackTrace TEXT NOT NULL,
    Timestamp DATETIME NOT NULL
);
