export interface BirthdayCreate {
    location1Id: number;
    birthdayPackageId: number;
    clientName: string;
    birthdayGirlBoyName: string;
    contactEmail: string;
    contactPhone: string;
    numberOfGuests: number;
    birthdayNo: number;
    startDateAndTime: Date;
    }

export class BirthdayEdit {
    id: number;
    location1Id: number;
    birthdayPackageId: number;
    clientName: string;
    birthdayGirlBoyName: string;
    contactEmail: string;
    contactPhone: string;
    numberOfGuests: number;
    birthdayNo: number;
    startDateAndTime: Date;
    endDateAndTime: Date;
    orderStatus1Id?: number;
    }

export interface BirthdayByIdForEdit {
    location1Id: number;
    birthdayPackageId: number;
    clientName: string;
    birthdayGirlBoyName: string;
    contactEmail: string;
    contactPhone: string;
    numberOfGuests: number;
    birthdayNo: number;
    startDateAndTime: Date;
    endDateAndTime: Date;
    orderStatus1Id?: number;
    }

export interface Birthday {
    id: number;
    location: string;
    birthdayPackage: string;
    orderStatus: string;
    clientName: string;
    birthdayGirlBoyName: string;
    contactPhone: string;
    contactEmail: string;
    numberOfGuests: number;
    remarks?: any;
    birthdayNo: number;
    price: number;
    startDateAndTime: Date;
    endDateAndTime: Date;
    orderStatus1Id?: number;
    }



