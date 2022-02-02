import { Item } from './item';

export interface IPaginationForItems {
    page: number;
    pageCount: number;
    count: number;
    data: Item[];
  }

export class PaginationForItems implements IPaginationForItems {
    page: number;
    pageCount: number;
    count: number;
    data: Item[];
  }
