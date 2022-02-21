export interface ItemWarehouse {
    itemId: number;
    warehouseId: number;
    stockQuantity: number;
    reservedQuantity?: number;
    item: string;
    warehouse: string;
}
