export interface Discount {
    id: number;
    name: string;
    discountPercentage: number;
    startDate?: Date;
    endDate?: Date;
    minimumOrderValue?: number;
}