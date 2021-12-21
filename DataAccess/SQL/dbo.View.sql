CREATE VIEW dbo.Stats
	AS SELECT COUNT(dbo.Products.Id) as NumberOfProducts
	FROM dbo.Products
