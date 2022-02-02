export interface Item {
    id: number;
    name: string;
    description: string;
    price: number;
    picture: string;
}

export interface ItemCreateEdit {
    id: number;
    name: string;
    description: string;
    price: number;
    picture: File;
}
