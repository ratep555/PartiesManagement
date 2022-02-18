import { Address } from './address';

export interface OrderClient {
    basketId: string;
    shippingOptionId: number;
    shippingAddress: Address;
}

export interface OrderItem {
    itemId: number;
    itemName: string;
    picture: string;
    price: number;
    quantity: number;
}

export interface Order {
    id: number;
    customerEmail: string;
    dateOfCreation: Date;
    shippingAddress: Address;
    shippingOption: string;
    shippingPrice: number;
    orderItems: OrderItem[];
    subtotal: number;
    getTotal: number;
    orderStatus: string;
    paymentStatus: string;
    orderStatus1Id?: number;
}
