import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Basket, BasketSum, BasketClass, BasketItem } from '../shared/models/basket';
import { Item } from '../shared/models/item';
import { PayingOption } from '../shared/models/payingOption';
import { ShippingOption } from '../shared/models/shippingOption';
import { WebshopService } from '../webshop/webshop.service';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketBSubject = new BehaviorSubject<Basket>(null);
  basket$ = this.basketBSubject.asObservable();
  private basketSumBSubject = new BehaviorSubject<BasketSum>(null);
  basketSum$ = this.basketSumBSubject.asObservable();
  shipping = 0;


  constructor(private http: HttpClient, private webshopService: WebshopService) { }

  gettingBasket(id: string) {
    return this.http.get(this.baseUrl + 'basket?id=' + id)
      .pipe(
        map((basket: Basket) => {
          this.basketBSubject.next(basket);
          this.shipping = basket.shippingPrice;
          this.calculateBasketSum1();
          console.log(basket);
        })
      );
  }

  editingBasket(basket: Basket) {
    return this.http.post(this.baseUrl + 'basket', basket).subscribe((response: Basket) => {
      this.basketBSubject.next(response);
      this.calculateBasketSum1();
    }, error => {
      console.log(error);
    });
  }

  editingBasket1(basket: Basket) {
    return this.http.post(this.baseUrl + 'basket', basket).subscribe((response: Basket) => {
      this.basketBSubject.next(response);
      this.calculateBasketSum1();
    }, error => {
      console.log(error);
    });
  }

  gettingValueOfBasket() {
    return this.basketBSubject.value;
  }

  addingItemToBasket(item: Item, quantity = 1) {
    const addedItem: BasketItem = this.mapItemToBasketItem1(item, quantity);
    const basket = this.gettingValueOfBasket() ?? this.creatingBasket();
    basket.basketItems = this.addingOrUpdatingBasketItem(basket.basketItems, addedItem, quantity);
    this.editingBasket(basket);
  }

  increaseBasketItemQuantity(basketitem: BasketItem) {
    const basket = this.gettingValueOfBasket();
    const itemIndex = basket.basketItems.findIndex(x => x.id === basketitem.id);
    basket.basketItems[itemIndex].quantity++;
    this.editingBasket(basket);
  }

  decreaseBasketItemQuantity(basketitem: BasketItem) {
    const basket = this.gettingValueOfBasket();
    const itemIndex = basket.basketItems.findIndex(x => x.id === basketitem.id);
    if (basket.basketItems[itemIndex].quantity > 1) {
      basket.basketItems[itemIndex].quantity--;
      this.editingBasket(basket);
    } else {
      this.removingItemFromBasket(basketitem);
    }
  }

  removingItemFromBasket(basketitem: BasketItem) {
    const basket = this.gettingValueOfBasket();
    if (basket.basketItems.some(x => x.id === basketitem.id)) {
      basket.basketItems = basket.basketItems.filter(i => i.id !== basketitem.id);
      if (basket.basketItems.length > 0) {
        this.editingBasket(basket);
      } else {
        this.deletingBasket(basket);
      }
    }
  }

  deletingBasket(basket: Basket) {
    return this.http.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe(() => {
      this.basketBSubject.next(null);
      this.basketSumBSubject.next(null);
      localStorage.removeItem('basket_id');
    }, error => {
      console.log(error);
    });
  }

  deletingBasketLocally(id: string) {
    this.basketBSubject.next(null);
    this.basketSumBSubject.next(null);
    localStorage.removeItem('basket_id');
  }

  calculateShippingPrice(shippingOption: ShippingOption) {
    this.shipping = shippingOption.price;
    const basket = this.gettingValueOfBasket();
    basket.shippingOptionId = shippingOption.id;
    basket.shippingPrice = shippingOption.price;
    this.calculateBasketSum1();
    this.editingBasket(basket);
  }

  persistingPayingOption(payingOption: PayingOption) {
    const basket = this.gettingValueOfBasket();
    basket.paymentOptionId = payingOption.id;
    this.editingBasket(basket);
  }

  createPaymentIntent() {
    return this.http.post(this.baseUrl + 'payments/' + this.gettingValueOfBasket().id, {})
      .pipe(
        map((basket: Basket) => {
          this.basketBSubject.next(basket);
        })
      );
  }

  private addingOrUpdatingBasketItem(basketitems: BasketItem[], itemToAdd: BasketItem,
                                     quantity: number): BasketItem[] {
    const index = basketitems.findIndex(i => i.id === itemToAdd.id);
    if (index === -1) {
      itemToAdd.quantity = quantity;
      basketitems.push(itemToAdd);
    } else {
      basketitems[index].quantity += quantity;
    }
    return basketitems;
  }



  private creatingBasket(): Basket {
    const basket = new BasketClass();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  private mapItemToBasketItem(item: Item, quantity: number): BasketItem {
    return {
      id: item.id,
      itemName: item.name,
      price: item.price,
      picture: item.picture,
      quantity
    };
  }

  private mapItemToBasketItem1(item: Item, quantity: number): BasketItem {
    return {
      id: item.id,
      itemName: item.name,
      price: item.price,
      picture: item.picture,
      stockQuantity: item.stockQuantity,
      discountedPrice: item.discountedPrice,
      quantity
    };
  }


  private calculateBasketSum() {
    const basket = this.gettingValueOfBasket();
    const shipping = this.shipping;
    const subtotal = basket.basketItems.reduce((a, b) => (b.price * b.quantity) + a, 0);
    const total = subtotal + shipping;
    this.basketSumBSubject.next({shipping, total, subtotal});
  }

    private calculateBasketSum1() {
    const basket = this.gettingValueOfBasket();
    const shipping = this.shipping;
    if (basket.basketItems.some(x => x.discountedPrice !== null)) {
      const discounttotal = basket.basketItems.filter(x => x.discountedPrice !== null);
      const regulartotal = basket.basketItems.filter(x => x.discountedPrice === null);
      const discounttotal1 = discounttotal.reduce((a, b) => (b.discountedPrice * b.quantity) + a, 0);
      const regulartotal1 = regulartotal.reduce((a, b) => (b.price * b.quantity) + a, 0);
      const subtotal = discounttotal1 + regulartotal1;
      const total = subtotal + shipping;
      this.basketSumBSubject.next({shipping, total, subtotal});
    } else {
      const subtotal = basket.basketItems.reduce((a, b) => (b.price * b.quantity) + a, 0);
      const total = subtotal + shipping;
      this.basketSumBSubject.next({shipping, total, subtotal});
    }
  }

  // moj pokušaj za addingitem
  addingItemToBasket1(item: Item, quantity = 1, stockQuantity = 1) {
    const addedItem: BasketItem = this.mapItemToBasketItem1(item, quantity);
    const basket = this.gettingValueOfBasket() ?? this.creatingBasket();
    basket.basketItems = this.addingOrUpdatingBasketItem1(basket.basketItems, addedItem, quantity, stockQuantity);
    this.editingBasket(basket);
  }

  private addingOrUpdatingBasketItem1(basketitems: BasketItem[], itemToAdd: BasketItem,
                                      quantity: number, stockQuantity: number): BasketItem[] {
const index = basketitems.findIndex(i => i.id === itemToAdd.id);
if (index === -1) {
itemToAdd.quantity = quantity;
itemToAdd.stockQuantity = quantity;
basketitems.push(itemToAdd);
} else {
basketitems[index].quantity += quantity;
basketitems[index].stockQuantity -= quantity;
}
return basketitems;
}

}






