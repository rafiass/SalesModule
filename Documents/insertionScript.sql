--
--	Insert sales for tests:
--


declare @saleid as integer
declare @pcid as integer
set @pcid = 1

--buy & get
--First sale: buy iphone6(pluno: 10017) and get samsung recharger (pluno: 10007) for half price(50%)
insert into sales (Empno, Title) values(9999, 'firstSale')
select @saleid = max(saleID) from sales
insert into SalesPcid (SaleID, PCID, DateFrom, DateTo, HourFrom, HourTo) values (@saleid, @pcid, '2015-09-05', '2015-12-01', '00:00:00', '23:59:59')

insert into PluReqSale (SaleID, pluID, isPluno, qty) values (@saleid, 10017, 1, 1)
insert into PluOutSale (SaleID, pluID, isPluno, offPrice, offType) values (@saleid, 10007, 1, 50, 2)

--bundle
--Second sale: buy iphone63(pluno: 10019) and a case(pluno: 10020) for only 200 NIS
insert into sales (Empno, Title, TotalOffPrice, TotalOffType) values(9999, 'secondSale', 200, 3)
select @saleid = max(saleID) from sales
insert into SalesPcid (SaleID, PCID, DateFrom, DateTo, HourFrom, HourTo) values (@saleid, @pcid, '2015-09-05', '2015-12-01', '00:00:00', '23:59:59')

insert into PluReqSale (SaleID, pluID, isPluno, qty) values (@saleid, 10019, 1, 1)
insert into PluReqSale (SaleID, pluID, isPluno, qty) values (@saleid, 10020, 1, 1)

--receipt
--Third sale: buy over 1500 NIS (total receipt), and get 20% discount on iphone6(pluno: 10022)
insert into sales (Empno, Title, MinTotalPrice) values(9999, 'thirdSale', 1500)
select @saleid = max(saleID) from sales
insert into SalesPcid (SaleID, PCID, DateFrom, DateTo, HourFrom, HourTo) values (@saleid, @pcid, '2015-06-05', '2015-07-01', '00:00:00', '23:59:59')

insert into PluOutSale (SaleID, pluID, isPluno, offPrice, offType) values (@saleid, 10022, 1, 20, 2)

