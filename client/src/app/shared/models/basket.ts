import { v4 as uuidv4 } from 'uuid';

export interface Basket {
    id: string;
    basketItems: BasketItem[];
    clientSecret?: string;
    shippingOptionId?: number;
    paymentOptionId?: number;
    shippingPrice?: number;
    paymentIntentId?: string;
}

export interface BasketItem {
    id: number;
    itemName: string;
    price: number;
    quantity: number;
    picture: string;
    stockQuantity?: number;
}

export class BasketClass implements Basket {
    id = uuidv4();
    basketItems: BasketItem[] = [];
}

export interface BasketSum {
    shipping: number;
    subtotal: number;
    total: number;
}


