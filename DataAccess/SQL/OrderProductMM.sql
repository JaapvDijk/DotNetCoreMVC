SELECT o.Id as OrderId, AVG(p.Price) avg_prod_price
FROM Products p
JOIN OrderProduct op ON p.Id = op.ProductsId
JOIN Orders o ON op.OrdersId = o.Id
GROUP BY o.Id