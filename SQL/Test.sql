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
VALUES (1, true);
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

INSERT INTO `WarehouseEntries` (WarehouseId, ItemId, Amount)
VALUES (1, 1, 5);
INSERT INTO `WarehouseEntries` (WarehouseId, ItemId, Amount)
VALUES (1, 2, 5);
INSERT INTO `WarehouseEntries` (WarehouseId, ItemId, Amount)
VALUES (1, 3, 5);
INSERT INTO `WarehouseEntries` (WarehouseId, ItemId, Amount)
VALUES (2, 1, 5);
INSERT INTO `WarehouseEntries` (WarehouseId, ItemId, Amount)
VALUES (2, 2, 5);
INSERT INTO `WarehouseEntries` (WarehouseId, ItemId, Amount)
VALUES (3, 1, 5);
INSERT INTO `WarehouseEntries` (WarehouseId, ItemId, Amount)
VALUES (3, 2, 5);
INSERT INTO `WarehouseEntries` (WarehouseId, ItemId, Amount)
VALUES (2, 3, 5);
INSERT INTO `WarehouseEntries` (WarehouseId, ItemId, Amount)
VALUES (4, 1, 5);
INSERT INTO `WarehouseEntries` (WarehouseId, ItemId, Amount)
VALUES (4, 2, 5);

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

INSERT INTO `Customers` (Id, Name, Money, Available)
VALUES (1, '胖墩', 100, true);

INSERT INTO `Deals` (Id, Time, WarehouseId, KeeperId, SalesmanId, CustomerId, InOrOut)
VALUES (1,'2007-01-01 10:00:00', 2, 1, 1, 1, true);
INSERT INTO `Deals` (Id, Time, WarehouseId, KeeperId, SalesmanId, CustomerId, InOrOut)
VALUES (2,'2010-01-01 10:00:00', 2, 1, 2, 1, false);
INSERT INTO `Deals` (Id, Time, WarehouseId, KeeperId, SalesmanId, CustomerId, InOrOut)
VALUES (3,'2011-01-01 10:00:00', 3, 1, 2, 1, true);
INSERT INTO `Deals` (Id, Time, WarehouseId, KeeperId, SalesmanId, CustomerId, InOrOut)
VALUES (4,'2015-01-01 10:00:00', 3, 1, 2, 1, false);
INSERT INTO `Deals` (Id, Time, WarehouseId, KeeperId, SalesmanId, CustomerId, InOrOut)
VALUES (5,'2017-01-01 10:00:00', 3, 1, 1, 1, true);
INSERT INTO `Deals` (Id, Time, WarehouseId, KeeperId, SalesmanId, CustomerId, InOrOut)
VALUES (6,'2018-01-01 10:00:00', 2, 1, 1, 1, false);
INSERT INTO `Deals` (Id, Time, WarehouseId, KeeperId, SalesmanId, CustomerId, InOrOut)
VALUES (7,'2019-02-01 10:00:00', 2, 1, 2, 1, true);
INSERT INTO `Deals` (Id, Time, WarehouseId, KeeperId, SalesmanId, CustomerId, InOrOut)
VALUES (8,'2019-04-01 10:00:00', 2, 1, 2, 1, false);
INSERT INTO `Deals` (Id, Time, WarehouseId, KeeperId, SalesmanId, CustomerId, InOrOut)
VALUES (9,'2019-05-21 10:00:00', 2, 1, 1, 1, true);


INSERT INTO `DealEntries` (DealId, ItemId, Prize, Amount, Note)
VALUES (1, 1, 5.00, 3, 'adsdasdas');
INSERT INTO `DealEntries` (DealId, ItemId, Prize, Amount, Note)
VALUES (1, 2, 4.00, 3, 'adsdasdas');
INSERT INTO `DealEntries` (DealId, ItemId, Prize, Amount, Note)
VALUES (1, 5, 7.00, 3, 'adsdasdas');
INSERT INTO `DealEntries` (DealId, ItemId, Prize, Amount, Note)
VALUES (1, 6, 15.00, 3, 'adsdasdas');
INSERT INTO `DealEntries` (DealId, ItemId, Prize, Amount, Note)
VALUES (2, 1, 5.00, 3, 'adsdasdas');
INSERT INTO `DealEntries` (DealId, ItemId, Prize, Amount, Note)
VALUES (2, 2, 5.00, 3, 'adsdasdas');
INSERT INTO `DealEntries` (DealId, ItemId, Prize, Amount, Note)
VALUES (2, 3, 5.00, 3, 'adsdasdas');
INSERT INTO `DealEntries` (DealId, ItemId, Prize, Amount, Note)
VALUES (3, 4, 5.00, 3, 'adsdasdas');
INSERT INTO `DealEntries` (DealId, ItemId, Prize, Amount, Note)
VALUES (3, 5, 5.00, 3, 'adsdasdas');
INSERT INTO `DealEntries` (DealId, ItemId, Prize, Amount, Note)
VALUES (3, 2, 5.00, 3, 'adsdasdas');
INSERT INTO `DealEntries` (DealId, ItemId, Prize, Amount, Note)
VALUES (3, 3, 5.00, 3, 'adsdasdas');
INSERT INTO `DealEntries` (DealId, ItemId, Prize, Amount, Note)
VALUES (5, 4, 5.00, 3, 'adsdasdas');
INSERT INTO `DealEntries` (DealId, ItemId, Prize, Amount, Note)
VALUES (5, 2, 5.00, 3, 'adsdasdas');