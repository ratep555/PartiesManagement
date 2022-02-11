import { Category } from './category';
import { Discount } from './discount';
import { Manufacturer } from './manufacturer';
import { Tag } from './tag';

export interface Item {
    id: number;
    name: string;
    description: string;
    price: number;
    picture: string;
    averageVote: number;
    userVote: number;
    count: number;
    stockQuantity?: number;
    discounts: Discount[];
}

export interface ItemCreateEdit {
    name: string;
    description: string;
    price: number;
    picture: File;
    categoriesIds: number[];
    manufacturersIds: number[];
    tagsIds: number[];
    discountsIds: number[];
}

export interface ItemEdit {
    id: number;
    name: string;
    description: string;
    price: number;
    picture: File;
    categoriesIds: number[];
    manufacturersIds: number[];
    tagsIds: number[];
    discountsIds: number[];

}

export interface ItemPutGet {
    item: Item;
    selectedCategories: Category[];
    nonSelectedCategories: Category[];
    selectedDiscounts: Discount[];
    nonSelectedDiscounts: Discount[];
    selectedManufacturers: Manufacturer[];
    nonSelectedManufacturers: Manufacturer[];
    selectedTags: Tag[];
    nonSelectedTags: Tag[];
}
