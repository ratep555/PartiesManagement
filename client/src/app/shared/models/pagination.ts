import { Discount } from './discount';
import { Item } from './item';
import { ItemWarehouse } from './itemWarehouses';

export interface IPaginationForItems {
    page: number;
    pageCount: number;
    count: number;
    data: Item[];
  }

export interface IPaginationForItemWarehouses {
    page: number;
    pageCount: number;
    count: number;
    data: ItemWarehouse[];
  }

export interface IPaginationForDiscounts {
    page: number;
    pageCount: number;
    count: number;
    data: Discount[];
  }

export class PaginationForItems implements IPaginationForItems {
    page: number;
    pageCount: number;
    count: number;
    data: Item[];
  }


