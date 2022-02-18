import { Category } from './category';
import { Item } from './item';
import { Manufacturer1 } from './manufacturer';

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
    categories: Category[];
}

export class DiscountEditClass {
    id: number;
    name: string;
    discountPercentage: number;
    startDate: Date;
    endDate: Date;
    minimumOrderValue?: number;
    items: Item[];
    categories: Category[];
    manufacturers: Manufacturer1[];
}

export interface DiscountPutGet {
    discount: Discount;
    selectedItems: Item[];
    nonSelectedItems: Item[];
    selectedCategories: Category[];
    nonSelectedCategories: Category[];
    nonSelectedManufacturers: Manufacturer1[];
    selectedManufacturers: Manufacturer1[];
}







