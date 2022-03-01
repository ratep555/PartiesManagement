export interface Location1 {
    id: number;
    street: string;
    city: string;
    description: string;
    latitude: number;
    longitude: number;
    picture: string;
    workingHours: string;
    email: string;
    phone: string;
    country: string;
}

export interface LocationCreateEdit {
    id: number;
    street: string;
    city: string;
    description: string;
    latitude: number;
    longitude: number;
    picture: string;
    workingHours: string;
    email: string;
    phone: string;
    countryId: number;
}

export class LocationCreateEdit {
    id: number;
    street: string;
    city: string;
    description: string;
    latitude: number;
    longitude: number;
    picture: string;
    workingHours: string;
    email: string;
    phone: string;
    countryId: number;
}



