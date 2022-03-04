import { Birthday } from './birthdays/birthday';
import { BirthdayPackage } from './birthdays/birthdaypackage';
import { ServiceIncluded } from './birthdays/serviceincluded';
import { Blog } from './blog';
import { Discount } from './discount';
import { Item } from './item';
import { ItemWarehouse } from './itemWarehouses';
import { Location1 } from './location';

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

export interface IPaginationForLocations {
    page: number;
    pageCount: number;
    count: number;
    data: Location1[];
  }

export interface IPaginationForBlogs {
    page: number;
    pageCount: number;
    count: number;
    data: Blog[];
  }

export class PaginationForBlogs implements IPaginationForBlogs {
    page: number;
    pageCount: number;
    count: number;
    data: Blog[];
  }

export interface IPaginationFofServicesIncluded {
    page: number;
    pageCount: number;
    count: number;
    data: ServiceIncluded[];
  }

export class PaginationForServicesIncluded implements IPaginationFofServicesIncluded {
    page: number;
    pageCount: number;
    count: number;
    data: ServiceIncluded[];
  }

export interface IPaginationForDiscounts {
    page: number;
    pageCount: number;
    count: number;
    data: Discount[];
  }

export interface IPaginationForBirthdays {
    page: number;
    pageCount: number;
    count: number;
    data: Birthday[];
  }

export class PaginationForBirthdays implements IPaginationForBirthdays {
    page: number;
    pageCount: number;
    count: number;
    data: Birthday[];
  }

export interface IPaginationForBirthdayPackages {
    page: number;
    pageCount: number;
    count: number;
    data: BirthdayPackage[];
  }

export class PaginationForBirthdayPackages implements IPaginationForBirthdayPackages{
    page: number;
    pageCount: number;
    count: number;
    data: BirthdayPackage[];
  }

export class PaginationForItems implements IPaginationForItems {
    page: number;
    pageCount: number;
    count: number;
    data: Item[];
  }


