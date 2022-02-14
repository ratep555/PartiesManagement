import { Item } from './item';

export interface Discount {
    id: number;
    name: string;
    discountPercentage: number;
    startDate: Date;
    endDate: Date;
    minimumOrderValue?: number;
    items: Item[];
}

export interface DiscountCreateEdit {
    id: number;
    name: string;
    discountPercentage: number;
    startDate: Date;
    endDate: Date;
    minimumOrderValue?: number;
    items: Item[];
}

export class DiscountEditClass {
    id: number;
    name: string;
    discountPercentage: number;
    startDate: Date;
    endDate: Date;
    minimumOrderValue?: number;
    items: Item[];
}

export interface DiscountPutGet {
    discount: Discount;
    selectedItems: Item[];
    nonSelectedItems: Item[];
}
