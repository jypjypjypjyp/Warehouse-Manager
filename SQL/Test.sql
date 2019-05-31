-- SQLite
INSERT INTO `Items` (Id, Name, Specification, Unit, Available)
VALUES (1,	'钢笔',	'小',	'支',true);
INSERT INTO `Items` (Id, Name, Specification, Unit, Available)
VALUES (2, '笔记本',	'精美',	'本',true);
INSERT INTO `Items` (Id, Name, Specification, Unit, Available)
VALUES (3,'小零食',	'大号',	'包',true);
INSERT INTO `Items` (Id, Name, Unit, Available)
VALUES (4,'上衣',	'件',true);
INSERT INTO `Items` (Id, Name, Unit, Available)
VALUES (5,'C++从入门到放弃'	,'本',true);
INSERT INTO `Items` (Id, Name, Specification, Unit, Available)
VALUES (6,'电脑'	,'高配'	,'台',true);
INSERT INTO `Items` (Id, Name, Specification, Unit, Available)
VALUES (7,'电脑'	,'低配'	,'台',true);

INSERT INTO `Warehouses` (Id, Available)
VALUES (2, true);
INSERT INTO `Warehouses` (Id, Available)
VALUES (3, true);
INSERT INTO `Warehouses` (Id, Available)
VALUES (4, true);
INSERT INTO `Warehouses` (Id, Available)
VALUES (5, true);
INSERT INTO `Warehouses` (Id, Available)
VALUES (6, true);
INSERT INTO `Warehouses` (Id, Available)
VALUES (7, true);
INSERT INTO `Warehouses` (Id, Available)
VALUES (8, true);
INSERT INTO `Warehouses` (Id, Available)
VALUES (9, true);
INSERT INTO `Warehouses` (Id, Available)
VALUES (10, true);
INSERT INTO `Warehouses` (Id, Available)
VALUES (11, true);
INSERT INTO `Warehouses` (Id, Available)
VALUES (12, true);

INSERT INTO `Keepers` (Id, Name, Contact, Password, Available)
VALUES (1,'abc','88888888','123', true);

INSERT INTO `Salesmen` (Id, Name, Available)
VALUES (1, '张三', true);
INSERT INTO `Salesmen` (Id, Name, Available)
VALUES (2, '李四', true);
INSERT INTO `Salesmen` (Id, Name, Available)
VALUES (3, '王五', true);
INSERT INTO `Salesmen` (Id, Name, Available)
VALUES (4, '蛤六', true);
INSERT INTO `Salesmen` (Id, Name, Available)
VALUES (5, '长者', true);

-- SQLite
INSERT INTO `Deals` (Id, Time, WarehouseId, KeeperId, SalesmanId, InOrOut)
VALUES (1,'2007-01-01 10:00:00', 2, 1, 1, true);
INSERT INTO `Deals` (Id, Time, WarehouseId, KeeperId, SalesmanId, InOrOut)
VALUES (2,'2010-01-01 10:00:00', 2, 1, 2, false);
INSERT INTO `Deals` (Id, Time, WarehouseId, KeeperId, SalesmanId, InOrOut)
VALUES (3,'2011-01-01 10:00:00', 3, 1, 2, true);
INSERT INTO `Deals` (Id, Time, WarehouseId, KeeperId, SalesmanId, InOrOut)
VALUES (4,'2015-01-01 10:00:00', 3, 1, 2, false);
INSERT INTO `Deals` (Id, Time, WarehouseId, KeeperId, SalesmanId, InOrOut)
VALUES (5,'2017-01-01 10:00:00', 3, 1, 1, true);
INSERT INTO `Deals` (Id, Time, WarehouseId, KeeperId, SalesmanId, InOrOut)
VALUES (6,'2018-01-01 10:00:00', 2, 1, 1, false);
INSERT INTO `Deals` (Id, Time, WarehouseId, KeeperId, SalesmanId, InOrOut)
VALUES (7,'2019-02-01 10:00:00', 2, 1, 2, true);
INSERT INTO `Deals` (Id, Time, WarehouseId, KeeperId, SalesmanId, InOrOut)
VALUES (8,'2019-04-01 10:00:00', 2, 1, 2, false);
INSERT INTO `Deals` (Id, Time, WarehouseId, KeeperId, SalesmanId, InOrOut)
VALUES (9,'2019-05-021 10:00:00', 2, 1, 1, true);

