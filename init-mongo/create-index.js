db = db.getSiblingDB("MinhaBaseDeDados");

db.orders.createIndex(
    { IdempotencyKey: 1 },
    { unique: true, name: "ux_orders_idempotencyKey" }
);