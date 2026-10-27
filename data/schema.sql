-- ProdMgr (legacy "생산실적관리") database schema.
-- This is the schema the legacy WinForms app ships with. The modernized app reads it as-is;
-- any change must go through a migration script.

PRAGMA foreign_keys = OFF;

CREATE TABLE IF NOT EXISTS Item (
    ItemCode    TEXT    NOT NULL PRIMARY KEY,
    ItemName    TEXT    NOT NULL,
    Unit        TEXT    NOT NULL DEFAULT 'EA'
);

CREATE TABLE IF NOT EXISTS DefectCode (
    Code        TEXT    NOT NULL PRIMARY KEY,
    Name        TEXT    NOT NULL
);

-- LineCode is a free-text code; the list of lines lives in the INI file, not in the DB.
-- WorkDate: 'yyyy-MM-dd'. Shift: '주간' or '야간'.
CREATE TABLE IF NOT EXISTS WorkOrder (
    WorkOrderId INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderNo     TEXT    NOT NULL,
    ItemCode    TEXT    NOT NULL,
    LineCode    TEXT    NOT NULL,
    PlanQty     INTEGER NOT NULL,
    WorkDate    TEXT    NOT NULL,
    Shift       TEXT    NOT NULL,
    Remark      TEXT,
    CreatedAt   TEXT    NOT NULL
);

-- One work order can have several result entries.
-- StartTime / EndTime: 'HH:mm' (no date part). EnteredAt: 'yyyy-MM-dd HH:mm:ss'.
CREATE TABLE IF NOT EXISTS ProductionResult (
    ResultId    INTEGER PRIMARY KEY AUTOINCREMENT,
    WorkOrderId INTEGER NOT NULL,
    GoodQty     INTEGER NOT NULL DEFAULT 0,
    DefectQty   INTEGER NOT NULL DEFAULT 0,
    DefectCode  TEXT,
    StartTime   TEXT    NOT NULL,
    EndTime     TEXT    NOT NULL,
    EnteredAt   TEXT    NOT NULL
);

CREATE INDEX IF NOT EXISTS IX_WorkOrder_WorkDate ON WorkOrder (WorkDate);
