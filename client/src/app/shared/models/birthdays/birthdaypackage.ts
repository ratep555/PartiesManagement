import { Discount } from '../discount';
import { ServiceIncluded } from './serviceincluded';

export interface BirthdayPackage {
    id: number;
    packageName: string;
    description: string;
    numberOfParticipants: number;
    price: number;
    additionalBillingPerParticipant: number;
    duration: number;
    picture: string;
    discountSum: number;
    discountedPrice?: number;
    hasDiscountsApplied?: boolean;
    servicesIncluded: ServiceIncluded[];
}

export interface BirthdayPackageCreateEdit {
    id: number;
    packageName: string;
    description: string;
    numberOfParticipants: number;
    price: number;
    additionalBillingPerParticipant: number;
    duration: number;
    picture: File;
    discountsIds: number[];
    servicesIds: number[];
}

export class BirthdayPackageCreateEditClass {
    id: number;
    packageName: string;
    description: string;
    numberOfParticipants: number;
    price: number;
    additionalBillingPerParticipant: number;
    duration: number;
    picture: File;
    discountsIds: number[];
    servicesIds: number[];
}

export interface BirthdayPackagePutGet {
    BirthdayPackage: BirthdayPackage;
    selectedServices: ServiceIncluded[];
    nonSelectedServices: ServiceIncluded[];
    selectedDiscounts: Discount[];
    nonSelectedDiscounts: Discount[];
}










