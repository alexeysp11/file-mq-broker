CREATE TABLE MessageFiles (
    MessageFileId INTEGER PRIMARY KEY,
    Name TEXT NOT NULL,
    MessageFileState TEXT NOT NULL
);

CREATE TABLE FileContents (
    FileContentsId INTEGER PRIMARY KEY,
    MessageFileId INTEGER NOT NULL,
    Content BLOB,
    FOREIGN KEY (MessageFileId) REFERENCES MessageFiles(MessageFileId)
);

CREATE TABLE FileMetadata (
    FileMetadataId INTEGER PRIMARY KEY,
    MessageFileId INTEGER NOT NULL,
    Size INTEGER,
    CreatedAt DATETIME,
    FOREIGN KEY (MessageFileId) REFERENCES MessageFiles(MessageFileId)
);

CREATE TABLE FileStateHistory (
    FileStateHistoryId INTEGER PRIMARY KEY,
    MessageFileId INTEGER NOT NULL,
    NewState TEXT NOT NULL,
    Timestamp DATETIME,
    FOREIGN KEY (MessageFileId) REFERENCES MessageFiles(MessageFileId)
);
