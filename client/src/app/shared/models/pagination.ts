import { Discount } from './discount';
import { Item } from './item';

export interface IPaginationForItems {
    page: number;
    pageCount: number;
    count: number;
    data: Item[];
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
